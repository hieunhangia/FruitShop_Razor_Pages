using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Repository;
using Repository.Constants;

namespace Service;

public partial class FileService(IMinioClient minioClient, IConfiguration configuration, ILogger<FileService> logger, BusinessRuleService businessRuleService)
{
    private readonly string _publicBucketName = configuration["MinioSettings:PublicBucketName"]!;
    private readonly string _privateBucketName = configuration["MinioSettings:PrivateBucketName"]!;
    private readonly string _endpoint = configuration["MinioSettings:Endpoint"]!;
    private readonly bool _useSSL = bool.Parse(configuration["MinioSettings:UseSSL"]!);

    [GeneratedRegex(BusinessRuleConstants.FileService.PathRegexPattern)]
    private static partial Regex PathRegex();

    public async Task<string> UploadProductImageAsync(IFormFile file) =>
        await UploadFileAsync(file, BusinessRuleConstants.FileService.ProductImagesPath);

    public async Task<string> UploadFileAsync(IFormFile file, string? path = null, bool isPublic = true)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentNullException(nameof(file), "Tệp không hợp lệ hoặc không có dữ liệu.");
        }

        var bucketName = isPublic ? _publicBucketName : _privateBucketName;
        try
        {
            if (!await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName)))
            {
                logger.LogError(
                    "Bucket '{BucketName}' not found. Please check your Minio configuration and ensure the bucket exists.",
                    bucketName);
                throw new Exception("Đã xảy ra lỗi khi tải lên tệp. Vui lòng thử lại sau.");
            }

            var filePath = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            if (!string.IsNullOrWhiteSpace(path))
            {
                if (!PathRegex().IsMatch(path))
                {
                    throw new ArgumentException(
                        "Path không hợp lệ. Định dạng yêu cầu: \"a/b/c/\". Chỉ dùng chữ, số, khoảng trắng, gạch ngang (-), gạch dưới (_).",
                        nameof(path));
                }

                filePath = $"{path}{filePath}";
            }

            await using var stream = file.OpenReadStream();
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(filePath)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(file.ContentType);

            await minioClient.PutObjectAsync(putObjectArgs);
            return filePath;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error uploading {FileName} to Minio bucket {BucketName}", file.FileName, bucketName);
            throw new Exception("Đã xảy ra lỗi khi tải lên tệp. Vui lòng thử lại sau.");
        }
    }

    public string GetPublicFileUrl(string filePath)
    {
        var protocol = _useSSL ? "https" : "http";
        return $"{protocol}://{_endpoint.TrimEnd('/')}/{_publicBucketName.Trim().Trim('/')}/" +
               string.Join("/", filePath.TrimStart('/').Split('/').Select(Uri.EscapeDataString));
    }

    public async Task<string> GetPrivateFileUrlAsync(string filePath)
    {
        try
        {
            return await minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(_privateBucketName)
                .WithObject(filePath)
                .WithExpiry(businessRuleService.GetValue<int>(BusinessRuleConstantType.PrivateFileUrlExpirationSeconds)));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error generating file URL for {FilePath} in Minio bucket {BucketName}", filePath,
                _privateBucketName);
            return string.Empty;
        }
    }

    public async Task DeleteFileAsync(string filePath, bool isPublic = true)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("Đường dẫn tệp không hợp lệ.", nameof(filePath));
        }

        var targetBucket = isPublic ? _publicBucketName : _privateBucketName;
        try
        {
            await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(targetBucket)
                .WithObject(filePath));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting file {FilePath} from Minio bucket {BucketName}", filePath, targetBucket);
            throw new Exception("Đã xảy ra lỗi khi xóa tệp. Vui lòng thử lại sau.");
        }
    }
}
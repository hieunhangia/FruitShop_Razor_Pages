using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Repository;

namespace Service;

public partial class FileService(IMinioClient minioClient, IConfiguration configuration, ILogger<FileService> logger)
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
            var beArgs = new BucketExistsArgs().WithBucket(bucketName);
            if (!await minioClient.BucketExistsAsync(beArgs))
            {
                throw new Exception(
                    $"Bucket '{bucketName}' not found. Please check your Minio configuration and ensure the bucket exists.");
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
            logger.LogError(e, "Error uploading file to Minio");
            throw new Exception("Đã xảy ra lỗi khi tải lên tệp. Vui lòng thử lại sau.");
        }
    }


    public async Task<string> GetFileUrlAsync(string filePath, bool isPublic = true)
    {
        try
        {
            if (!isPublic)
            {
                return await minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                    .WithBucket(_privateBucketName)
                    .WithObject(filePath)
                    .WithExpiry(BusinessRuleConstants.FileService.PrivateFileUrlExpirationSeconds));
            }

            var protocol = _useSSL ? "https" : "http";
            return $"{protocol}://{_endpoint}/{_publicBucketName}/{filePath}";
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error generating file URL from Minio");
            return string.Empty;
        }
    }

    public async Task DeleteFileAsync(string filePath, bool isPublic = true)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("Đường dẫn tệp không hợp lệ.", nameof(filePath));
        }

        try
        {
            var targetBucket = isPublic ? _publicBucketName : _privateBucketName;
            await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(targetBucket)
                .WithObject(filePath));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting file from Minio");
            throw new Exception("Đã xảy ra lỗi khi xóa tệp. Vui lòng thử lại sau.");
        }
    }
}
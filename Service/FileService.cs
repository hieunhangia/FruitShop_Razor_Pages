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
        await UploadPublicFileAsync(file, BusinessRuleConstants.FileService.ProductImagesPath);

    public async Task<string> UploadPublicFileAsync(IFormFile file, string? path = null)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentNullException(nameof(file), "Tệp không hợp lệ hoặc không có dữ liệu.");
        }

        try
        {
            var beArgs = new BucketExistsArgs().WithBucket(_publicBucketName);
            if (!await minioClient.BucketExistsAsync(beArgs))
            {
                throw new Exception(
                    $"Bucket '{_publicBucketName}' not found. Please check your Minio configuration and ensure the bucket exists.");
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
                .WithBucket(_publicBucketName)
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

    public async Task<string> UploadPrivateFileAsync(IFormFile file, string? path = null)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentNullException(nameof(file), "Tệp không hợp lệ hoặc không có dữ liệu.");
        }

        try
        {
            var beArgs = new BucketExistsArgs().WithBucket(_privateBucketName);
            if (!await minioClient.BucketExistsAsync(beArgs))
            {
                throw new Exception(
                    $"Bucket '{_privateBucketName}' not found. Please check your Minio configuration and ensure the bucket exists.");
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
                .WithBucket(_privateBucketName)
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

    public string GetPublicFileUrl(string filePath)
    {
        var protocol = _useSSL ? "https" : "http";
        return $"{protocol}://{_endpoint}/{_publicBucketName}/{filePath}";
    }

    public async Task<string> GetPrivateFileUrlAsync(string filePath)
    {
        try
        {
            return await minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(_privateBucketName)
                .WithObject(filePath)
                .WithExpiry(BusinessRuleConstants.FileService.PrivateFileUrlExpirationSeconds));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error generating file URL from Minio");
            return string.Empty;
        }
    }

    public async Task DeletePublicFileAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("Đường dẫn tệp không hợp lệ.", nameof(filePath));
        }

        try
        {
            await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_publicBucketName)
                .WithObject(filePath));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting file from Minio");
            throw new Exception("Đã xảy ra lỗi khi xóa tệp. Vui lòng thử lại sau.");
        }
    }

    public async Task DeletePrivateFileAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("Đường dẫn tệp không hợp lệ.", nameof(filePath));
        }

        try
        {
            await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_privateBucketName)
                .WithObject(filePath));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting file from Minio");
            throw new Exception("Đã xảy ra lỗi khi xóa tệp. Vui lòng thử lại sau.");
        }
    }
}
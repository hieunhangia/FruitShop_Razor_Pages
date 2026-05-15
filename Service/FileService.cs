using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Repository;

namespace Service;

public class FileService(IMinioClient minioClient, IConfiguration configuration, ILogger<FileService> logger)
{
    private readonly string _publicBucketName = configuration["MinioSettings:PublicBucketName"]!;
    private readonly string _privateBucketName = configuration["MinioSettings:PrivateBucketName"]!;
    private readonly string _endpoint = configuration["Minio:Endpoint"]!;
    private readonly bool _useSSL = bool.Parse(configuration["Minio:UseSSL"]!);

    public async Task<string> UploadFileAsync(IFormFile file, bool isPublic = true)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentNullException(nameof(file), "Tệp không hợp lệ hoặc không có dữ liệu.");

        var bucketName = isPublic ? _publicBucketName : _privateBucketName;
        try
        {
            var beArgs = new BucketExistsArgs().WithBucket(bucketName);
            if (!await minioClient.BucketExistsAsync(beArgs))
            {
                throw new Exception(
                    $"Bucket '{bucketName}' not found. Please check your Minio configuration and ensure the bucket exists.");
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            await using var stream = file.OpenReadStream();
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(file.ContentType);

            await minioClient.PutObjectAsync(putObjectArgs);
            return fileName;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error uploading file to Minio");
            throw new Exception("Đã xảy ra lỗi khi tải lên tệp. Vui lòng thử lại sau.");
        }
    }

    public async Task<string> GetFileUrlAsync(string fileName, bool isPublic = true)
    {
        if (isPublic)
        {
            var protocol = _useSSL ? "https" : "http";
            return $"{protocol}://{_endpoint}/{_publicBucketName}/{fileName}";
        }

        var args = new PresignedGetObjectArgs()
            .WithBucket(_privateBucketName)
            .WithObject(fileName)
            .WithExpiry(BusinessRuleConstants.FileService.PrivateFileUrlExpirationSeconds);

        return await minioClient.PresignedGetObjectAsync(args);
    }

    public async Task DeleteFileAsync(string fileName, bool isPublic = true)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Tên file không được để trống");

        var targetBucket = isPublic ? _publicBucketName : _privateBucketName;

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(targetBucket)
            .WithObject(fileName);

        await minioClient.RemoveObjectAsync(removeObjectArgs);
    }
}
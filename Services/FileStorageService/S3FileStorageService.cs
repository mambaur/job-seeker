using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSeeker.Services.FileStorageService
{
    public class S3FileStorageService
    {
        // private readonly IAmazonS3 _s3Client;
        // private readonly string _bucketName;

        // public S3FileStorageService(IAmazonS3 s3Client, string bucketName)
        // {
        //     _s3Client = s3Client;
        //     _bucketName = bucketName;
        // }

        // public async Task<string> SaveFileAsync(IFormFile file)
        // {
        //     using (var stream = file.OpenReadStream())
        //     {
        //         var putRequest = new PutObjectRequest
        //         {
        //             BucketName = _bucketName,
        //             Key = file.FileName,
        //             InputStream = stream,
        //             ContentType = file.ContentType
        //         };
        //         await _s3Client.PutObjectAsync(putRequest);
        //     }
        //     return file.FileName;
        // }

        // public async Task DeleteFileAsync(string fileName)
        // {
        //     var deleteRequest = new DeleteObjectRequest
        //     {
        //         BucketName = _bucketName,
        //         Key = fileName
        //     };
        //     await _s3Client.DeleteObjectAsync(deleteRequest);
        // }

        // public Task<string> GetFileUrlAsync(string fileName)
        // {
        //     return Task.FromResult($"https://{_bucketName}.s3.amazonaws.com/{fileName}");
        // }
    }
}
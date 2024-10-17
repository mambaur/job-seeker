using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSeeker.Services.FileStorageService
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file);
        Task DeleteFileAsync(string fileName);
        Task<string> GetFileUrlAsync(string fileName);
    }
}
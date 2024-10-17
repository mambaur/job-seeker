
namespace JobSeeker.Services.FileStorageService
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _storagePath;

        public LocalFileStorageService(string storagePath)
        {
            _storagePath = storagePath;

            // Membuat folder jika belum ada
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_storagePath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }

        public Task<string> GetFileUrlAsync(string fileName)
        {
            return Task.FromResult(Path.Combine(_storagePath, fileName));
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            // Membuat folder jika belum ada
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string FileName = $"{timestamp} {file.FileName}";

            var filePath = Path.Combine(_storagePath, FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return FileName;
        }
    }
}
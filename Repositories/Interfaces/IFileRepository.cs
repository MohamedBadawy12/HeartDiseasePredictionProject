using Microsoft.AspNetCore.Http;

namespace Repositories.Interfaces
{
    public interface IFileService
    {
        Tuple<int, string> SaveImage(IFormFile imageFile);
        public Task<string> UploadAsync(IFormFile imageFile, string location);
        public bool DeleteImage(string imageFileName);
    }
}

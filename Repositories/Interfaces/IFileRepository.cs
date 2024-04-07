using Microsoft.AspNetCore.Http;

namespace Repositories.Interfaces
{
    public interface IFileRepository
    {
        Tuple<int, string> SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
    }
}

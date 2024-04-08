using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Repositories.Interfaces;

namespace Repositories
{
    public class FileService : IFileService
    {
        IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var wwwPath = this._webHostEnvironment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Check the allowed extenstions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);

            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "Error has occured");
            }
        }

        public bool DeleteImage(string imageFileName)
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + imageFileName);
            if (File.Exists(directoryPath))
            {
                File.Delete(directoryPath);
                return true;
            }
            return false;
        }

        public async Task<string> UploadAsync(IFormFile imageFile, string location)
        {
            try
            {
                var path = _webHostEnvironment.WebRootPath + location;
                var extension = Path.GetExtension(imageFile.FileName);
                var fileName = Guid.NewGuid().ToString().Replace("-", string.Empty) + extension;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (FileStream fileStream = File.Create(path + fileName))
                {
                    await imageFile.CopyToAsync(fileStream);
                    fileStream.Flush();
                    return $"{location}/{fileName}";
                }
            }
            catch
            {
                return "An Problem occured when creating file";
            }
        }
    }
}

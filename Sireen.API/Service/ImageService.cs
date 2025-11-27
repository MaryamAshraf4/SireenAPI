using Sireen.API.Interfaces.IService;

namespace Sireen.API.Service
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<string> UploadImageAsync(IFormFile file, string folderName)
        {
            var wwwRootPath = _env.WebRootPath;
            var folderPath = Path.Combine(wwwRootPath, folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{folderName}/{fileName}";
        }
    }
}

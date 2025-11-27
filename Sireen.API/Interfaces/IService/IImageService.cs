namespace Sireen.API.Interfaces.IService
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file, string folderName);
    }
}

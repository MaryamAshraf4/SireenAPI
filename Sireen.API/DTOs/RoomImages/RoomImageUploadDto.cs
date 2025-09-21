namespace Sireen.API.DTOs.RoomImages
{
    public class RoomImageUploadDto
    {
        public int RoomId { get; set; }
        public IFormFile Image { get; set; } = null!;
    }
}

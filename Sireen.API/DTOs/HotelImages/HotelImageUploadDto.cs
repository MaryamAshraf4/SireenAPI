namespace Sireen.API.DTOs.HotelImages
{
    public class HotelImageUploadDto
    {
        public int HotelId { get; set; }
        public IFormFile Image { get; set; } = null!;
    }
}

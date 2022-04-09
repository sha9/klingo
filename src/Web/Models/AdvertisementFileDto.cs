using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class AdvertisementFileDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public string Data { get; set; } = string.Empty;
        [Required]
        public bool MarkForDelete { get; set; }
        public int AdvertismentId { get; set; }
    }
}
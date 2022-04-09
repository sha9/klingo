using System.ComponentModel.DataAnnotations;

namespace Web.Data
{
    public class AdvertisementFile
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public string Data { get; set; } = string.Empty;

        public int AdvertisementId { get; set; }
        public Advertisement Advertisement { get; set; }
    }
}

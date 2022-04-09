using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class AdvertisementDto
    {
        public int Id { get; set; }
        [Display(Name = "Navn")]
        public string ProductName { get; set; } = string.Empty;
        [Display(Name = "Beskrivelse")]
        public string Description { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        [Display(Name = "Årgang")]
        public int Year { get; set; }
        [Display(Name = "Pris")]
        public decimal? Price { get; set; }
        [Display(Name = "Byd på pris")]
        public bool IsOffer { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        [Display(Name = "Bilag")]
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
        public ICollection<AdvertisementFileDto> AdvertisementFileDtos { get; set; } = new List<AdvertisementFileDto>();
    }
}

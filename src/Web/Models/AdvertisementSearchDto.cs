using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class AdvertisementSearchDto
    {
        [Display(Name = "Navn")]
        public string ProductName { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        [Display(Name = "Årgang fra")]
        public int YearFrom { get; set; }
        [Display(Name = "Årgang til")]
        public int YearTo { get; set; }

        [Display(Name = "Pris fra")]
        public decimal? PriceFrom { get; set; }
        [Display(Name = "Pris to")]
        public decimal? PriceTo { get; set; }
        [Display(Name = "Byd på pris")]
        public bool IsOffer { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Data
{
    public class Advertisement
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(80)]
        [MinLength(1)]
        public string ProductName { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        [MinLength(1)]
        public string Description { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Year { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get;set; }
        public bool IsOffer { get; set; }
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        public List<AdvertisementFile> AdvertisementFiles { get; set; } = new List<AdvertisementFile>();
    }
}

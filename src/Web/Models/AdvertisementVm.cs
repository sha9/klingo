namespace Web.Models
{
    public class AdvertisementVm
    {
        public AdvertisementSearchDto AdvertisementSearchDto { get; set; } = new AdvertisementSearchDto();
        public List<AdvertisementDto> Advertisements { get; set; } = new List<AdvertisementDto>();
    }
}

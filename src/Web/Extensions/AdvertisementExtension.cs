using Web.Data;
using Web.Models;
using System.Linq;

namespace Web.Extensions
{
    public static class AdvertisementExtension
    {
        public static List<AdvertisementDto> ToAdvertisementDtoList(this List<Advertisement> advertisements)
        {
            return advertisements.Select(adv => adv.ToAdvertisementDto()).ToList();
        }
        public static AdvertisementDto ToAdvertisementDto(this Advertisement advertisement)
        {
            return new AdvertisementDto 
            { 
                ApplicationUserId = advertisement.ApplicationUserId,
                Description = advertisement.Description,
                Id = advertisement.Id,
               IsOffer = advertisement.IsOffer,
               Model = advertisement.Model,
               Price = advertisement.Price,
               ProductName = advertisement.ProductName,
               Year = advertisement.Year
            };
        }

        public static List<Advertisement> ToAdvertisementList(this List<AdvertisementDto> advertisements)
        {
            return advertisements.Select(adv => adv.ToAdvertisement()).ToList();
        }
        public static Advertisement ToAdvertisement(this AdvertisementDto advertisement)
        {
            return new Advertisement
            {
                ApplicationUserId = advertisement.ApplicationUserId,
                Description = advertisement.Description,
                Id = advertisement.Id,
                IsOffer = advertisement.IsOffer,
                Model = advertisement.Model,
                Price = advertisement.Price,
                ProductName = advertisement.ProductName,
                Year = advertisement.Year
            };
        }
    }
}

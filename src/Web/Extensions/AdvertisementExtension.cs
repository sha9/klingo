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
               Year = advertisement.Year,
               AdvertisementFileDtos = advertisement.AdvertisementFiles.ToAdvertisementFileDtoList(),
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
                Year = advertisement.Year,
            };
        }

        public static List<AdvertisementFileDto> ToAdvertisementFileDtoList(this ICollection<AdvertisementFile> advertisementFiles)
        {
            return advertisementFiles.Select(advFil => advFil.ToAdvertisementFileDto()).ToList();
        }

        public static AdvertisementFileDto ToAdvertisementFileDto(this AdvertisementFile advertisementFile)
        {
            return new AdvertisementFileDto
            {
                AdvertismentId = advertisementFile.AdvertisementId,
                Data = advertisementFile.Data,
                Id = advertisementFile.Id,
                Name = advertisementFile.Name,
                Type = advertisementFile.Type,
            };
        }

        public static List<AdvertisementFile> ToAdvertisementFileList(this List<AdvertisementFileDto> advertisementDtoFiles)
        {
            return advertisementDtoFiles.Select(advFil => advFil.ToAdvertisementFile()).ToList();
        }

        public static AdvertisementFile ToAdvertisementFile(this AdvertisementFileDto advertisementFileDto)
        {
            return new AdvertisementFile
            {
                AdvertisementId = advertisementFileDto.AdvertismentId,
                Data = advertisementFileDto.Data,
                Id = advertisementFileDto.Id,
                Name = advertisementFileDto.Name,
                Type = advertisementFileDto.Type,
            };
        }
    }
}

using Web.Data;
using Web.Models;
using System.Linq;

namespace Web.Extensions
{
    public static class AdvertisementExtension
    {
        public static List<AdvertisementDto> ToAdvertisementDtoList(this List<Advertisement> advertisements, bool generateImages = false)
        {
            return advertisements.Select(adv => adv.ToAdvertisementDto(generateImages)).ToList();
        }
        public static AdvertisementDto ToAdvertisementDto(this Advertisement advertisement, bool generateImages = false)
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
                AdvertisementFileDtos = advertisement.AdvertisementFiles.ToAdvertisementFileDtoList(generateImages),
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

        public static List<AdvertisementFileDto> ToAdvertisementFileDtoList(this ICollection<AdvertisementFile> advertisementFiles, bool generateImages = false)
        {
            return advertisementFiles.Select(advFil => advFil.ToAdvertisementFileDto(generateImages)).ToList();
        }

        public static AdvertisementFileDto ToAdvertisementFileDto(this AdvertisementFile advertisementFile, bool generateImages = false)
        {
            return new AdvertisementFileDto
            {
                AdvertismentId = advertisementFile.AdvertisementId,
                Data = advertisementFile.Data,
                Id = advertisementFile.Id,
                Name = advertisementFile.Name,
                Type = advertisementFile.Type,
                ImageUrl = generateImages ? $"data:{advertisementFile.Type};base64,{advertisementFile.Data}" : string.Empty,
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

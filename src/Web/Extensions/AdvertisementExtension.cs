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
            var dto = new AdvertisementDto
            {
                ApplicationUserId = advertisement.ApplicationUserId,
                Description = advertisement.Description,
                Id = advertisement.Id,
                IsOffer = advertisement.IsOffer,
                Model = advertisement.Model,
                Price = advertisement.Price,
                ProductName = advertisement.ProductName,
                Year = advertisement.Year,
                AdvertisementFileDtos = advertisement.AdvertisementFiles.ToAdvertisementFileDtoList(generateImages)
            };
            dto = MapCategoriesForDto(advertisement, dto);
            return dto;
        }

        private static AdvertisementDto MapCategoriesForDto(Advertisement advertisement, AdvertisementDto dto)
        {
            if (!string.IsNullOrEmpty(advertisement.Category))
            {
                if (advertisement.Category.Contains(','))
                {
                    dto.SelectedCategories = advertisement.Category.Split(',');
                    var categories = string.Empty;
                    foreach (var category in dto.SelectedCategories)
                    {
                        categories += GetPrettyCategory(category) + ", ";
                    }
                    dto.Category = categories.Remove(categories.Length - 2, 1);
                }
                else
                {
                    dto.SelectedCategories = new string[] { advertisement.Category };
                    dto.Category = GetPrettyCategory(advertisement.Category);
                }
            }
            else
            {
                dto.SelectedCategories = new string[] { "0" };
                dto.Category = "Intet";
            }
            return dto;
        }

        private static string GetPrettyCategory(string category)
        {
            return category switch
            {
                "0" => "Intet",
                "1" => "Tandlæge",
                "2" => "Læge",
                "3" => "Fysioterapi",
                "4" => "Tandpleje",
                _ => "Intet",
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
                Category = advertisement.SelectedCategories != null && advertisement.SelectedCategories.Any() ? string.Join(",", advertisement.SelectedCategories) : string.Empty
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

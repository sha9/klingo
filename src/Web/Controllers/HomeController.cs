using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web.Data;
using Web.Extensions;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index(AdvertisementSearchDto advertisementSearchDto = null)
        {
            return View(new AdvertisementVm() { Advertisements = GetAdvertisements(advertisementSearchDto).ToAdvertisementDtoList(true) });
        }
        private List<Advertisement> GetAdvertisements(AdvertisementSearchDto advertisementSearchDto)
        {
            var advertisements = _context.Advertisements.Include(x=>x.AdvertisementFiles).Take(10).AsEnumerable();
            //if (advertisementSearchDto != null)
            //{
            //    var tmpAdds = advertisementSearchDto.IsOffer ? _context.Advertisements.Where(x => x.IsOffer) : _context.Advertisements.AsEnumerable();

            //    if (!string.IsNullOrEmpty(advertisementSearchDto.ProductName))
            //        tmpAdds = tmpAdds.Where(x => x.ProductName.ToLower().Contains(advertisementSearchDto.ProductName.ToLower())).AsEnumerable();

            //    if (advertisementSearchDto.YearFrom != 0)
            //        tmpAdds = tmpAdds.Where(x => x.Year >= advertisementSearchDto.YearFrom).AsEnumerable();

            //    if (advertisementSearchDto.YearTo != 0)
            //        tmpAdds = tmpAdds.Where(x => x.Year <= advertisementSearchDto.YearTo).AsEnumerable();

            //    if (advertisementSearchDto.PriceFrom != null && advertisementSearchDto.PriceFrom != 0)
            //        tmpAdds = tmpAdds.Where(x => x.Price >= advertisementSearchDto.PriceFrom).AsEnumerable();

            //    if (advertisementSearchDto.PriceTo != null && advertisementSearchDto.PriceTo != 0)
            //        tmpAdds = tmpAdds.Where(x => x.Price <= advertisementSearchDto.PriceTo).AsEnumerable();

            //    advertisements = tmpAdds;
            //}
            return advertisements.ToList();
        }
        public IActionResult NavRedirectByCategory(string categoryId)
        {
            return RedirectToAction("Index", "Advertisement", new AdvertisementSearchDto() { Category = cate });
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
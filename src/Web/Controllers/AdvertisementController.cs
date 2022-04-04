using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web.Data;
using Web.Extensions;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class AdvertisementController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdvertisementController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        [AllowAnonymous]
        public IActionResult Index(AdvertisementSearchDto advertisementSearchDto = null)
        {
            return View(new AdvertisementVm() { Advertisements = GetAdvertisements(advertisementSearchDto).ToAdvertisementDtoList() });
        }
        private List<Advertisement> GetAdvertisements(AdvertisementSearchDto advertisementSearchDto)
        {
            var advertisements = _context.Advertisements.AsEnumerable();
            if (advertisementSearchDto != null)
            {
                var tmpAdds = advertisementSearchDto.IsOffer ? _context.Advertisements.Where(x=>x.IsOffer) : _context.Advertisements.AsEnumerable();

                if (!string.IsNullOrEmpty(advertisementSearchDto.ProductName))
                    tmpAdds = tmpAdds.Where(x => x.ProductName.ToLower().Contains(advertisementSearchDto.ProductName.ToLower())).AsEnumerable();

                if(advertisementSearchDto.YearFrom != 0)
                    tmpAdds = tmpAdds.Where(x => x.Year >= advertisementSearchDto.YearFrom).AsEnumerable();

                if (advertisementSearchDto.YearTo != 0)
                    tmpAdds = tmpAdds.Where(x => x.Year <= advertisementSearchDto.YearTo).AsEnumerable();

                if (advertisementSearchDto.PriceFrom != 0)
                    tmpAdds = tmpAdds.Where(x => x.Price >= advertisementSearchDto.PriceFrom).AsEnumerable();

                if (advertisementSearchDto.PriceTo != 0)
                    tmpAdds = tmpAdds.Where(x => x.Price <= advertisementSearchDto.PriceTo).AsEnumerable();

                advertisements = tmpAdds;
            }
            return advertisements.ToList();
        }

        [HttpGet]
        public async Task<IActionResult> MyAdds()
        {
            var currentUserId = GetCurrentUserId();
            var list = await _context.Advertisements.Where(x=>x.ApplicationUserId == currentUserId).ToListAsync();
            return View(list.ToAdvertisementDtoList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AdvertisementDto advertisement)
        {
            if (ModelState.IsValid)
            {
                var add = advertisement.ToAdvertisement();
                add.ApplicationUserId = GetCurrentUserId();
                _context.Advertisements.Add(add);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyAdds");
            }
            return View(advertisement);
        }

        private string GetCurrentUserId()
        {
            if(_httpContextAccessor.HttpContext == null)
                return string.Empty;
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}

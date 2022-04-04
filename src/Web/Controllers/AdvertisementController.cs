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
        public async Task<IActionResult> IndexAsync(AdvertisementSearchDto advertisementSearchDto = null)
        {
            var list = await GetAdvertisements(advertisementSearchDto);
            var vm = new AdvertisementVm() { Advertisements = list.ToAdvertisementDtoList() };
            return View(vm);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Search(AdvertisementSearchDto advertisementSearchDto)
        {
            return await IndexAsync(advertisementSearchDto);
        }
        private async Task<List<Advertisement>> GetAdvertisements(AdvertisementSearchDto advertisementSearchDto)
        {
            if (advertisementSearchDto != null)
            {
                IQueryable<Advertisement> advertisements = null;
                if(!string.IsNullOrEmpty(advertisementSearchDto.ProductName))
                    advertisements = _context.Advertisements.Where(x=>x.ProductName.Contains(advertisementSearchDto.ProductName));

                if(advertisementSearchDto.PriceTo > 0)
                    advertisements = _context.Advertisements.Where(x => x.Price >= advertisementSearchDto.PriceFrom && x.Price <= advertisementSearchDto.PriceTo);
                else
                    advertisements = _context.Advertisements.Where(x=>x.Price >= advertisementSearchDto.PriceFrom);

                if (advertisements != null)
                    return await advertisements.ToListAsync();
            }
            return await _context.Advertisements.ToListAsync();
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

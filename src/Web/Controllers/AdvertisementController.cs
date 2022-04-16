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
        private List<string> AllowedFileTypes { get; set; } = new List<string>() {"png", "jpg", "jpeg" };
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

                if (advertisementSearchDto.PriceFrom != null && advertisementSearchDto.PriceFrom != 0)
                    tmpAdds = tmpAdds.Where(x => x.Price >= advertisementSearchDto.PriceFrom).AsEnumerable();

                if (advertisementSearchDto.PriceTo != null && advertisementSearchDto.PriceTo != 0)
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
                foreach(var file in advertisement.Files)
                {
                    if (!AllowedFileTypes.Contains(file.FileName.Split(".")[1].ToLower()))
                    {
                        ModelState.AddModelError("Files", "Only png, jpg and jpeg are allowed");
                        return View(advertisement);
                    }
                }
                var add = advertisement.ToAdvertisement();
                add.ApplicationUserId = GetCurrentUserId();
                _context.Advertisements.Add(add);
                await _context.SaveChangesAsync();
                FormFilesToAdvertisementFilesForCreate(add.Id, advertisement.Files);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyAdds");
            }
            return View(advertisement);
        }

        private void FormFilesToAdvertisementFilesForCreate(int advId, List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var data = GetFileData(file);

                if (string.IsNullOrEmpty(data))
                    continue;

                var fileForSave = new AdvertisementFile
                {
                    AdvertisementId = advId,
                    Name = file.FileName,
                    Type = file.ContentType,
                    Data = data
                };

                _context.AdvertisementFiles.Add(fileForSave);
            }
        }

        private string GetFileData(IFormFile file)
        {
            var data = string.Empty;

            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    data = Convert.ToBase64String(fileBytes);
                }
            }
            return data;
        }
        [AllowAnonymous]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisements
                .Include(x=>x.AdvertisementFiles)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (advertisement == null)
            {
                return NotFound();
            }

            return View(advertisement.ToAdvertisementDto(true));
        }

        public async Task<IActionResult> ShowFile(int? id)
        {
            if(id == null || id <= 0)
                return BadRequest();

            var advertisement = await _context.AdvertisementFiles.SingleOrDefaultAsync(x => x.Id == id);
            if (advertisement == null)
            {
                return NotFound();
            }
            var data = Convert.FromBase64String(advertisement.Data);
            var type = advertisement.Type;
            return File(data, type);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisements.FindAsync(id);
            if (advertisement == null)
            {
                return NotFound();
            }
            return View(advertisement.ToAdvertisementDto());
        }

        // POST: Advertisements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,Description,Model,Category,Year,Price,IsOffer,Files,AdvertisementFileDtos,ApplicationUserId")] AdvertisementDto advertisement)
        {
            if (id != advertisement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advertisement.ToAdvertisement());
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertisementExists(advertisement.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(advertisement);
        }

        // GET: Advertisements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advertisement == null)
            {
                return NotFound();
            }

            return View(advertisement.ToAdvertisementDto());
        }

        // POST: Advertisements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advertisement = await _context.Advertisements.FindAsync(id);
            _context.Advertisements.Remove(advertisement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvertisementExists(int id)
        {
            return _context.Advertisements.Any(e => e.Id == id);
        }

        private string GetCurrentUserId()
        {
            if(_httpContextAccessor.HttpContext == null)
                return string.Empty;
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}

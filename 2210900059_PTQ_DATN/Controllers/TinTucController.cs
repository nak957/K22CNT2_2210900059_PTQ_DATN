using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2210900059_PTQ_DATN.Models;

namespace _2210900059_PTQ_DATN.Controllers
{
    public class TinTucController : Controller
    {
        private readonly LeSkinDbContext _context;

        public TinTucController(LeSkinDbContext context)
        {
            _context = context;
        }

        // ==============================
        // DANH SÁCH TIN TỨC
        // ==============================
        public async Task<IActionResult> Index()
        {
            var danhSachTinTuc = await _context.BaiViets
                .Where(x => x.Loai == "Tin tức")
                .OrderByDescending(x => x.NoiBat)
                .ThenByDescending(x => x.NgayDang)
                .ToListAsync();

            return View(danhSachTinTuc);
        }

        // ==============================
        // CHI TIẾT TIN TỨC
        // ==============================
        public async Task<IActionResult> ChiTiet(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return NotFound();

            var baiViet = await _context.BaiViets
                .FirstOrDefaultAsync(x => x.Slug == slug && x.Loai == "Tin tức");

            if (baiViet == null)
                return NotFound();

            return View(baiViet);
        }
    }
}

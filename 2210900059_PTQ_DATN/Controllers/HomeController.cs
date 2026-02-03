using _2210900059_PTQ_DATN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace _2210900059_PTQ_DATN.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LeSkinDbContext _context;

        public HomeController(
            LeSkinDbContext context,
            ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // ================== SẢN PHẨM NỔI BẬT ==================
            ViewBag.SanPhams = await _context.SanPhams
                .Where(x => x.NoiBat == true && x.TrangThai == true)
                .ToListAsync();

            // ================== DỊCH VỤ NỔI BẬT ==================
            ViewBag.DichVus = await _context.DichVus
                .Where(x => x.NoiBat == true && x.TrangThai == true)
                .ToListAsync();

            // ================== BÀI VIẾT NỔI BẬT ==================
            ViewBag.BaiViets = await _context.BaiViets
                .Where(x => x.NoiBat == true)
                .OrderByDescending(x => x.NgayDang)
                .Take(3) 
                .ToListAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using _2210900059_PTQ_DATN.Models;
using Microsoft.EntityFrameworkCore;

namespace _2210900059_PTQ_DATN.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class HomeController : Controller
    {
        private readonly LeSkinDbContext _context;

        public HomeController(LeSkinDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TongNguoiDung = await _context.NguoiDungs.CountAsync();
            ViewBag.TongSanPham = await _context.SanPhams.CountAsync();
            ViewBag.TongDonHang = await _context.DonHangs.CountAsync();
            ViewBag.TongLienHe = await _context.LienHes.CountAsync();

            return View();
        }
    }
}

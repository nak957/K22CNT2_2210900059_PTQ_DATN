using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2210900059_PTQ_DATN.Models;

namespace _2210900059_PTQ_DATN.Controllers
{
    public class SanPhamController : Controller
    {
        
        private readonly LeSkinDbContext _context;

        public SanPhamController(LeSkinDbContext context)
        {
            _context = context;
        }

        /* =========================
         * TRANG DANH SÁCH SẢN PHẨM
         * ========================= */
        public IActionResult Index()
        {
            var sanPhams = _context.SanPhams
                .Where(sp => sp.TrangThai == true)
                .OrderByDescending(sp => sp.NgayTao)
                .ToList();

            return View(sanPhams);
        }

        /* =========================
         * TRANG CHI TIẾT SẢN PHẨM
         * ========================= */
        public IActionResult ChiTiet(int id)
        {
            var sanPham = _context.SanPhams
                .Include(sp => sp.MaDanhMucNavigation)
                .FirstOrDefault(sp => sp.MaSanPham == id && sp.TrangThai == true);

            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
    }
}

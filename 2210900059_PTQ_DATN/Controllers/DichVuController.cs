using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2210900059_PTQ_DATN.Models;

namespace _2210900059_PTQ_DATN.Controllers
{
    public class DichVuController : Controller
    {
        private readonly LeSkinDbContext _context;

        public DichVuController(LeSkinDbContext context)
        {
            _context = context;
        }

        // Trang danh sách dịch vụ
        public IActionResult Index()
        {
            var dichVus = _context.DichVus
                .Where(dv => dv.TrangThai == true)
                .ToList();

            return View(dichVus);
        }

        // ===== CHI TIẾT DỊCH VỤ =====
        public IActionResult ChiTiet(int id)
        {
            var dichVu = _context.DichVus
                .FirstOrDefault(dv => dv.MaDichVu == id && dv.TrangThai == true);

            if (dichVu == null)
            {
                return NotFound();
            }

            return View(dichVu);
        }
    }
}

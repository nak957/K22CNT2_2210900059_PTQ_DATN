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

        // ==========================
        // DANH SÁCH + LỌC DỊCH VỤ
        // ==========================
        public IActionResult Index(string? danhMuc)
        {
            // Query gốc
            var query = _context.DichVus
                .Include(dv => dv.MaDanhMucDvNavigation)
                .Where(dv => dv.TrangThai == true);

            // Nếu có lọc theo tên danh mục
            if (!string.IsNullOrEmpty(danhMuc))
            {
                query = query.Where(dv =>
                    dv.MaDanhMucDvNavigation != null &&
                    dv.MaDanhMucDvNavigation.TenDanhMuc == danhMuc
                );
            }

            // Danh sách dịch vụ
            var dichVus = query.ToList();

            // Danh sách danh mục để render dropdown
            ViewBag.DanhMucs = _context.DanhMucDichVus.ToList();
            ViewBag.DanhMucDangChon = danhMuc;

            return View(dichVus);
        }

        // ==========================
        // CHI TIẾT DỊCH VỤ
        // ==========================
        public IActionResult ChiTiet(int id)
        {
            var dichVu = _context.DichVus
                .Include(dv => dv.MaDanhMucDvNavigation)
                .FirstOrDefault(dv => dv.MaDichVu == id && dv.TrangThai == true);

            if (dichVu == null)
            {
                return NotFound();
            }

            return View(dichVu);
        }
    }
}

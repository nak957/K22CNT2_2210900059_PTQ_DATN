using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2210900059_PTQ_DATN.Models;
using System.Linq;

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
         * TRANG DANH SÁCH SẢN PHẨM + LỌC DANH MỤC
         * ========================= */
        public IActionResult Index(int? danhMucId)
        {
            // Danh sách danh mục để lọc
            ViewBag.DanhMucSanPham = _context.DanhMucSanPhams.ToList();
            ViewBag.DanhMucDangChon = danhMucId;

            var sanPhams = _context.SanPhams
                .Include(sp => sp.MaDanhMucNavigation)
                .Where(sp => sp.TrangThai == true);

            // Nếu có chọn danh mục → lọc
            if (danhMucId.HasValue)
            {
                sanPhams = sanPhams.Where(sp => sp.MaDanhMuc == danhMucId);
            }

            return View(
                sanPhams
                    .OrderByDescending(sp => sp.NgayTao)
                    .ToList()
            );
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

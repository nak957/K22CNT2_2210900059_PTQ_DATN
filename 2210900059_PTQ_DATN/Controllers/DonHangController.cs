using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2210900059_PTQ_DATN.Models;
using _2210900059_PTQ_DATN.ViewModels;

namespace _2210900059_PTQ_DATN.Controllers
{
    public class DonHangController : Controller
    {
        private readonly LeSkinDbContext _context;

        public DonHangController(LeSkinDbContext context)
        {
            _context = context;
        }

        // ================================
        // TRANG QUẢN LÝ ĐƠN HÀNG
        // ================================
        public IActionResult Index()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");

            if (maNguoiDung != null)
            {
                var donHangs = _context.DonHangs
                    .Where(x => x.MaNguoiDung == maNguoiDung)
                    .OrderByDescending(x => x.NgayDat)
                    .ToList();

                return View(donHangs);
            }

            return View(new List<DonHang>());
        }

        // ================================
        // TRA CỨU ĐƠN HÀNG (KHÁCH)
        // ================================
        [HttpPost]
        public IActionResult TraCuu(string maDonHangCode, string soDienThoai)
        {
            if (string.IsNullOrEmpty(maDonHangCode) || string.IsNullOrEmpty(soDienThoai))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ mã đơn hàng và số điện thoại.";
                return RedirectToAction("Index");
            }

            var donHangs = _context.DonHangs
                .Where(x => x.MaDonHangCode == maDonHangCode && x.SoDienThoai == soDienThoai)
                .ToList();

            if (!donHangs.Any())
            {
                TempData["Error"] = "Không tìm thấy đơn hàng phù hợp.";
            }

            return View("Index", donHangs);
        }

        // ================================
        // CHI TIẾT ĐƠN HÀNG
        // ================================
        public IActionResult ChiTiet(int id)
        {
            var donHang = _context.DonHangs
                .FirstOrDefault(x => x.MaDonHang == id);

            if (donHang == null)
                return NotFound();

            var chiTietVM = _context.ChiTietDonHangs
                .Where(ct => ct.MaDonHang == id)
                .Select(ct => new ChiTietDonHangVM
                {
                    TenItem = ct.ItemType == "SanPham"
                        ? _context.SanPhams
                            .Where(sp => sp.MaSanPham == ct.ItemId)
                            .Select(sp => sp.TenSanPham)
                            .FirstOrDefault()
                        : _context.DichVus
                            .Where(dv => dv.MaDichVu == ct.ItemId)
                            .Select(dv => dv.TenDichVu)
                            .FirstOrDefault(),

                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    ThanhTien = ct.ThanhTien
                })
                .ToList();

            ViewBag.ChiTiet = chiTietVM;

            return View(donHang);
        }
        // ================================
        // HỦY ĐƠN HÀNG
        // ================================
        [HttpPost]
        public IActionResult HuyDon(int id)
        {
            var donHang = _context.DonHangs.FirstOrDefault(x => x.MaDonHang == id);

            if (donHang == null)
                return NotFound();

            // Chỉ cho hủy khi đang chờ xử lý
            if (donHang.TrangThai != "Chờ xử lý")
            {
                TempData["Error"] = "Đơn hàng này không thể hủy.";
                return RedirectToAction("ChiTiet", new { id });
            }

            donHang.TrangThai = "Đã hủy";
            donHang.NgayCapNhat = DateTime.Now;

            _context.SaveChanges();

            TempData["Success"] = "Hủy đơn hàng thành công.";

            return RedirectToAction("Index");
        }

    }
}

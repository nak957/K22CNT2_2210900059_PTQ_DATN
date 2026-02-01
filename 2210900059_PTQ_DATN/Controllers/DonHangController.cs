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

        // =====================================================
        // ĐƠN SẢN PHẨM – DANH SÁCH
        // =====================================================
        public IActionResult Index()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");

            if (maNguoiDung != null)
            {
                var donHangs = _context.DonHangs
                    .Where(dh =>
                        dh.MaNguoiDung == maNguoiDung &&
                        _context.ChiTietDonHangs.Any(ct =>
                            ct.MaDonHang == dh.MaDonHang &&
                            ct.ItemType == "SanPham"))
                    .OrderByDescending(dh => dh.NgayDat)
                    .ToList();

                return View(donHangs);
            }

            // khách chưa đăng nhập → view rỗng
            return View(new List<DonHang>());
        }


        // =====================================================
        // TRA CỨU ĐƠN SẢN PHẨM – KHÁCH
        // =====================================================
        [HttpPost]
        public IActionResult TraCuu(string maDonHangCode, string soDienThoai)
        {
            if (string.IsNullOrEmpty(maDonHangCode) || string.IsNullOrEmpty(soDienThoai))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ mã đơn hàng và số điện thoại.";
                return RedirectToAction("Index");
            }

            var donHangs = _context.DonHangs
                .Where(dh =>
                    dh.MaDonHangCode == maDonHangCode &&
                    dh.SoDienThoai == soDienThoai &&
                    _context.ChiTietDonHangs.Any(ct =>
                        ct.MaDonHang == dh.MaDonHang &&
                        ct.ItemType == "SanPham"))
                .OrderByDescending(dh => dh.NgayDat)
                .ToList();

            if (!donHangs.Any())
                TempData["Error"] = "Không tìm thấy đơn hàng phù hợp.";

            return View("Index", donHangs);
        }
        // =====================================================
        // CHI TIẾT ĐƠN SẢN PHẨM
        // =====================================================
        public IActionResult ChiTiet(int id)
        {
            var donHang = _context.DonHangs.FirstOrDefault(x => x.MaDonHang == id);
            if (donHang == null) return NotFound();

            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung != null && donHang.MaNguoiDung != maNguoiDung)
                return Forbid();

            // chỉ lấy chi tiết sản phẩm
            var chiTietVM = _context.ChiTietDonHangs
                .Where(ct => ct.MaDonHang == id && ct.ItemType == "SanPham")
                .Select(ct => new ChiTietDonHangVM
                {
                    TenItem = _context.SanPhams
                        .Where(sp => sp.MaSanPham == ct.ItemId)
                        .Select(sp => sp.TenSanPham)
                        .FirstOrDefault(),

                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    ThanhTien = ct.ThanhTien
                })
                .ToList();

            ViewBag.ChiTiet = chiTietVM;
            return View(donHang);
        }
        // =====================================================
        // HỦY ĐƠN SẢN PHẨM
        // =====================================================
        [HttpPost]
        public IActionResult HuyDon(int id)
        {
            var donHang = _context.DonHangs.FirstOrDefault(x => x.MaDonHang == id);
            if (donHang == null) return NotFound();

            // đảm bảo là đơn sản phẩm
            bool isSanPham = _context.ChiTietDonHangs.Any(ct =>
                ct.MaDonHang == id && ct.ItemType == "SanPham");

            if (!isSanPham)
                return BadRequest();

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
        // =====================================================
        // ĐƠN DỊCH VỤ – LỊCH HẸN
        // =====================================================
        public IActionResult DonHangDichVu()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");

            if (maNguoiDung != null)
            {
                var lichHen = _context.ChiTietDonHangs
                    .Where(ct =>
                        ct.ItemType == "DichVu" &&
                        ct.MaDonHangNavigation.MaNguoiDung == maNguoiDung)
                    .Select(ct => new DonHangDichVuVM
                    {
                        MaDonHang = ct.MaDonHang,
                        MaDonHangCode = ct.MaDonHangNavigation.MaDonHangCode,
                        TenDichVu = _context.DichVus
                            .Where(dv => dv.MaDichVu == ct.ItemId)
                            .Select(dv => dv.TenDichVu)
                            .FirstOrDefault() ?? "",
                        SoLuong = ct.SoLuong,
                        NgayHen = ct.MaDonHangNavigation.NgayDat!.Value.AddDays(1),
                        GioHen = "09:00 - 11:00",
                        TrangThai = ct.MaDonHangNavigation.TrangThai
                    })
                    .OrderByDescending(x => x.NgayHen)
                    .ToList();

                return View("DonHangDichVu", lichHen);
            }

            // khách vãng lai → view rỗng + form tra cứu
            return View("DonHangDichVu", new List<DonHangDichVuVM>());
        }

        // TRA CỨU LỊCH HẸN – KHÁCH VÃNG LAI
        [HttpPost]
        public IActionResult TraCuuDichVu(string maDonHangCode, string soDienThoai)
        {
            if (string.IsNullOrEmpty(maDonHangCode) || string.IsNullOrEmpty(soDienThoai))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ mã đơn hàng và số điện thoại.";
                return RedirectToAction("DichVu");
            }

            var lichHen = _context.ChiTietDonHangs
                .Where(ct =>
                    ct.ItemType == "DichVu" &&
                    ct.MaDonHangNavigation.MaDonHangCode == maDonHangCode &&
                    ct.MaDonHangNavigation.SoDienThoai == soDienThoai)
                .Select(ct => new DonHangDichVuVM
                {
                    MaDonHang = ct.MaDonHang,
                    MaDonHangCode = ct.MaDonHangNavigation.MaDonHangCode,
                    TenDichVu = _context.DichVus
                        .Where(dv => dv.MaDichVu == ct.ItemId)
                        .Select(dv => dv.TenDichVu)
                        .FirstOrDefault() ?? "",
                    SoLuong = ct.SoLuong,
                    NgayHen = ct.MaDonHangNavigation.NgayDat!.Value.AddDays(1),
                    GioHen = "09:00 - 11:00",
                    TrangThai = ct.MaDonHangNavigation.TrangThai
                })
                .ToList();

            if (!lichHen.Any())
                TempData["Error"] = "Không tìm thấy lịch hẹn phù hợp.";

            return View("DonHangDichVu", lichHen);
        }

        public IActionResult ChiTietDichVu(int id)
        {
            var donHang = _context.DonHangs.FirstOrDefault(x => x.MaDonHang == id);
            if (donHang == null) return NotFound();

            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung != null && donHang.MaNguoiDung != maNguoiDung)
                return Forbid();

            var dichVu = _context.ChiTietDonHangs
                .Where(x => x.MaDonHang == id && x.ItemType == "DichVu")
                .Select(x => new DonHangDichVuVM
                {
                    MaDonHang = id,
                    MaDonHangCode = donHang.MaDonHangCode,
                    TenDichVu = _context.DichVus
                        .Where(dv => dv.MaDichVu == x.ItemId)
                        .Select(dv => dv.TenDichVu)
                        .FirstOrDefault() ?? "",
                    SoLuong = x.SoLuong,
                    NgayHen = donHang.NgayDat!.Value.AddDays(1),
                    GioHen = "09:00 - 11:00",
                    TrangThai = donHang.TrangThai
                })
                .FirstOrDefault();

            return View(dichVu);
        }

        [HttpPost]
        public IActionResult HuyLichHen(int maDonHang)
        {
            var donHang = _context.DonHangs.FirstOrDefault(x => x.MaDonHang == maDonHang);
            if (donHang == null) return NotFound();

            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung != null && donHang.MaNguoiDung != maNguoiDung)
                return Forbid();

            if (donHang.TrangThai != "Chờ xử lý")
            {
                TempData["Error"] = "Không thể hủy lịch hẹn ở trạng thái hiện tại.";
                return RedirectToAction("ChiTietDichVu", new { id = maDonHang });
            }

            donHang.TrangThai = "Đã hủy";
            donHang.NgayCapNhat = DateTime.Now;
            _context.SaveChanges();

            TempData["Success"] = "Đã hủy lịch hẹn thành công.";
            return RedirectToAction("DichVu");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2210900059_PTQ_DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace _2210900059_PTQ_DATN.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly LeSkinDbContext _context;
        private readonly ILogger<ThanhToanController> _logger;

        public ThanhToanController(LeSkinDbContext context, ILogger<ThanhToanController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // =====================================================
        // NHẬN DANH SÁCH ITEM ĐƯỢC CHỌN + CHECK TỒN KHO
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(List<int> selectedItemIds)
        {
            if (selectedItemIds == null || !selectedItemIds.Any())
            {
                TempData["CheckoutError"] = "Vui lòng chọn ít nhất một sản phẩm để thanh toán.";
                return RedirectToAction("Index", "GioHang");
            }

            var sessionId = HttpContext.Session.GetString("CART_SESSION");
            if (string.IsNullOrEmpty(sessionId))
            {
                TempData["CheckoutError"] = "Giỏ hàng đã hết hạn.";
                return RedirectToAction("Index", "GioHang");
            }

            var cart = _context.GioHangs
                .Include(g => g.GioHangChiTiets)
                .FirstOrDefault(g => g.SessionId == sessionId);

            if (cart == null)
            {
                TempData["CheckoutError"] = "Không tìm thấy giỏ hàng.";
                return RedirectToAction("Index", "GioHang");
            }

            var selectedItems = cart.GioHangChiTiets
                .Where(x => selectedItemIds.Contains(x.MaCt))
                .ToList();

            // ================= CHECK TỒN KHO =================
            foreach (var item in selectedItems)
            {
                if (item.LoaiItem == "SP")
                {
                    var sp = _context.SanPhams.FirstOrDefault(x => x.MaSanPham == item.MaItem);
                    if (sp == null || item.SoLuong > sp.SoLuong)
                    {
                        TempData["CheckoutError"] =
                            $"Sản phẩm \"{sp?.TenSanPham}\" chỉ còn {sp?.SoLuong ?? 0} sản phẩm.";
                        return RedirectToAction("Index", "GioHang");
                    }
                }
            }

            HttpContext.Session.SetString(
                "CHECKOUT_ITEMS",
                JsonSerializer.Serialize(selectedItemIds)
            );

            return RedirectToAction("ThongTinLienHe");
        }

        // =====================================================
        // FORM THÔNG TIN LIÊN HỆ (GET)
        // =====================================================
        [HttpGet]
        public IActionResult ThongTinLienHe()
        {
            return View(new LienHe());
        }

        // =====================================================
        // FORM THÔNG TIN LIÊN HỆ (POST)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThongTinLienHe(LienHe model)
        {
            if (!ModelState.IsValid) return View(model);

            model.NgayGui = DateTime.Now;
            model.DaDoc = false;
            model.TrangThai = "Chưa xử lý";

            _context.LienHes.Add(model);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("MA_LIEN_HE", model.MaLienHe);
            return RedirectToAction("XacNhan");
        }

        // =====================================================
        // TRANG XÁC NHẬN
        // =====================================================
        public IActionResult XacNhan()
        {
            var maLienHe = HttpContext.Session.GetInt32("MA_LIEN_HE");
            if (maLienHe == null) return RedirectToAction("ThongTinLienHe");

            var lienHe = _context.LienHes.FirstOrDefault(x => x.MaLienHe == maLienHe);
            if (lienHe == null) return RedirectToAction("ThongTinLienHe");

            return View(lienHe);
        }

        // =====================================================
        // TẠO ĐƠN HÀNG (CHECK TỒN KHO LẦN CUỐI)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TaoDonHang()
        {
            var json = HttpContext.Session.GetString("CHECKOUT_ITEMS");
            if (string.IsNullOrEmpty(json))
            {
                TempData["CheckoutError"] = "Không có sản phẩm để thanh toán.";
                return RedirectToAction("Index", "GioHang");
            }

            var selectedItemIds = JsonSerializer.Deserialize<List<int>>(json) ?? new();

            var sessionId = HttpContext.Session.GetString("CART_SESSION");
            var cart = _context.GioHangs
                .Include(g => g.GioHangChiTiets)
                .FirstOrDefault(g => g.SessionId == sessionId);

            var selectedItems = cart.GioHangChiTiets
                .Where(x => selectedItemIds.Contains(x.MaCt))
                .ToList();

            // ===== CHECK TỒN KHO LẦN CUỐI (CHỐNG RACE CONDITION) =====
            foreach (var item in selectedItems)
            {
                if (item.LoaiItem == "SP")
                {
                    var sp = _context.SanPhams.FirstOrDefault(x => x.MaSanPham == item.MaItem);
                    if (sp == null || item.SoLuong > sp.SoLuong)
                    {
                        TempData["CheckoutError"] =
                            $"Sản phẩm \"{sp?.TenSanPham}\" không đủ số lượng để thanh toán.";
                        return RedirectToAction("Index", "GioHang");
                    }
                }
            }

            var lienHe = _context.LienHes
                .FirstOrDefault(x => x.MaLienHe == HttpContext.Session.GetInt32("MA_LIEN_HE"));

            decimal tongTien = selectedItems.Sum(x => x.SoLuong * x.DonGia);

            var donHang = new DonHang
            {
                MaDonHangCode = GenerateOrderCode(),
                HoTen = lienHe.HoTen,
                SoDienThoai = lienHe.SoDienThoai,
                Email = lienHe.Email,
                DiaChi = lienHe.DiaChi,
                TongTien = tongTien,
                TrangThai = "Chờ xử lý",
                TrangThaiThanhToan = "Chưa thanh toán",
                NgayDat = DateTime.Now
            };

            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            foreach (var item in selectedItems)
            {
                _context.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    ItemId = item.MaItem,
                    ItemType = item.LoaiItem == "SP" ? "SanPham" : "DichVu",
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia,
                    ThanhTien = item.SoLuong * item.DonGia
                });

                // TRỪ TỒN KHO
                if (item.LoaiItem == "SP")
                {
                    var sp = _context.SanPhams.First(x => x.MaSanPham == item.MaItem);
                    sp.SoLuong -= item.SoLuong;
                }
            }

            _context.GioHangChiTiets.RemoveRange(selectedItems);
            _context.SaveChanges();

            HttpContext.Session.Remove("CHECKOUT_ITEMS");

            return RedirectToAction("HoanTat", new { id = donHang.MaDonHang });
        }

        public IActionResult HoanTat(int id)
        {
            var donHang = _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .FirstOrDefault(d => d.MaDonHang == id);

            if (donHang == null) return NotFound();
            return View(donHang);
        }

        private static string GenerateOrderCode()
        {
            return $"DH{DateTime.Now:yyyyMMddHHmmssfff}";
        }
    }
}

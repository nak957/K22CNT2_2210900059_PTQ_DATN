using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2210900059_PTQ_DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Security.Claims;

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

        // Helper: try get current user id from Session first, then from Claims
        private int? GetCurrentUserId()
        {
            // 1) ưu tiên Session
            var sid = HttpContext.Session.GetInt32("USER_ID");
            if (sid.HasValue) return sid.Value;

            // 2) thử từ Claims
            if (User?.Identity?.IsAuthenticated == true)
            {
                var claimKeys = new[]
                {
                    System.Security.Claims.ClaimTypes.NameIdentifier,
                    "sub",
                    "id",
                    "userid",
                    "user_id",
                    "UserId",
                    "nameidentifier"
                };

                foreach (var key in claimKeys)
                {
                    var c = User.FindFirst(key);
                    if (c != null && int.TryParse(c.Value, out var id))
                        return id;
                }

                // 3) fallback: nếu User.Identity.Name có giá trị, thử khớp TenDangNhap hoặc Email trong DB
                var identityName = User.Identity?.Name;
                if (!string.IsNullOrEmpty(identityName))
                {
                    var user = _context.NguoiDungs
                        .AsNoTracking()
                        .FirstOrDefault(u => u.TenDangNhap == identityName || u.Email == identityName);
                    if (user != null) return user.MaNguoiDung;
                }
            }

            return null;
        }

        // Helper: load cart either by session id or by user id (when user logged in)
        private GioHang? LoadCart(string? sessionId, int? userId)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                var cart = _context.GioHangs
                    .Include(g => g.GioHangChiTiets)
                    .FirstOrDefault(g => g.SessionId == sessionId);
                if (cart != null) return cart;
            }

            if (userId != null)
            {
                return _context.GioHangs
                    .Include(g => g.GioHangChiTiets)
                    .FirstOrDefault(g => g.MaNguoiDung == userId);
            }

            return null;
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
            var userId = GetCurrentUserId();

            var cart = LoadCart(sessionId, userId);

            if (cart == null)
            {
                TempData["CheckoutError"] = "Giỏ hàng đã hết hạn hoặc không tồn tại.";
                return RedirectToAction("Index", "GioHang");
            }

            var selectedItems = cart.GioHangChiTiets
                .Where(x => selectedItemIds.Contains(x.MaCt))
                .ToList();

            if (!selectedItems.Any())
            {
                TempData["CheckoutError"] = "Không tìm thấy sản phẩm được chọn trong giỏ hàng.";
                return RedirectToAction("Index", "GioHang");
            }

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
        // - Nếu đã đăng nhập + đã có LiênHe → bỏ qua form
        // - Nếu chưa có → hiển thị form
        // =====================================================
        [HttpGet]
        public IActionResult ThongTinLienHe()
        {
            var userId = GetCurrentUserId();

            // ===============================
            // ĐÃ ĐĂNG NHẬP
            // ===============================
            if (userId != null)
            {
                var lienHe = _context.LienHes
                    .Where(x => x.MaNguoiDung == userId)
                    .OrderByDescending(x => x.NgayGui)
                    .FirstOrDefault();

                if (lienHe != null)
                {
                    HttpContext.Session.SetInt32("MA_LIEN_HE", lienHe.MaLienHe);
                    return RedirectToAction("XacNhan");
                }
            }

            // ===============================
            // CHƯA ĐĂNG NHẬP
            // HOẶC CHƯA CÓ LIÊN HỆ
            // ===============================
            return View(new LienHe());
        }

        // =====================================================
        // FORM THÔNG TIN LIÊN HỆ (POST)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThongTinLienHe(LienHe model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = GetCurrentUserId();

            // Nếu chưa có userId từ session/claim nhưng user đang auth, cố lookup 1 lần nữa bằng User.Identity.Name
            if (userId == null && User?.Identity?.IsAuthenticated == true)
            {
                var identityName = User.Identity?.Name;
                if (!string.IsNullOrEmpty(identityName))
                {
                    var user = _context.NguoiDungs
                        .AsNoTracking()
                        .FirstOrDefault(u => u.TenDangNhap == identityName || u.Email == identityName);
                    if (user != null) userId = user.MaNguoiDung;
                }
            }

            model.NgayGui = DateTime.Now;
            model.DaDoc = false;
            model.TrangThai = "Chưa xử lý";

            if (userId != null)
            {
                model.MaNguoiDung = userId;
            }

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
            var userId = GetCurrentUserId();

            var cart = LoadCart(sessionId, userId);
            if (cart == null)
            {
                TempData["CheckoutError"] = "Giỏ hàng không tồn tại.";
                return RedirectToAction("Index", "GioHang");
            }

            var selectedItems = cart.GioHangChiTiets
                .Where(x => selectedItemIds.Contains(x.MaCt))
                .ToList();

            if (!selectedItems.Any())
            {
                TempData["CheckoutError"] = "Không tìm thấy sản phẩm để thanh toán.";
                return RedirectToAction("Index", "GioHang");
            }

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

            if (lienHe == null)
            {
                TempData["CheckoutError"] = "Vui lòng cung cấp thông tin liên hệ trước khi thanh toán.";
                return RedirectToAction("ThongTinLienHe");
            }

            decimal tongTien = selectedItems.Sum(x => x.SoLuong * x.DonGia);

            // Use transaction to ensure atomicity
            using var tx = _context.Database.BeginTransaction();
            try
            {
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
                    NgayDat = DateTime.Now,
                    MaNguoiDung = userId
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
                        sp.SoLuong = sp.SoLuong - item.SoLuong;
                        _context.SanPhams.Update(sp);
                    }
                }

                _context.GioHangChiTiets.RemoveRange(selectedItems);
                _context.SaveChanges();

                tx.Commit();

                HttpContext.Session.Remove("CHECKOUT_ITEMS");

                return RedirectToAction("HoanTat", new { id = donHang.MaDonHang });
            }
            catch (Exception ex)
            {
                tx.Rollback();
                _logger.LogError(ex, "Tạo đơn hàng thất bại.");
                TempData["CheckoutError"] = "Có lỗi khi tạo đơn hàng. Vui lòng thử lại.";
                return RedirectToAction("Index", "GioHang");
            }
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

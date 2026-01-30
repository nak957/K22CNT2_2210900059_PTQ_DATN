using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2210900059_PTQ_DATN.Models;
using _2210900059_PTQ_DATN.Models.ViewModels;

namespace _2210900059_PTQ_DATN.Controllers
{
    public class GioHangController : Controller
    {
        private readonly LeSkinDbContext _context;

        public GioHangController(LeSkinDbContext context)
        {
            _context = context;
        }

        // ===============================
        // LẤY HOẶC TẠO GIỎ HÀNG
        // ===============================
        private GioHang GetOrCreateCart()
        {
            var sessionId = HttpContext.Session.GetString("CART_SESSION");

            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("CART_SESSION", sessionId);
            }

            var cart = _context.GioHangs
                .Include(g => g.GioHangChiTiets)
                .FirstOrDefault(g => g.SessionId == sessionId);

            if (cart == null)
            {
                cart = new GioHang
                {
                    SessionId = sessionId,
                    NgayTao = DateTime.Now
                };
                _context.GioHangs.Add(cart);
                _context.SaveChanges();
            }

            return cart;
        }

        // ===============================
        // THÊM SẢN PHẨM
        [HttpPost]
        public IActionResult ThemSanPham(int maSanPham, int soLuong = 1)
        {
            var sp = _context.SanPhams.FirstOrDefault(x => x.MaSanPham == maSanPham);
            if (sp == null) return NotFound();

            if (soLuong > sp.SoLuong)
            {
                TempData["CheckoutError"] = $"Sản phẩm chỉ còn {sp.SoLuong} sản phẩm.";
                return RedirectToAction("Index");
            }

            var cart = GetOrCreateCart();

            var item = cart.GioHangChiTiets
                .FirstOrDefault(x => x.LoaiItem == "SP" && x.MaItem == maSanPham);

            if (item != null)
            {
                if (item.SoLuong + soLuong > sp.SoLuong)
                {
                    TempData["CheckoutError"] = $"Sản phẩm chỉ còn {sp.SoLuong} sản phẩm.";
                    return RedirectToAction("Index");
                }

                item.SoLuong += soLuong;
            }
            else
            {
                cart.GioHangChiTiets.Add(new GioHangChiTiet
                {
                    LoaiItem = "SP",
                    MaItem = maSanPham,
                    SoLuong = soLuong,
                    DonGia = sp.Gia
                });
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        // ===============================
        // THÊM DỊCH VỤ
        // ===============================
        [HttpPost]
        public IActionResult ThemDichVu(int maDichVu, int soLuong = 1)
        {
            var dv = _context.DichVus.FirstOrDefault(x => x.MaDichVu == maDichVu);
            if (dv == null) return NotFound();

            var cart = GetOrCreateCart();

            var item = cart.GioHangChiTiets
                .FirstOrDefault(x => x.LoaiItem == "DV" && x.MaItem == maDichVu);

            if (item != null)
            {
                item.SoLuong += soLuong;
                item.NgayCapNhat = DateTime.Now;
            }
            else
            {
                cart.GioHangChiTiets.Add(new GioHangChiTiet
                {
                    LoaiItem = "DV",
                    MaItem = maDichVu,
                    SoLuong = soLuong,
                    DonGia = dv.Gia ?? 0,
                    NgayCapNhat = DateTime.Now
                });
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        // ===============================
        // CẬP NHẬT SỐ LƯỢNG ITEM
        // ===============================
        public IActionResult CapNhatSoLuong(string loai, int id, int soLuong)
        {
            if (soLuong < 1)
            {
                // nếu giảm về 0 thì xóa luôn
                return RedirectToAction("XoaItem", new { maCt = id });
            }

            var cart = GetOrCreateCart();

            var item = cart.GioHangChiTiets.FirstOrDefault(x => x.MaCt == id);
            if (item == null)
            {
                return RedirectToAction("Index");
            }

            item.SoLuong = soLuong;
            item.NgayCapNhat = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        // ===============================
        // XÓA ITEM KHỎI GIỎ HÀNG
        // ===============================
        [HttpPost]
        public IActionResult XoaItem(int maCt)
        {
            var cart = GetOrCreateCart();

            var item = cart.GioHangChiTiets.FirstOrDefault(x => x.MaCt == maCt);
            if (item != null)
            {
                _context.GioHangChiTiets.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ===============================
        // HIỂN THỊ GIỎ HÀNG
        // ===============================
        public IActionResult Index()
        {
            var cart = GetOrCreateCart();

            var viewModel = cart.GioHangChiTiets.Select(ct =>
            {
                if (ct.LoaiItem == "SP")
                {
                    var sp = _context.SanPhams.FirstOrDefault(x => x.MaSanPham == ct.MaItem);
                    return new GioHangItemVM
                    {
                        MaCt = ct.MaCt,
                        Ten = sp?.TenSanPham,
                        HinhAnh = sp?.HinhAnh,
                        DonGia = ct.DonGia,
                        SoLuong = ct.SoLuong,
                        LoaiItem = "SP"
                    };
                }
                else
                {
                    var dv = _context.DichVus.FirstOrDefault(x => x.MaDichVu == ct.MaItem);
                    return new GioHangItemVM
                    {
                        MaCt = ct.MaCt,
                        Ten = dv?.TenDichVu,
                        HinhAnh = dv?.HinhAnh,
                        DonGia = ct.DonGia,
                        SoLuong = ct.SoLuong,
                        LoaiItem = "DV"
                    };
                }
            }).ToList();

            return View(viewModel);
        }
    }
}
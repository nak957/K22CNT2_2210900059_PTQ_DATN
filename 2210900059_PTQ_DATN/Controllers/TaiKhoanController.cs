using Microsoft.AspNetCore.Mvc;
using _2210900059_PTQ_DATN.Models;
using Microsoft.EntityFrameworkCore;
using _2210900059_PTQ_DATN.Models.ViewModels;

namespace _2210900059_PTQ_DATN.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly LeSkinDbContext _context;

        public TaiKhoanController(LeSkinDbContext context)
        {
            _context = context;
        }

        // ===================== LOGIN =====================

        // GET: /TaiKhoan/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /TaiKhoan/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var nguoiDung = _context.NguoiDungs
                .FirstOrDefault(u =>
                    u.TenDangNhap == username &&
                    u.MatKhau == password);

            if (nguoiDung == null)
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }

            // Lưu session
            HttpContext.Session.SetInt32("MaNguoiDung", nguoiDung.MaNguoiDung);
            HttpContext.Session.SetString("TenDangNhap", nguoiDung.TenDangNhap);
            HttpContext.Session.SetString("VaiTro", nguoiDung.VaiTro);

            // Phân quyền
            if (nguoiDung.VaiTro == "admin")
            {
                return RedirectToAction(
                    "Index",
                    "Home",
                    new { area = "Admins" }
                );
            }

            return RedirectToAction("Index", "Home");
        }

        // ===================== REGISTER =====================

        // GET: /TaiKhoan/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra trùng username
            if (_context.NguoiDungs.Any(u => u.TenDangNhap == model.TenDangNhap))
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại");
                return View(model);
            }

            // Kiểm tra trùng email
            if (!string.IsNullOrEmpty(model.Email) &&
                _context.NguoiDungs.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng");
                return View(model);
            }

            // ===== TẠO NGƯỜI DÙNG =====
            var nguoiDung = new NguoiDung
            {
                TenDangNhap = model.TenDangNhap,
                MatKhau = model.MatKhau, // TODO: hash sau
                HoTen = model.HoTen,
                Email = model.Email,
                VaiTro = "khachhang",
                NgayTao = DateTime.Now
            };

            _context.NguoiDungs.Add(nguoiDung);
            _context.SaveChanges(); // QUAN TRỌNG: để có MaNguoiDung

            // ===== TẠO LIÊN HỆ =====
            var lienHe = new LienHe
            {
                HoTen = model.HoTen ?? model.TenDangNhap,
                Email = model.Email,
                SoDienThoai = model.SoDienThoai,
                TieuDe = "Thông tin khách hàng mới",
                LoaiLienHe = "Khách hàng",
                NoiDung = model.DiaChi ?? "Chưa cung cấp địa chỉ",
                TrangThai = "Mới",
                DaDoc = false,
                NgayGui = DateTime.Now,
                MaNguoiDung = nguoiDung.MaNguoiDung
            };

            _context.LienHes.Add(lienHe);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }


        // ===================== EDIT =====================
        // GET: /TaiKhoan/Edit
        [HttpGet]
        public IActionResult Edit()
        {
            int? maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null)
            {
                return RedirectToAction("Login");
            }

            var nguoiDung = _context.NguoiDungs
                .FirstOrDefault(x => x.MaNguoiDung == maNguoiDung);

            if (nguoiDung == null)
            {
                return NotFound();
            }

            var model = new EditTaiKhoanViewModel
            {
                MaNguoiDung = nguoiDung.MaNguoiDung,
                HoTen = nguoiDung.HoTen,
                Email = nguoiDung.Email
            };

            return View(model);
        }
        // POST: /TaiKhoan/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditTaiKhoanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var nguoiDung = _context.NguoiDungs
                .FirstOrDefault(x => x.MaNguoiDung == model.MaNguoiDung);

            if (nguoiDung == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin cá nhân
            nguoiDung.HoTen = model.HoTen;
            nguoiDung.Email = model.Email;

            // ====== ĐỔI MẬT KHẨU (NẾU CÓ NHẬP) ======
            if (!string.IsNullOrEmpty(model.MatKhauMoi))
            {
                if (nguoiDung.MatKhau != model.MatKhauCu)
                {
                    ModelState.AddModelError(
                        "MatKhauCu",
                        "Mật khẩu cũ không đúng"
                    );
                    return View(model);
                }

                nguoiDung.MatKhau = model.MatKhauMoi;
            }

            _context.SaveChanges();

            ViewBag.Success = "Cập nhật thông tin thành công";
            return View(model);
        }

        // ===================== LOGOUT =====================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

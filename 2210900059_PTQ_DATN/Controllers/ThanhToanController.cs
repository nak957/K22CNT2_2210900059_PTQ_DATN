using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2210900059_PTQ_DATN.Models;

namespace _2210900059_PTQ_DATN.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly LeSkinDbContext _context;

        public ThanhToanController(LeSkinDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // ĐIỂM VÀO KHI BẤM "TIẾN HÀNH THANH TOÁN"
        // =====================================================
        public IActionResult Index()
        {
            // Lấy userId từ session (bạn đang dùng session, không phải Identity)
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");

            // ======================
            // CASE 1: CHƯA ĐĂNG NHẬP
            // ======================
            if (maNguoiDung == null)
            {
                return RedirectToAction("ThongTinLienHe");
            }

            // ======================
            // CASE 2: ĐÃ ĐĂNG NHẬP
            // ======================
            var lienHe = _context.LienHes
                .FirstOrDefault(x => x.MaNguoiDung == maNguoiDung);

            // Chưa có thông tin liên hệ → bắt nhập
            if (lienHe == null)
            {
                return RedirectToAction("ThongTinLienHe");
            }

            // Có đủ thông tin → sang xác nhận
            return RedirectToAction("XacNhan");
        }

        // =====================================================
        // FORM LIÊN HỆ (GET)
        // =====================================================
        [HttpGet]
        public IActionResult ThongTinLienHe()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");

            // Nếu đã đăng nhập → load sẵn dữ liệu (nếu có)
            if (maNguoiDung != null)
            {
                var lienHe = _context.LienHes
                    .FirstOrDefault(x => x.MaNguoiDung == maNguoiDung);

                if (lienHe != null)
                {
                    return View(lienHe);
                }
            }

            return View(new LienHe());
        }

        // =====================================================
        // FORM LIÊN HỆ (POST)
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThongTinLienHe(LienHe model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");

            // Gán user nếu đã đăng nhập
            if (maNguoiDung != null)
            {
                model.MaNguoiDung = maNguoiDung.Value;
            }

            model.NgayGui = DateTime.Now;
            model.DaDoc = false;
            model.TrangThai = "Chưa xử lý";

            _context.LienHes.Add(model);
            _context.SaveChanges();

            // Lưu ID liên hệ vào session để bước sau dùng
            HttpContext.Session.SetInt32("MaLienHe", model.MaLienHe);

            return RedirectToAction("XacNhan");
        }

        // =====================================================
        // TRANG XÁC NHẬN THANH TOÁN
        // =====================================================
        public IActionResult XacNhan()
        {
            // Kiểm tra lại thông tin liên hệ
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            LienHe lienHe = null;

            if (maNguoiDung != null)
            {
                lienHe = _context.LienHes
                    .FirstOrDefault(x => x.MaNguoiDung == maNguoiDung);
            }
            else
            {
                var maLienHe = HttpContext.Session.GetInt32("MaLienHe");
                if (maLienHe != null)
                {
                    lienHe = _context.LienHes
                        .FirstOrDefault(x => x.MaLienHe == maLienHe);
                }
            }

            if (lienHe == null)
            {
                return RedirectToAction("ThongTinLienHe");
            }

            return View(lienHe);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using _2210900059_PTQ_DATN.Models;

namespace _2210900059_PTQ_DATN.Controllers
{
    public class LienHeController : Controller
    {
        private readonly LeSkinDbContext _context;

        public LienHeController(LeSkinDbContext context)
        {
            _context = context;
        }

        // ==================================
        // GET: /LienHe
        // Hiển thị form liên hệ
        // ==================================
        [HttpGet]
        public IActionResult Index()
        {
            var model = new LienHe();

            // Kiểm tra đăng nhập
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");

            if (maNguoiDung.HasValue)
            {
                var nguoiDung = _context.NguoiDungs.Find(maNguoiDung.Value);
                if (nguoiDung != null)
                {
                    // ✔ CHỈ fill các field CÓ trong NguoiDung
                    model.HoTen = nguoiDung.HoTen ?? "";
                    model.Email = nguoiDung.Email;
                }
            }

            return View(model);
        }

        // ==================================
        // POST: /LienHe
        // Xử lý gửi liên hệ
        // ==================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LienHe model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Nếu đã đăng nhập → gán MaNguoiDung
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            model.MaNguoiDung = maNguoiDung; // null nếu là khách vãng lai ✔

            // Thiết lập dữ liệu hệ thống
            model.NgayGui = DateTime.Now;
            model.DaDoc = false;
            model.TrangThai = "Chưa xử lý";

            _context.LienHes.Add(model);
            _context.SaveChanges();

            TempData["success"] = "Cảm ơn bạn đã liên hệ! Trung tâm sẽ phản hồi sớm nhất.";

            return RedirectToAction("Index");
        }
    }
}
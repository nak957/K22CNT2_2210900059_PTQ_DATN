using Microsoft.AspNetCore.Mvc;
using _2210900059_PTQ_DATN.Models;
using Microsoft.EntityFrameworkCore;

namespace _2210900059_PTQ_DATN.Controllers
{
    public class GioiThieuController : Controller
    {
        private readonly LeSkinDbContext _context;

        public GioiThieuController(LeSkinDbContext context)
        {
            _context = context;
        }

        // GET: /GioiThieu
        public IActionResult Index()
        {
            var trang = _context.TrangNoiDungs
                .Where(t => t.LoaiTrang == "gioi-thieu" && t.HienThi == true)
                .OrderBy(t => t.ThuTu)
                .FirstOrDefault();

            if (trang == null)
            {
                return NotFound();
            }

            return View(trang);
        }
    }
}
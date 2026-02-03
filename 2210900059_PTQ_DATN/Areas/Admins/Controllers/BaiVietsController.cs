using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _2210900059_PTQ_DATN.Models;

namespace _2210900059_PTQ_DATN.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class BaiVietsController : Controller
    {
        private readonly LeSkinDbContext _context;

        public BaiVietsController(LeSkinDbContext context)
        {
            _context = context;
        }

        // =======================
        // GET: Index
        // =======================
        public async Task<IActionResult> Index()
        {
            var data = _context.BaiViets
                .Include(b => b.MaNguoiTaoNavigation)
                .Include(b => b.MaNguoiCapNhatNavigation);

            return View(await data.ToListAsync());
        }

        // =======================
        // GET: Create
        // =======================
        public IActionResult Create()
        {
            ViewData["Loai"] =
                new SelectList(new List<string> { "Tin tức", "Sự kiện" });

            return View();
        }

        // =======================
        // POST: Create
        // =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BaiViet baiViet, IFormFile fileAnh)
        {
            // FIX 1: gán người tạo
            baiViet.MaNguoiTao =
                HttpContext.Session.GetInt32("MaNguoiDung");

            // FIX 2: ngày đăng
            baiViet.NgayDang ??= DateTime.Now;

            // Upload ảnh
            if (fileAnh != null && fileAnh.Length > 0)
            {
                baiViet.HinhAnh = await UploadAnh(fileAnh);
            }

            if (!ModelState.IsValid)
            {
                ViewData["Loai"] =
                    new SelectList(new List<string> { "Tin tức", "Sự kiện" }, baiViet.Loai);

                return View(baiViet);
            }

            _context.BaiViets.Add(baiViet);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =======================
        // GET: Edit
        // =======================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var baiViet = await _context.BaiViets.FindAsync(id);
            if (baiViet == null) return NotFound();

            ViewData["Loai"] =
                new SelectList(new List<string> { "Tin tức", "Sự kiện" }, baiViet.Loai);

            return View(baiViet);
        }

        // =======================
        // POST: Edit
        // =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BaiViet baiViet, IFormFile fileAnh)
        {
            if (id != baiViet.MaBaiViet)
                return NotFound();

            var baiVietDb = await _context.BaiViets.FindAsync(id);
            if (baiVietDb == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["Loai"] =
                    new SelectList(new List<string> { "Tin tức", "Sự kiện" }, baiViet.Loai);

                return View(baiViet);
            }

            baiVietDb.TieuDe = baiViet.TieuDe;
            baiVietDb.Slug = baiViet.Slug;
            baiVietDb.MoTa = baiViet.MoTa;
            baiVietDb.NoiDung = baiViet.NoiDung;
            baiVietDb.Loai = baiViet.Loai;
            baiVietDb.NoiBat = baiViet.NoiBat;
            baiVietDb.NgayDang = baiViet.NgayDang ?? DateTime.Now;

            // FIX 3: người cập nhật
            baiVietDb.MaNguoiCapNhat =
                HttpContext.Session.GetInt32("MaNguoiDung");

            if (fileAnh != null && fileAnh.Length > 0)
            {
                baiVietDb.HinhAnh = await UploadAnh(fileAnh);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // =======================
        // UPLOAD ẢNH (GIỮ NGUYÊN)
        // =======================
        private async Task<string> UploadAnh(IFormFile file)
        {
            var folder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/uploads/bai-viet");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/bai-viet/" + fileName;
        }
    }
}

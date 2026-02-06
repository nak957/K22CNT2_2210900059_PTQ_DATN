using _2210900059_PTQ_DATN.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _2210900059_PTQ_DATN.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class DichVusController : Controller
    {
        private readonly LeSkinDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DichVusController(LeSkinDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ==========================
        // INDEX + LỌC THEO DANH MỤC
        // ==========================
        public async Task<IActionResult> Index(int? maDanhMuc)
        {
            var query = _context.DichVus
                .Include(d => d.MaDanhMucDvNavigation)
                .Include(d => d.MaNguoiCapNhatNavigation)
                .Include(d => d.MaNguoiTaoNavigation)
                .AsQueryable();

            if (maDanhMuc.HasValue)
            {
                query = query.Where(d => d.MaDanhMucDv == maDanhMuc);
            }

            ViewBag.DanhMucs = await _context.DanhMucDichVus.ToListAsync();
            ViewBag.MaDanhMucDangChon = maDanhMuc;

            return View(await query.ToListAsync());
        }

        // ==========================
        // DETAILS
        // ==========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var dichVu = await _context.DichVus
                .Include(d => d.MaDanhMucDvNavigation)
                .Include(d => d.MaNguoiCapNhatNavigation)
                .Include(d => d.MaNguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaDichVu == id);

            if (dichVu == null) return NotFound();

            return View(dichVu);
        }

        // ==========================
        // CREATE
        // ==========================
        public IActionResult Create()
        {
            ViewData["MaDanhMucDv"] = new SelectList(
                _context.DanhMucDichVus,
                "MaDanhMucDv",
                "TenDanhMuc"
            );

            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "HoTen");
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "HoTen");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            DichVu dichVu,
            IFormFile? HinhAnhFile
        )
        {
            if (ModelState.IsValid)
            {
                if (HinhAnhFile != null && HinhAnhFile.Length > 0)
                {
                    string uploadPath = Path.Combine(
                        _environment.WebRootPath ?? "wwwroot",
                        "images",
                        "services"
                    );

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    string fileName = Guid.NewGuid() + Path.GetExtension(HinhAnhFile.FileName);
                    string filePath = Path.Combine(uploadPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await HinhAnhFile.CopyToAsync(stream);

                    dichVu.HinhAnh = fileName;
                }

                dichVu.NgayTao ??= DateTime.Now;

                _context.Add(dichVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaDanhMucDv"] = new SelectList(
                _context.DanhMucDichVus,
                "MaDanhMucDv",
                "TenDanhMuc",
                dichVu.MaDanhMucDv
            );

            return View(dichVu);
        }

        // ==========================
        // EDIT
        // ==========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu == null) return NotFound();

            ViewData["MaDanhMucDv"] = new SelectList(
                _context.DanhMucDichVus,
                "MaDanhMucDv",
                "TenDanhMuc",
                dichVu.MaDanhMucDv
            );

            return View(dichVu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DichVu dichVu, IFormFile? HinhAnhFile)
        {
            if (id != dichVu.MaDichVu) return NotFound();

            if (ModelState.IsValid)
            {
                if (HinhAnhFile != null && HinhAnhFile.Length > 0)
                {
                    string uploadPath = Path.Combine(
                        _environment.WebRootPath ?? "wwwroot",
                        "images",
                        "services"
                    );

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    string fileName = Guid.NewGuid() + Path.GetExtension(HinhAnhFile.FileName);
                    string filePath = Path.Combine(uploadPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await HinhAnhFile.CopyToAsync(stream);

                    dichVu.HinhAnh = fileName;
                }
                else
                {
                    dichVu.HinhAnh = await _context.DichVus
                        .AsNoTracking()
                        .Where(d => d.MaDichVu == id)
                        .Select(d => d.HinhAnh)
                        .FirstOrDefaultAsync();
                }

                _context.Update(dichVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(dichVu);
        }

        // ==========================
        // DELETE
        // ==========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dichVu = await _context.DichVus
                .Include(d => d.MaDanhMucDvNavigation)
                .FirstOrDefaultAsync(m => m.MaDichVu == id);

            if (dichVu == null) return NotFound();

            return View(dichVu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu != null)
            {
                _context.DichVus.Remove(dichVu);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

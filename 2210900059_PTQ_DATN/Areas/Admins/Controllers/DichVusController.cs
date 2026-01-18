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

        // GET: Admins/DichVus
        public async Task<IActionResult> Index()
        {
            var leSkinDbContext = _context.DichVus.Include(d => d.MaDanhMucDvNavigation).Include(d => d.MaNguoiCapNhatNavigation).Include(d => d.MaNguoiTaoNavigation);
            return View(await leSkinDbContext.ToListAsync());
        }

        // GET: Admins/DichVus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus
                .Include(d => d.MaDanhMucDvNavigation)
                .Include(d => d.MaNguoiCapNhatNavigation)
                .Include(d => d.MaNguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaDichVu == id);
            if (dichVu == null)
            {
                return NotFound();
            }

            return View(dichVu);
        }

        // GET: Admins/DichVus/Create
        public IActionResult Create()
        {
            ViewData["MaDanhMucDv"] = new SelectList(
                _context.DanhMucDichVus,
                "MaDanhMucDv",      // value
                "TenDanhMuc"        // text hiển thị
            );

            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "HoTen");
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "HoTen");
            return View();
        }

        // POST: Admins/DichVus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("MaDichVu,MaDanhMucDv,TenDichVu,Slug,LoaiDichVu,CongNghe,ThoiLuong,SoBuoi,DoiTuongPhuHop,VungTacDong,MoTaNgan,NoiDungChiTiet,HinhAnh,Gia,NoiBat,NgayTao,TrangThai,MaNguoiTao,MaNguoiCapNhat")]
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

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await HinhAnhFile.CopyToAsync(stream);
                    }

                    dichVu.HinhAnh = fileName; // chỉ lưu tên file
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

            ViewData["MaNguoiCapNhat"] = new SelectList(
                _context.NguoiDungs,
                "MaNguoiDung",
                "HoTen",
                dichVu.MaNguoiCapNhat
            );

            ViewData["MaNguoiTao"] = new SelectList(
                _context.NguoiDungs,
                "MaNguoiDung",
                "HoTen",
                dichVu.MaNguoiTao
            );

            return View(dichVu);
        }

        // GET: Admins/DichVus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu == null)
            {
                return NotFound();
            }

            ViewData["MaDanhMucDv"] = new SelectList(_context.DanhMucDichVus, "MaDanhMucDv", "TenDanhMuc", dichVu.MaDanhMucDv);
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "HoTen", dichVu.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "HoTen", dichVu.MaNguoiTao);
            return View(dichVu);
        }

        // POST: Admins/DichVus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("MaDichVu,MaDanhMucDv,TenDichVu,Slug,LoaiDichVu,CongNghe,ThoiLuong,SoBuoi,DoiTuongPhuHop,VungTacDong,MoTaNgan,NoiDungChiTiet,HinhAnh,Gia,NoiBat,NgayTao,TrangThai,MaNguoiTao,MaNguoiCapNhat")]
            DichVu dichVu,
            IFormFile? HinhAnhFile
        )
        {
            if (id != dichVu.MaDichVu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // If a new image is uploaded, save it and set HinhAnh
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

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnhFile.CopyToAsync(stream);
                        }

                        dichVu.HinhAnh = fileName;
                    }
                    else
                    {
                        // Preserve existing image when no new file uploaded
                        var existingImage = await _context.DichVus
                            .AsNoTracking()
                            .Where(d => d.MaDichVu == id)
                            .Select(d => d.HinhAnh)
                            .FirstOrDefaultAsync();

                        dichVu.HinhAnh = existingImage;
                    }

                    _context.Update(dichVu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DichVuExists(dichVu.MaDichVu))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaDanhMucDv"] = new SelectList(_context.DanhMucDichVus, "MaDanhMucDv", "TenDanhMuc", dichVu.MaDanhMucDv);
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "HoTen", dichVu.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "HoTen", dichVu.MaNguoiTao);
            return View(dichVu);
        }

        // GET: Admins/DichVus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus
                .Include(d => d.MaDanhMucDvNavigation)
                .Include(d => d.MaNguoiCapNhatNavigation)
                .Include(d => d.MaNguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaDichVu == id);
            if (dichVu == null)
            {
                return NotFound();
            }

            return View(dichVu);
        }

        // POST: Admins/DichVus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu != null)
            {
                _context.DichVus.Remove(dichVu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DichVuExists(int id)
        {
            return _context.DichVus.Any(e => e.MaDichVu == id);
        }
    }
}

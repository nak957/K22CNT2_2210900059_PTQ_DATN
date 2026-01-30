using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _2210900059_PTQ_DATN.Models;

namespace _2210900059_PTQ_DATN.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class SanPhamsController : Controller
    {
        private readonly LeSkinDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SanPhamsController(LeSkinDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // =======================
        // GET: Admins/SanPhams
        // =======================
        public async Task<IActionResult> Index()
        {
            var data = _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .Include(s => s.MaNguoiTaoNavigation)
                .Include(s => s.MaNguoiCapNhatNavigation);

            return View(await data.ToListAsync());
        }

        // =======================
        // GET: Details
        // =======================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .Include(s => s.MaNguoiTaoNavigation)
                .Include(s => s.MaNguoiCapNhatNavigation)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);

            if (sanPham == null) return NotFound();

            return View(sanPham);
        }

        // =======================
        // GET: Create
        // =======================
        public IActionResult Create()
        {
            ViewData["MaDanhMuc"] = new SelectList(
                _context.DanhMucSanPhams,
                "MaDanhMuc",
                "TenDanhMuc"
            );

            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");

            return View();
        }

        // =======================
        // POST: Create
        // =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("MaDanhMuc,TenSanPham,Slug,LoaiSanPham,ThuongHieu,DungTich,DoiTuongSuDung,CongDungChinh,ThanhPhanNoiBat,MoTaNgan,MoTaChiTiet,HinhAnh,Gia,SoLuong,NoiBat,TrangThai,MaNguoiTao,MaNguoiCapNhat")]
            SanPham sanPham,
            IFormFile? HinhAnhFile
        )
        {
            if (sanPham.SoLuong < 0)
            {
                ModelState.AddModelError("SoLuong", "Số lượng phải ≥ 0");
            }

            if (ModelState.IsValid)
            {
                if (HinhAnhFile != null && HinhAnhFile.Length > 0)
                {
                    string uploadPath = Path.Combine(_environment.WebRootPath, "images", "products");
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    string fileName = Guid.NewGuid() + Path.GetExtension(HinhAnhFile.FileName);
                    string filePath = Path.Combine(uploadPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await HinhAnhFile.CopyToAsync(stream);

                    sanPham.HinhAnh = fileName;
                }

                sanPham.NgayTao = DateTime.Now;

                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaDanhMuc"] = new SelectList(
                _context.DanhMucSanPhams,
                "MaDanhMuc",
                "TenDanhMuc",
                sanPham.MaDanhMuc
            );

            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", sanPham.MaNguoiTao);
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", sanPham.MaNguoiCapNhat);

            return View(sanPham);
        }

        // =======================
        // GET: Edit
        // =======================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null) return NotFound();

            ViewData["MaDanhMuc"] = new SelectList(
                _context.DanhMucSanPhams,
                "MaDanhMuc",
                "TenDanhMuc",
                sanPham.MaDanhMuc
            );

            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", sanPham.MaNguoiTao);
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", sanPham.MaNguoiCapNhat);

            return View(sanPham);
        }

        // =======================
        // POST: Edit (AN TOÀN)
        // =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("MaSanPham,MaDanhMuc,TenSanPham,Slug,LoaiSanPham,ThuongHieu,DungTich,DoiTuongSuDung,CongDungChinh,ThanhPhanNoiBat,MoTaNgan,MoTaChiTiet,HinhAnh,Gia,SoLuong,NoiBat,TrangThai,MaNguoiTao,MaNguoiCapNhat")]
            SanPham sanPham,
            IFormFile? HinhAnhFile
        )
        {
            if (id != sanPham.MaSanPham)
                return NotFound();

            if (sanPham.SoLuong < 0)
            {
                ModelState.AddModelError("SoLuong", "Số lượng phải ≥ 0");
            }

            if (ModelState.IsValid)
            {
                var sanPhamDb = await _context.SanPhams.FindAsync(id);
                if (sanPhamDb == null) return NotFound();

                sanPhamDb.TenSanPham = sanPham.TenSanPham;
                sanPhamDb.Slug = sanPham.Slug;
                sanPhamDb.MaDanhMuc = sanPham.MaDanhMuc;
                sanPhamDb.LoaiSanPham = sanPham.LoaiSanPham;
                sanPhamDb.ThuongHieu = sanPham.ThuongHieu;
                sanPhamDb.DungTich = sanPham.DungTich;
                sanPhamDb.DoiTuongSuDung = sanPham.DoiTuongSuDung;
                sanPhamDb.CongDungChinh = sanPham.CongDungChinh;
                sanPhamDb.ThanhPhanNoiBat = sanPham.ThanhPhanNoiBat;
                sanPhamDb.MoTaNgan = sanPham.MoTaNgan;
                sanPhamDb.MoTaChiTiet = sanPham.MoTaChiTiet;
                sanPhamDb.Gia = sanPham.Gia;
                sanPhamDb.SoLuong = sanPham.SoLuong;
                sanPhamDb.NoiBat = sanPham.NoiBat;
                sanPhamDb.TrangThai = sanPham.TrangThai;
                sanPhamDb.MaNguoiCapNhat = sanPham.MaNguoiCapNhat;

                if (HinhAnhFile != null && HinhAnhFile.Length > 0)
                {
                    string uploadPath = Path.Combine(_environment.WebRootPath, "images", "products");
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    string fileName = Guid.NewGuid() + Path.GetExtension(HinhAnhFile.FileName);
                    string filePath = Path.Combine(uploadPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await HinhAnhFile.CopyToAsync(stream);

                    sanPhamDb.HinhAnh = fileName;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaDanhMuc"] = new SelectList(_context.DanhMucSanPhams, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", sanPham.MaNguoiTao);
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", sanPham.MaNguoiCapNhat);

            return View(sanPham);
        }

        // =======================
        // DELETE
        // =======================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);

            if (sanPham == null) return NotFound();

            return View(sanPham);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

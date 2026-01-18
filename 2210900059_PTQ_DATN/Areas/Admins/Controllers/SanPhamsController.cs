using System;
using System.Collections.Generic;
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


        // GET: Admins/SanPhams
        public async Task<IActionResult> Index()
        {
            var leSkinDbContext = _context.SanPhams.Include(s => s.MaDanhMucNavigation).Include(s => s.MaNguoiCapNhatNavigation).Include(s => s.MaNguoiTaoNavigation);
            return View(await leSkinDbContext.ToListAsync());
        }

        // GET: Admins/SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .Include(s => s.MaNguoiCapNhatNavigation)
                .Include(s => s.MaNguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: Admins/SanPhams/Create
        public IActionResult Create()
        {
            ViewData["MaDanhMuc"] = new SelectList(
                _context.DanhMucSanPhams,
                "MaDanhMuc",      // value gửi về
                "TenDanhMuc"      // text hiển thị
            );

            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");

            return View();
        }


        // POST: Admins/SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
        [Bind("MaSanPham,MaDanhMuc,TenSanPham,Slug,LoaiSanPham,ThuongHieu,DungTich,DoiTuongSuDung,CongDungChinh,ThanhPhanNoiBat,MoTaNgan,MoTaChiTiet,HinhAnh,Gia,NoiBat,NgayTao,TrangThai,MaNguoiTao,MaNguoiCapNhat")]
        SanPham sanPham,
        IFormFile? HinhAnhFile
)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnhFile != null && HinhAnhFile.Length > 0)
                {
                    string uploadPath = Path.Combine(_environment.WebRootPath, "images", "products");

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(HinhAnhFile.FileName);
                    string filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await HinhAnhFile.CopyToAsync(stream);
                    }

                    sanPham.HinhAnh = fileName; // CHỈ lưu tên file
                }

                sanPham.NgayTao ??= DateTime.Now;

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

            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", sanPham.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", sanPham.MaNguoiTao);
            return View(sanPham);
        }

        // GET: Admins/SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
                return NotFound();

            ViewData["MaDanhMuc"] = new SelectList(
                _context.DanhMucSanPhams,
                "MaDanhMuc",
                "TenDanhMuc",
                sanPham.MaDanhMuc
            );

            ViewData["MaNguoiCapNhat"] = new SelectList(
                _context.NguoiDungs,
                "MaNguoiDung",
                "MaNguoiDung",
                sanPham.MaNguoiCapNhat
            );

            ViewData["MaNguoiTao"] = new SelectList(
                _context.NguoiDungs,
                "MaNguoiDung",
                "MaNguoiDung",
                sanPham.MaNguoiTao
            );

            return View(sanPham);
        }
    

        // POST: Admins/SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
        int id,
        [Bind("MaSanPham,MaDanhMuc,TenSanPham,Slug,LoaiSanPham,ThuongHieu,DungTich,DoiTuongSuDung,CongDungChinh,ThanhPhanNoiBat,MoTaNgan,MoTaChiTiet,HinhAnh,Gia,NoiBat,NgayTao,TrangThai,MaNguoiTao,MaNguoiCapNhat")]
        SanPham sanPham,
        IFormFile? HinhAnhFile
        )
            {
                if (id != sanPham.MaSanPham)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        if (HinhAnhFile != null && HinhAnhFile.Length > 0)
                        {
                            string uploadPath = Path.Combine(_environment.WebRootPath, "images", "products");

                            if (!Directory.Exists(uploadPath))
                                Directory.CreateDirectory(uploadPath);

                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(HinhAnhFile.FileName);
                            string filePath = Path.Combine(uploadPath, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await HinhAnhFile.CopyToAsync(stream);
                            }

                            sanPham.HinhAnh = fileName;
                        }
                        // nếu không chọn ảnh mới → giữ ảnh cũ (đã có hidden field)

                        _context.Update(sanPham);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SanPhamExists(sanPham.MaSanPham))
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

                    ViewData["MaDanhMuc"] = new SelectList(
                        _context.DanhMucSanPhams,
                        "MaDanhMuc",
                        "TenDanhMuc",
                        sanPham.MaDanhMuc
                    );

                    ViewData["MaNguoiCapNhat"] = new SelectList(
                        _context.NguoiDungs,
                        "MaNguoiDung",
                        "MaNguoiDung",
                        sanPham.MaNguoiCapNhat
                    );

                    ViewData["MaNguoiTao"] = new SelectList(
                        _context.NguoiDungs,
                        "MaNguoiDung",
                        "MaNguoiDung",
                        sanPham.MaNguoiTao
                    );

                    return View(sanPham);

                }


        // GET: Admins/SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .Include(s => s.MaNguoiCapNhatNavigation)
                .Include(s => s.MaNguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: Admins/SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.MaSanPham == id);
        }
    }
}

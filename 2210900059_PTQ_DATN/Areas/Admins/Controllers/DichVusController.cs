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
    public class DichVusController : Controller
    {
        private readonly LeSkinDbContext _context;

        public DichVusController(LeSkinDbContext context)
        {
            _context = context;
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
            ViewData["MaDanhMucDv"] = new SelectList(_context.DanhMucDichVus, "MaDanhMucDv", "MaDanhMucDv");
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");
            return View();
        }

        // POST: Admins/DichVus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDichVu,MaDanhMucDv,TenDichVu,Slug,LoaiDichVu,CongNghe,ThoiLuong,SoBuoi,DoiTuongPhuHop,VungTacDong,MoTaNgan,NoiDungChiTiet,HinhAnh,Gia,NoiBat,NgayTao,TrangThai,MaNguoiTao,MaNguoiCapNhat")] DichVu dichVu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dichVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDanhMucDv"] = new SelectList(_context.DanhMucDichVus, "MaDanhMucDv", "MaDanhMucDv", dichVu.MaDanhMucDv);
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", dichVu.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", dichVu.MaNguoiTao);
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
            ViewData["MaDanhMucDv"] = new SelectList(_context.DanhMucDichVus, "MaDanhMucDv", "MaDanhMucDv", dichVu.MaDanhMucDv);
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", dichVu.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", dichVu.MaNguoiTao);
            return View(dichVu);
        }

        // POST: Admins/DichVus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDichVu,MaDanhMucDv,TenDichVu,Slug,LoaiDichVu,CongNghe,ThoiLuong,SoBuoi,DoiTuongPhuHop,VungTacDong,MoTaNgan,NoiDungChiTiet,HinhAnh,Gia,NoiBat,NgayTao,TrangThai,MaNguoiTao,MaNguoiCapNhat")] DichVu dichVu)
        {
            if (id != dichVu.MaDichVu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["MaDanhMucDv"] = new SelectList(_context.DanhMucDichVus, "MaDanhMucDv", "MaDanhMucDv", dichVu.MaDanhMucDv);
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", dichVu.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", dichVu.MaNguoiTao);
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

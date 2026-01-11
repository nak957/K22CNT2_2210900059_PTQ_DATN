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
    public class DanhMucDichVusController : Controller
    {
        private readonly LeSkinDbContext _context;

        public DanhMucDichVusController(LeSkinDbContext context)
        {
            _context = context;
        }

        // GET: Admins/DanhMucDichVus
        public async Task<IActionResult> Index()
        {
            var leSkinDbContext = _context.DanhMucDichVus.Include(d => d.MaNguoiCapNhatNavigation).Include(d => d.MaNguoiTaoNavigation);
            return View(await leSkinDbContext.ToListAsync());
        }

        // GET: Admins/DanhMucDichVus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucDichVu = await _context.DanhMucDichVus
                .Include(d => d.MaNguoiCapNhatNavigation)
                .Include(d => d.MaNguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaDanhMucDv == id);
            if (danhMucDichVu == null)
            {
                return NotFound();
            }

            return View(danhMucDichVu);
        }

        // GET: Admins/DanhMucDichVus/Create
        public IActionResult Create()
        {
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");
            return View();
        }

        // POST: Admins/DanhMucDichVus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDanhMucDv,TenDanhMuc,Slug,MoTa,MaNguoiTao,MaNguoiCapNhat")] DanhMucDichVu danhMucDichVu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhMucDichVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", danhMucDichVu.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", danhMucDichVu.MaNguoiTao);
            return View(danhMucDichVu);
        }

        // GET: Admins/DanhMucDichVus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucDichVu = await _context.DanhMucDichVus.FindAsync(id);
            if (danhMucDichVu == null)
            {
                return NotFound();
            }
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", danhMucDichVu.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", danhMucDichVu.MaNguoiTao);
            return View(danhMucDichVu);
        }

        // POST: Admins/DanhMucDichVus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDanhMucDv,TenDanhMuc,Slug,MoTa,MaNguoiTao,MaNguoiCapNhat")] DanhMucDichVu danhMucDichVu)
        {
            if (id != danhMucDichVu.MaDanhMucDv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhMucDichVu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucDichVuExists(danhMucDichVu.MaDanhMucDv))
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
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", danhMucDichVu.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", danhMucDichVu.MaNguoiTao);
            return View(danhMucDichVu);
        }

        // GET: Admins/DanhMucDichVus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucDichVu = await _context.DanhMucDichVus
                .Include(d => d.MaNguoiCapNhatNavigation)
                .Include(d => d.MaNguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaDanhMucDv == id);
            if (danhMucDichVu == null)
            {
                return NotFound();
            }

            return View(danhMucDichVu);
        }

        // POST: Admins/DanhMucDichVus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhMucDichVu = await _context.DanhMucDichVus.FindAsync(id);
            if (danhMucDichVu != null)
            {
                _context.DanhMucDichVus.Remove(danhMucDichVu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhMucDichVuExists(int id)
        {
            return _context.DanhMucDichVus.Any(e => e.MaDanhMucDv == id);
        }
    }
}

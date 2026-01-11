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
    public class TrangNoiDungsController : Controller
    {
        private readonly LeSkinDbContext _context;

        public TrangNoiDungsController(LeSkinDbContext context)
        {
            _context = context;
        }

        // GET: Admins/TrangNoiDungs
        public async Task<IActionResult> Index()
        {
            var leSkinDbContext = _context.TrangNoiDungs.Include(t => t.MaNguoiCapNhatNavigation).Include(t => t.MaNguoiTaoNavigation);
            return View(await leSkinDbContext.ToListAsync());
        }

        // GET: Admins/TrangNoiDungs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trangNoiDung = await _context.TrangNoiDungs
                .Include(t => t.MaNguoiCapNhatNavigation)
                .Include(t => t.MaNguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaTrang == id);
            if (trangNoiDung == null)
            {
                return NotFound();
            }

            return View(trangNoiDung);
        }

        // GET: Admins/TrangNoiDungs/Create
        public IActionResult Create()
        {
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");
            return View();
        }

        // POST: Admins/TrangNoiDungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTrang,TieuDe,Slug,LoaiTrang,MoTaNgan,NoiDung,ThuTu,HienThi,NoiBat,NgayCapNhat,MaNguoiTao,MaNguoiCapNhat")] TrangNoiDung trangNoiDung)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trangNoiDung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", trangNoiDung.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", trangNoiDung.MaNguoiTao);
            return View(trangNoiDung);
        }

        // GET: Admins/TrangNoiDungs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trangNoiDung = await _context.TrangNoiDungs.FindAsync(id);
            if (trangNoiDung == null)
            {
                return NotFound();
            }
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", trangNoiDung.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", trangNoiDung.MaNguoiTao);
            return View(trangNoiDung);
        }

        // POST: Admins/TrangNoiDungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTrang,TieuDe,Slug,LoaiTrang,MoTaNgan,NoiDung,ThuTu,HienThi,NoiBat,NgayCapNhat,MaNguoiTao,MaNguoiCapNhat")] TrangNoiDung trangNoiDung)
        {
            if (id != trangNoiDung.MaTrang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trangNoiDung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrangNoiDungExists(trangNoiDung.MaTrang))
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
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", trangNoiDung.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", trangNoiDung.MaNguoiTao);
            return View(trangNoiDung);
        }

        // GET: Admins/TrangNoiDungs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trangNoiDung = await _context.TrangNoiDungs
                .Include(t => t.MaNguoiCapNhatNavigation)
                .Include(t => t.MaNguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaTrang == id);
            if (trangNoiDung == null)
            {
                return NotFound();
            }

            return View(trangNoiDung);
        }

        // POST: Admins/TrangNoiDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trangNoiDung = await _context.TrangNoiDungs.FindAsync(id);
            if (trangNoiDung != null)
            {
                _context.TrangNoiDungs.Remove(trangNoiDung);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrangNoiDungExists(int id)
        {
            return _context.TrangNoiDungs.Any(e => e.MaTrang == id);
        }
    }
}

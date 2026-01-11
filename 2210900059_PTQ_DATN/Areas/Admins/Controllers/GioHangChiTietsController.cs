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
    public class GioHangChiTietsController : Controller
    {
        private readonly LeSkinDbContext _context;

        public GioHangChiTietsController(LeSkinDbContext context)
        {
            _context = context;
        }

        // GET: Admins/GioHangChiTiets
        public async Task<IActionResult> Index()
        {
            var leSkinDbContext = _context.GioHangChiTiets.Include(g => g.MaGioHangNavigation).Include(g => g.MaSanPhamNavigation);
            return View(await leSkinDbContext.ToListAsync());
        }

        // GET: Admins/GioHangChiTiets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioHangChiTiet = await _context.GioHangChiTiets
                .Include(g => g.MaGioHangNavigation)
                .Include(g => g.MaSanPhamNavigation)
                .FirstOrDefaultAsync(m => m.MaCt == id);
            if (gioHangChiTiet == null)
            {
                return NotFound();
            }

            return View(gioHangChiTiet);
        }

        // GET: Admins/GioHangChiTiets/Create
        public IActionResult Create()
        {
            ViewData["MaGioHang"] = new SelectList(_context.GioHangs, "MaGioHang", "MaGioHang");
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "MaSanPham");
            return View();
        }

        // POST: Admins/GioHangChiTiets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCt,MaGioHang,MaSanPham,SoLuong,DonGia,ThanhTien,GhiChu,NgayCapNhat")] GioHangChiTiet gioHangChiTiet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gioHangChiTiet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaGioHang"] = new SelectList(_context.GioHangs, "MaGioHang", "MaGioHang", gioHangChiTiet.MaGioHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "MaSanPham", gioHangChiTiet.MaSanPham);
            return View(gioHangChiTiet);
        }

        // GET: Admins/GioHangChiTiets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioHangChiTiet = await _context.GioHangChiTiets.FindAsync(id);
            if (gioHangChiTiet == null)
            {
                return NotFound();
            }
            ViewData["MaGioHang"] = new SelectList(_context.GioHangs, "MaGioHang", "MaGioHang", gioHangChiTiet.MaGioHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "MaSanPham", gioHangChiTiet.MaSanPham);
            return View(gioHangChiTiet);
        }

        // POST: Admins/GioHangChiTiets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaCt,MaGioHang,MaSanPham,SoLuong,DonGia,ThanhTien,GhiChu,NgayCapNhat")] GioHangChiTiet gioHangChiTiet)
        {
            if (id != gioHangChiTiet.MaCt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gioHangChiTiet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GioHangChiTietExists(gioHangChiTiet.MaCt))
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
            ViewData["MaGioHang"] = new SelectList(_context.GioHangs, "MaGioHang", "MaGioHang", gioHangChiTiet.MaGioHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "MaSanPham", gioHangChiTiet.MaSanPham);
            return View(gioHangChiTiet);
        }

        // GET: Admins/GioHangChiTiets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioHangChiTiet = await _context.GioHangChiTiets
                .Include(g => g.MaGioHangNavigation)
                .Include(g => g.MaSanPhamNavigation)
                .FirstOrDefaultAsync(m => m.MaCt == id);
            if (gioHangChiTiet == null)
            {
                return NotFound();
            }

            return View(gioHangChiTiet);
        }

        // POST: Admins/GioHangChiTiets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gioHangChiTiet = await _context.GioHangChiTiets.FindAsync(id);
            if (gioHangChiTiet != null)
            {
                _context.GioHangChiTiets.Remove(gioHangChiTiet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GioHangChiTietExists(int id)
        {
            return _context.GioHangChiTiets.Any(e => e.MaCt == id);
        }
    }
}

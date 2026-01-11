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
    public class BaiVietsController : Controller
    {
        private readonly LeSkinDbContext _context;

        public BaiVietsController(LeSkinDbContext context)
        {
            _context = context;
        }

        // GET: Admins/BaiViets
        public async Task<IActionResult> Index()
        {
            var leSkinDbContext = _context.BaiViets.Include(b => b.MaNguoiCapNhatNavigation).Include(b => b.MaNguoiTaoNavigation);
            return View(await leSkinDbContext.ToListAsync());
        }

        // GET: Admins/BaiViets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiViet = await _context.BaiViets
                .Include(b => b.MaNguoiCapNhatNavigation)
                .Include(b => b.MaNguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaBaiViet == id);
            if (baiViet == null)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        // GET: Admins/BaiViets/Create
        public IActionResult Create()
        {
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");
            return View();
        }

        // POST: Admins/BaiViets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaBaiViet,TieuDe,Slug,HinhAnh,MoTa,NoiDung,Loai,NoiBat,NgayDang,MaNguoiTao,MaNguoiCapNhat")] BaiViet baiViet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(baiViet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", baiViet.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", baiViet.MaNguoiTao);
            return View(baiViet);
        }

        // GET: Admins/BaiViets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiViet = await _context.BaiViets.FindAsync(id);
            if (baiViet == null)
            {
                return NotFound();
            }
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", baiViet.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", baiViet.MaNguoiTao);
            return View(baiViet);
        }

        // POST: Admins/BaiViets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaBaiViet,TieuDe,Slug,HinhAnh,MoTa,NoiDung,Loai,NoiBat,NgayDang,MaNguoiTao,MaNguoiCapNhat")] BaiViet baiViet)
        {
            if (id != baiViet.MaBaiViet)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(baiViet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaiVietExists(baiViet.MaBaiViet))
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
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", baiViet.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", baiViet.MaNguoiTao);
            return View(baiViet);
        }

        // GET: Admins/BaiViets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiViet = await _context.BaiViets
                .Include(b => b.MaNguoiCapNhatNavigation)
                .Include(b => b.MaNguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaBaiViet == id);
            if (baiViet == null)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        // POST: Admins/BaiViets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var baiViet = await _context.BaiViets.FindAsync(id);
            if (baiViet != null)
            {
                _context.BaiViets.Remove(baiViet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BaiVietExists(int id)
        {
            return _context.BaiViets.Any(e => e.MaBaiViet == id);
        }
    }
}

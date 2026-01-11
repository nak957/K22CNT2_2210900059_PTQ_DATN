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
    public class DanhMucSanPhamsController : Controller
    {
        private readonly LeSkinDbContext _context;

        public DanhMucSanPhamsController(LeSkinDbContext context)
        {
            _context = context;
        }

        // GET: Admins/DanhMucSanPhams
        public async Task<IActionResult> Index()
        {
            var leSkinDbContext = _context.DanhMucSanPhams.Include(d => d.MaNguoiCapNhatNavigation).Include(d => d.MaNguoiTaoNavigation);
            return View(await leSkinDbContext.ToListAsync());
        }

        // GET: Admins/DanhMucSanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucSanPham = await _context.DanhMucSanPhams
                .Include(d => d.MaNguoiCapNhatNavigation)
                .Include(d => d.MaNguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaDanhMuc == id);
            if (danhMucSanPham == null)
            {
                return NotFound();
            }

            return View(danhMucSanPham);
        }

        // GET: Admins/DanhMucSanPhams/Create
        public IActionResult Create()
        {
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung");
            return View();
        }

        // POST: Admins/DanhMucSanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDanhMuc,TenDanhMuc,Slug,MoTa,MaNguoiTao,MaNguoiCapNhat")] DanhMucSanPham danhMucSanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhMucSanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", danhMucSanPham.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", danhMucSanPham.MaNguoiTao);
            return View(danhMucSanPham);
        }

        // GET: Admins/DanhMucSanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucSanPham = await _context.DanhMucSanPhams.FindAsync(id);
            if (danhMucSanPham == null)
            {
                return NotFound();
            }
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", danhMucSanPham.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", danhMucSanPham.MaNguoiTao);
            return View(danhMucSanPham);
        }

        // POST: Admins/DanhMucSanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDanhMuc,TenDanhMuc,Slug,MoTa,MaNguoiTao,MaNguoiCapNhat")] DanhMucSanPham danhMucSanPham)
        {
            if (id != danhMucSanPham.MaDanhMuc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhMucSanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucSanPhamExists(danhMucSanPham.MaDanhMuc))
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
            ViewData["MaNguoiCapNhat"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", danhMucSanPham.MaNguoiCapNhat);
            ViewData["MaNguoiTao"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "MaNguoiDung", danhMucSanPham.MaNguoiTao);
            return View(danhMucSanPham);
        }

        // GET: Admins/DanhMucSanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucSanPham = await _context.DanhMucSanPhams
                .Include(d => d.MaNguoiCapNhatNavigation)
                .Include(d => d.MaNguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaDanhMuc == id);
            if (danhMucSanPham == null)
            {
                return NotFound();
            }

            return View(danhMucSanPham);
        }

        // POST: Admins/DanhMucSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhMucSanPham = await _context.DanhMucSanPhams.FindAsync(id);
            if (danhMucSanPham != null)
            {
                _context.DanhMucSanPhams.Remove(danhMucSanPham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhMucSanPhamExists(int id)
        {
            return _context.DanhMucSanPhams.Any(e => e.MaDanhMuc == id);
        }
    }
}

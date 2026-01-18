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
    public class NguoiDungsController : Controller
    {
        private readonly LeSkinDbContext _context;

        public NguoiDungsController(LeSkinDbContext context)
        {
            _context = context;
        }

        // GET: Admins/NguoiDungs
        public async Task<IActionResult> Index()
        {
            return View(await _context.NguoiDungs.ToListAsync());
        }

        // GET: Admins/NguoiDungs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs
                .FirstOrDefaultAsync(m => m.MaNguoiDung == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }

        // GET: Admins/NguoiDungs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/NguoiDungs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("MaNguoiDung,TenDangNhap,MatKhau,HoTen,Email,VaiTro,NgayTao")]
            NguoiDung nguoiDung)
        {
            // Siết vai trò phía server
            if (nguoiDung.VaiTro != "admin" && nguoiDung.VaiTro != "khachhang")
            {
                ModelState.AddModelError("VaiTro", "Vai trò không hợp lệ");
            }

            if (ModelState.IsValid)
            {
                _context.Add(nguoiDung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(nguoiDung);
        }

        // GET: Admins/NguoiDungs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }
            return View(nguoiDung);
        }

        // POST: Admins/NguoiDungs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("MaNguoiDung,TenDangNhap,MatKhau,HoTen,Email,VaiTro,NgayTao")]
            NguoiDung nguoiDung)
        {
            if (id != nguoiDung.MaNguoiDung)
            {
                return NotFound();
            }

            // Siết vai trò phía server
            if (nguoiDung.VaiTro != "admin" && nguoiDung.VaiTro != "khachhang")
            {
                ModelState.AddModelError("VaiTro", "Vai trò không hợp lệ");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nguoiDung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NguoiDungExists(nguoiDung.MaNguoiDung))
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

            return View(nguoiDung);
        }

        // GET: Admins/NguoiDungs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs
                .FirstOrDefaultAsync(m => m.MaNguoiDung == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }

        // POST: Admins/NguoiDungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung != null)
            {
                _context.NguoiDungs.Remove(nguoiDung);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NguoiDungExists(int id)
        {
            return _context.NguoiDungs.Any(e => e.MaNguoiDung == id);
        }
    }
}

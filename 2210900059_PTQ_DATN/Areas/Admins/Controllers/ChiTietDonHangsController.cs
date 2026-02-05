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
    public class ChiTietDonHangsController : Controller
    {
        private readonly LeSkinDbContext _context;

        public ChiTietDonHangsController(LeSkinDbContext context)
        {
            _context = context;
        }

        // =========================
        // INDEX – LỌC THEO ĐƠN HÀNG
        // =========================
        public async Task<IActionResult> Index(int? maDonHang)
        {
            var query = _context.ChiTietDonHangs
                .Include(c => c.MaDonHangNavigation)
                .AsQueryable();

            if (maDonHang.HasValue)
            {
                query = query.Where(c => c.MaDonHang == maDonHang.Value);
                ViewBag.MaDonHang = maDonHang.Value;
            }

            return View(await query.ToListAsync());
        }

        // ======================================
        // DETAILS – CHI TIẾT ĐƠN HÀNG (THEO MÃ ĐƠN)
        // ======================================
        public async Task<IActionResult> Details(int maDonHang)
        {
            var chiTietDonHang = await _context.ChiTietDonHangs
                .Include(c => c.MaDonHangNavigation)
                .FirstOrDefaultAsync(c => c.MaDonHang == maDonHang);

            if (chiTietDonHang == null)
                return NotFound();

            // =========================
            // LẤY TÊN SP / DV THEO ITEM
            // =========================
            string itemName = "Không xác định";

            if (chiTietDonHang.ItemType == "SanPham")
            {
                itemName = await _context.SanPhams
                    .Where(x => x.MaSanPham == chiTietDonHang.ItemId)
                    .Select(x => x.TenSanPham)
                    .FirstOrDefaultAsync()
                    ?? "Sản phẩm không tồn tại";
            }
            else if (chiTietDonHang.ItemType == "DichVu")
            {
                itemName = await _context.DichVus
                    .Where(x => x.MaDichVu == chiTietDonHang.ItemId)
                    .Select(x => x.TenDichVu)
                    .FirstOrDefaultAsync()
                    ?? "Dịch vụ không tồn tại";
            }

            ViewBag.ItemName = itemName;

            return View(chiTietDonHang);
        }

        // GET: Admins/ChiTietDonHangs/Create
        public IActionResult Create()
        {
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang");
            return View();
        }

        // POST: Admins/ChiTietDonHangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChiTietDonHang chiTietDonHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietDonHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaDonHang"] = new SelectList(
                _context.DonHangs,
                "MaDonHang",
                "MaDonHang",
                chiTietDonHang.MaDonHang
            );

            return View(chiTietDonHang);
        }

        // GET: Admins/ChiTietDonHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var chiTietDonHang = await _context.ChiTietDonHangs.FindAsync(id);
            if (chiTietDonHang == null)
                return NotFound();

            ViewData["MaDonHang"] = new SelectList(
                _context.DonHangs,
                "MaDonHang",
                "MaDonHang",
                chiTietDonHang.MaDonHang
            );

            return View(chiTietDonHang);
        }

        // POST: Admins/ChiTietDonHangs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChiTietDonHang chiTietDonHang)
        {
            if (id != chiTietDonHang.MaChiTiet)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietDonHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ChiTietDonHangs.Any(e => e.MaChiTiet == chiTietDonHang.MaChiTiet))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["MaDonHang"] = new SelectList(
                _context.DonHangs,
                "MaDonHang",
                "MaDonHang",
                chiTietDonHang.MaDonHang
            );

            return View(chiTietDonHang);
        }

        // GET: Admins/ChiTietDonHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var chiTietDonHang = await _context.ChiTietDonHangs
                .Include(c => c.MaDonHangNavigation)
                .FirstOrDefaultAsync(m => m.MaChiTiet == id);

            if (chiTietDonHang == null)
                return NotFound();

            return View(chiTietDonHang);
        }

        // POST: Admins/ChiTietDonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chiTietDonHang = await _context.ChiTietDonHangs.FindAsync(id);
            if (chiTietDonHang != null)
            {
                _context.ChiTietDonHangs.Remove(chiTietDonHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

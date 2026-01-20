using _2210900059_PTQ_DATN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var items = await _context.GioHangChiTiets
                .Include(g => g.MaGioHangNavigation)
                .ToListAsync();

            var itemNames = new Dictionary<int, string>();
            var itemImages = new Dictionary<int, string>();
            var itemLoais = new Dictionary<int, string>();

            foreach (var ct in items)
            {
                var loai = (ct.LoaiItem ?? "").Trim();
                string name = null;
                string image = null;

                if (loai == "SP")
                {
                    var sp = await _context.SanPhams
                        .Where(s => s.MaSanPham == ct.MaItem)
                        .Select(s => new { s.TenSanPham, s.HinhAnh })
                        .FirstOrDefaultAsync();

                    name = sp?.TenSanPham;
                    image = sp?.HinhAnh;
                }
                else if (loai == "DV")
                {
                    var dv = await _context.DichVus
                        .Where(d => d.MaDichVu == ct.MaItem)
                        .Select(d => new { d.TenDichVu, d.HinhAnh })
                        .FirstOrDefaultAsync();

                    name = dv?.TenDichVu;
                    image = dv?.HinhAnh;
                }

                itemNames[ct.MaCt] = name ?? "(Không xác định)";
                itemImages[ct.MaCt] = image;
                itemLoais[ct.MaCt] = loai;
            }

            ViewBag.ItemNames = itemNames;
            ViewBag.ItemImages = itemImages;
            ViewBag.ItemLoais = itemLoais;

            return View(items);
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
                .FirstOrDefaultAsync(m => m.MaCt == id);
            if (gioHangChiTiet == null)
            {
                return NotFound();
            }

            // Resolve item info theo LoaiItem + MaItem
            string itemName = null;
            string itemImage = null;
            string itemLoai = gioHangChiTiet.LoaiItem ?? "";

            if (itemLoai == "SP")
            {
                var sp = await _context.SanPhams.FirstOrDefaultAsync(s => s.MaSanPham == gioHangChiTiet.MaItem);
                itemName = sp?.TenSanPham;
                itemImage = sp?.HinhAnh;
            }
            else if (itemLoai == "DV")
            {
                var dv = await _context.DichVus.FirstOrDefaultAsync(d => d.MaDichVu == gioHangChiTiet.MaItem);
                itemName = dv?.TenDichVu;
                itemImage = dv?.HinhAnh;
            }

            ViewBag.ItemName = itemName ?? "(Không xác định)";
            ViewBag.ItemImage = itemImage;
            ViewBag.ItemLoai = itemLoai;

            return View(gioHangChiTiet);
        }

        // GET: Admins/GioHangChiTiets/Create
        public IActionResult Create()
        {
            ViewData["MaGioHang"] = new SelectList(_context.GioHangs, "MaGioHang", "MaGioHang");

            // Chuẩn bị dữ liệu cho admin: danh sách sản phẩm và dịch vụ (xử lý ở view)
            ViewData["SanPhams"] = new SelectList(_context.SanPhams.OrderBy(s => s.TenSanPham), "MaSanPham", "TenSanPham");
            ViewData["DichVus"] = new SelectList(_context.DichVus.OrderBy(d => d.TenDichVu), "MaDichVu", "TenDichVu");

            // Loại item: SP hoặc DV
            ViewData["LoaiItem"] = new SelectList(new[] { "SP", "DV" });

            return View();
        }

        // POST: Admins/GioHangChiTiets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCt,MaGioHang,LoaiItem,MaItem,SoLuong,DonGia,ThanhTien,GhiChu,NgayCapNhat")] GioHangChiTiet gioHangChiTiet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gioHangChiTiet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaGioHang"] = new SelectList(_context.GioHangs, "MaGioHang", "MaGioHang", gioHangChiTiet.MaGioHang);
            ViewData["SanPhams"] = new SelectList(_context.SanPhams.OrderBy(s => s.TenSanPham), "MaSanPham", "TenSanPham");
            ViewData["DichVus"] = new SelectList(_context.DichVus.OrderBy(d => d.TenDichVu), "MaDichVu", "TenDichVu");
            ViewData["LoaiItem"] = new SelectList(new[] { "SP", "DV" }, gioHangChiTiet.LoaiItem);
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
            ViewData["SanPhams"] = new SelectList(_context.SanPhams.OrderBy(s => s.TenSanPham), "MaSanPham", "TenSanPham");
            ViewData["DichVus"] = new SelectList(_context.DichVus.OrderBy(d => d.TenDichVu), "MaDichVu", "TenDichVu");
            ViewData["LoaiItem"] = new SelectList(new[] { "SP", "DV" }, gioHangChiTiet.LoaiItem);
            return View(gioHangChiTiet);
        }

        // POST: Admins/GioHangChiTiets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaCt,MaGioHang,LoaiItem,MaItem,SoLuong,DonGia,ThanhTien,GhiChu,NgayCapNhat")] GioHangChiTiet gioHangChiTiet)
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
            ViewData["SanPhams"] = new SelectList(_context.SanPhams.OrderBy(s => s.TenSanPham), "MaSanPham", "TenSanPham");
            ViewData["DichVus"] = new SelectList(_context.DichVus.OrderBy(d => d.TenDichVu), "MaDichVu", "TenDichVu");
            ViewData["LoaiItem"] = new SelectList(new[] { "SP", "DV" }, gioHangChiTiet.LoaiItem);
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
                .FirstOrDefaultAsync(m => m.MaCt == id);

            if (gioHangChiTiet == null)
            {
                return NotFound();
            }

            // Resolve item info theo LoaiItem + MaItem
            string itemName = null;
            string itemImage = null;
            string itemLoai = gioHangChiTiet.LoaiItem ?? "";

            if (itemLoai == "SP")
            {
                var sp = await _context.SanPhams.FirstOrDefaultAsync(s => s.MaSanPham == gioHangChiTiet.MaItem);
                itemName = sp?.TenSanPham;
                itemImage = sp?.HinhAnh;
            }
            else if (itemLoai == "DV")
            {
                var dv = await _context.DichVus.FirstOrDefaultAsync(d => d.MaDichVu == gioHangChiTiet.MaItem);
                itemName = dv?.TenDichVu;
                itemImage = dv?.HinhAnh;
            }

            ViewBag.ItemName = itemName ?? "(Không xác định)";
            ViewBag.ItemImage = itemImage;
            ViewBag.ItemLoai = itemLoai;

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
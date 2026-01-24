using System;
using System.Collections.Generic;

namespace _2210900059_PTQ_DATN.Models
{
    public partial class ChiTietDonHang
    {
        public int MaChiTiet { get; set; }


        public int MaDonHang { get; set; }


        public int ItemId { get; set; } // ID SP hoặc DV


        public string ItemType { get; set; } = null!;
        // "SanPham" | "DichVu"


        public int SoLuong { get; set; }


        public decimal DonGia { get; set; }


        public decimal ThanhTien { get; set; }


        public string? GhiChu { get; set; }


        public virtual DonHang MaDonHangNavigation { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;

namespace _2210900059_PTQ_DATN.Models;

public partial class GioHangChiTiet
{
    public int MaCt { get; set; }

    public int MaGioHang { get; set; }

    public int MaSanPham { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public decimal? ThanhTien { get; set; }

    public string? GhiChu { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public virtual GioHang MaGioHangNavigation { get; set; } = null!;

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}

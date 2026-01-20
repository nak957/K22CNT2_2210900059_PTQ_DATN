using System;
using System.Collections.Generic;

namespace _2210900059_PTQ_DATN.Models;

public partial class GioHangChiTiet
{
    public int MaCt { get; set; }

    public int MaGioHang { get; set; }

    // MỚI: loại item ('SP' hoặc 'DV')
    public string LoaiItem { get; set; } = null!;

    // MỚI: id của item, có thể là maSanPham hoặc maDichVu tùy LoaiItem
    public int MaItem { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    // ThanhTien giờ là cột tính toán (computed) trong DB
    public decimal? ThanhTien { get; set; }

    public string? GhiChu { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public virtual GioHang MaGioHangNavigation { get; set; } = null!;

    // Không còn navigation trực tiếp tới SanPham vì model polymorphic.
    // Nếu cần, bạn có thể thêm helper không-mapped hoặc nạp thủ công SanPham/DichVu tùy LoaiItem.
    
}

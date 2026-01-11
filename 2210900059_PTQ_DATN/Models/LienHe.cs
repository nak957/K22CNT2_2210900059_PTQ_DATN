using System;
using System.Collections.Generic;

namespace _2210900059_PTQ_DATN.Models;

public partial class LienHe
{
    public int MaLienHe { get; set; }

    public string HoTen { get; set; } = null!;

    public string? Email { get; set; }

    public string? SoDienThoai { get; set; }

    public string? TieuDe { get; set; }

    public string? LoaiLienHe { get; set; }

    public string NoiDung { get; set; } = null!;

    public bool? DaDoc { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public DateTime? NgayGui { get; set; }

    public int MaNguoiDung { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
}

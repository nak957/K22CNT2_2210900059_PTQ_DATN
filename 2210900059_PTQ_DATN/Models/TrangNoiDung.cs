using System;
using System.Collections.Generic;

namespace _2210900059_PTQ_DATN.Models;

public partial class TrangNoiDung
{
    public int MaTrang { get; set; }

    public string TieuDe { get; set; } = null!;

    public string? Slug { get; set; }

    public string? LoaiTrang { get; set; }

    public string? MoTaNgan { get; set; }

    public string? NoiDung { get; set; }

    public int? ThuTu { get; set; }

    public bool? HienThi { get; set; }

    public bool? NoiBat { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public int? MaNguoiTao { get; set; }

    public int? MaNguoiCapNhat { get; set; }

    public virtual NguoiDung? MaNguoiCapNhatNavigation { get; set; }

    public virtual NguoiDung? MaNguoiTaoNavigation { get; set; }
}

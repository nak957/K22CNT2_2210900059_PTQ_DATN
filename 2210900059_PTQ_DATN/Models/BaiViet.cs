using System;
using System.Collections.Generic;

namespace _2210900059_PTQ_DATN.Models;

public partial class BaiViet
{
    public int MaBaiViet { get; set; }

    public string TieuDe { get; set; } = null!;

    public string? Slug { get; set; }

    public string? HinhAnh { get; set; }

    public string? MoTa { get; set; }

    public string? NoiDung { get; set; }

    public string? Loai { get; set; }

    public bool? NoiBat { get; set; }

    public DateTime? NgayDang { get; set; }

    public int? MaNguoiTao { get; set; }

    public int? MaNguoiCapNhat { get; set; }

    public virtual NguoiDung? MaNguoiCapNhatNavigation { get; set; }

    public virtual NguoiDung? MaNguoiTaoNavigation { get; set; }
}

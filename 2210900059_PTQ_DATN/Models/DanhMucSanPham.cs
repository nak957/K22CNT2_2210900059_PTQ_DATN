using System;
using System.Collections.Generic;

namespace _2210900059_PTQ_DATN.Models;

public partial class DanhMucSanPham
{
    public int MaDanhMuc { get; set; }

    public string TenDanhMuc { get; set; } = null!;

    public string? Slug { get; set; }

    public string? MoTa { get; set; }

    public int? MaNguoiTao { get; set; }

    public int? MaNguoiCapNhat { get; set; }

    public virtual NguoiDung? MaNguoiCapNhatNavigation { get; set; }

    public virtual NguoiDung? MaNguoiTaoNavigation { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}

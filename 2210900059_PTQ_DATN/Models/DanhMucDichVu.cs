using System;
using System.Collections.Generic;

namespace _2210900059_PTQ_DATN.Models;

public partial class DanhMucDichVu
{
    public int MaDanhMucDv { get; set; }

    public string TenDanhMuc { get; set; } = null!;

    public string? Slug { get; set; }

    public string? MoTa { get; set; }

    public int? MaNguoiTao { get; set; }

    public int? MaNguoiCapNhat { get; set; }

    public virtual ICollection<DichVu> DichVus { get; set; } = new List<DichVu>();

    public virtual NguoiDung? MaNguoiCapNhatNavigation { get; set; }

    public virtual NguoiDung? MaNguoiTaoNavigation { get; set; }
}

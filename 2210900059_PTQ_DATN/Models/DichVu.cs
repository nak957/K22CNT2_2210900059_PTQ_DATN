using System;
using System.Collections.Generic;

namespace _2210900059_PTQ_DATN.Models;

public partial class DichVu
{
    public int MaDichVu { get; set; }

    public int? MaDanhMucDv { get; set; }

    public string TenDichVu { get; set; } = null!;

    public string? Slug { get; set; }

    public string? LoaiDichVu { get; set; }

    public string? CongNghe { get; set; }

    public int? ThoiLuong { get; set; }

    public int? SoBuoi { get; set; }

    public string? DoiTuongPhuHop { get; set; }

    public string? VungTacDong { get; set; }

    public string? MoTaNgan { get; set; }

    public string? NoiDungChiTiet { get; set; }

    public string? HinhAnh { get; set; }

    public decimal? Gia { get; set; }

    public bool? NoiBat { get; set; }

    public DateTime? NgayTao { get; set; }

    public bool? TrangThai { get; set; }

    public int? MaNguoiTao { get; set; }

    public int? MaNguoiCapNhat { get; set; }

    public virtual DanhMucDichVu? MaDanhMucDvNavigation { get; set; }

    public virtual NguoiDung? MaNguoiCapNhatNavigation { get; set; }

    public virtual NguoiDung? MaNguoiTaoNavigation { get; set; }
}

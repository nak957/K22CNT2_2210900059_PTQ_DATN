using System;
using System.Collections.Generic;

namespace _2210900059_PTQ_DATN.Models;

public partial class NguoiDung
{
    public int MaNguoiDung { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string? HoTen { get; set; }

    public string? Email { get; set; }

    public string VaiTro { get; set; } = null!;

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<BaiViet> BaiVietMaNguoiCapNhatNavigations { get; set; } = new List<BaiViet>();

    public virtual ICollection<BaiViet> BaiVietMaNguoiTaoNavigations { get; set; } = new List<BaiViet>();

    public virtual ICollection<DanhMucDichVu> DanhMucDichVuMaNguoiCapNhatNavigations { get; set; } = new List<DanhMucDichVu>();

    public virtual ICollection<DanhMucDichVu> DanhMucDichVuMaNguoiTaoNavigations { get; set; } = new List<DanhMucDichVu>();

    public virtual ICollection<DanhMucSanPham> DanhMucSanPhamMaNguoiCapNhatNavigations { get; set; } = new List<DanhMucSanPham>();

    public virtual ICollection<DanhMucSanPham> DanhMucSanPhamMaNguoiTaoNavigations { get; set; } = new List<DanhMucSanPham>();

    public virtual ICollection<DichVu> DichVuMaNguoiCapNhatNavigations { get; set; } = new List<DichVu>();

    public virtual ICollection<DichVu> DichVuMaNguoiTaoNavigations { get; set; } = new List<DichVu>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual ICollection<LienHe> LienHes { get; set; } = new List<LienHe>();

    public virtual ICollection<SanPham> SanPhamMaNguoiCapNhatNavigations { get; set; } = new List<SanPham>();

    public virtual ICollection<SanPham> SanPhamMaNguoiTaoNavigations { get; set; } = new List<SanPham>();

    public virtual ICollection<TrangNoiDung> TrangNoiDungMaNguoiCapNhatNavigations { get; set; } = new List<TrangNoiDung>();

    public virtual ICollection<TrangNoiDung> TrangNoiDungMaNguoiTaoNavigations { get; set; } = new List<TrangNoiDung>();
}

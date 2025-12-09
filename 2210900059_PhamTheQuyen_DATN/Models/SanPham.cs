using _221090005_PhamTheQuyen_DATN.Models;

public class SanPham
{
    public int MaSanPham { get; set; }
    public int? MaDanhMuc { get; set; }

    public string TenSanPham { get; set; }
    public string Slug { get; set; }
    public string MoTaNgan { get; set; }
    public string MoTaChiTiet { get; set; }
    public string HinhAnh { get; set; }
    public decimal Gia { get; set; }

    public DateTime? NgayTao { get; set; }
    public bool TrangThai { get; set; }

    public int? MaNguoiTao { get; set; }
    public int? MaNguoiCapNhat { get; set; }

    public DanhMucSanPham DanhMuc { get; set; }
    public NguoiDung NguoiTao { get; set; }
    public NguoiDung NguoiCapNhat { get; set; }

    public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    public ICollection<GioHangChiTiet> GioHangChiTiets { get; set; }
}

using _221090005_PhamTheQuyen_DATN.Models;

public class DichVu
{
    public int MaDichVu { get; set; }
    public int? MaDanhMucDV { get; set; }

    public string TenDichVu { get; set; }
    public string MoTaNgan { get; set; }
    public string NoiDungChiTiet { get; set; }
    public string HinhAnh { get; set; }
    public decimal? Gia { get; set; }
    public string Slug { get; set; }
    public DateTime? NgayTao { get; set; }
    public bool TrangThai { get; set; }

    public int? MaNguoiTao { get; set; }
    public int? MaNguoiCapNhat { get; set; }

    public DanhMucDichVu DanhMuc { get; set; }
    public NguoiDung NguoiTao { get; set; }
    public NguoiDung NguoiCapNhat { get; set; }
}

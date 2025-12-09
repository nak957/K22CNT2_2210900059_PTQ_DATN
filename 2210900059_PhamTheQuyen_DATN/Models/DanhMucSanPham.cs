using _221090005_PhamTheQuyen_DATN.Models;

public class DanhMucSanPham
{
    public int MaDanhMuc { get; set; }
    public string TenDanhMuc { get; set; }
    public string Slug { get; set; }
    public string MoTa { get; set; }

    public int? MaNguoiTao { get; set; }
    public int? MaNguoiCapNhat { get; set; }

    public NguoiDung NguoiTao { get; set; }
    public NguoiDung NguoiCapNhat { get; set; }

    public ICollection<SanPham> SanPhams { get; set; }
}

using _221090005_PhamTheQuyen_DATN.Models;

public class BaiViet
{
    public int MaBaiViet { get; set; }
    public string TieuDe { get; set; }
    public string Slug { get; set; }
    public string HinhAnh { get; set; }
    public string MoTa { get; set; }
    public string NoiDung { get; set; }
    public string Loai { get; set; }
    public DateTime? NgayDang { get; set; }

    public int? MaNguoiTao { get; set; }
    public int? MaNguoiCapNhat { get; set; }

    public NguoiDung NguoiTao { get; set; }
    public NguoiDung NguoiCapNhat { get; set; }
}

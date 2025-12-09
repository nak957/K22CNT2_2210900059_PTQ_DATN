using _221090005_PhamTheQuyen_DATN.Models;

public class TrangNoiDung
{
    public int MaTrang { get; set; }
    public string TieuDe { get; set; }
    public string Slug { get; set; }
    public string NoiDung { get; set; }
    public DateTime? NgayCapNhat { get; set; }

    public int? MaNguoiTao { get; set; }
    public int? MaNguoiCapNhat { get; set; }

    public NguoiDung NguoiTao { get; set; }
    public NguoiDung NguoiCapNhat { get; set; }
}

using _221090005_PhamTheQuyen_DATN.Models;

public class LienHe
{
    public int MaLienHe { get; set; }
    public string HoTen { get; set; }
    public string Email { get; set; }
    public string SoDienThoai { get; set; }
    public string NoiDung { get; set; }
    public DateTime? NgayGui { get; set; }

    public int MaNguoiDung { get; set; }
    public NguoiDung NguoiDung { get; set; }
}

using _221090005_PhamTheQuyen_DATN.Models;

public class GioHang
{
    public int MaGioHang { get; set; }
    public string SessionId { get; set; }
    public int? MaNguoiDung { get; set; }
    public string TrangThai { get; set; }
    public DateTime? NgayTao { get; set; }
    public DateTime? NgayCapNhat { get; set; }

    public NguoiDung NguoiDung { get; set; }
    public ICollection<GioHangChiTiet> GioHangChiTiets { get; set; }
}

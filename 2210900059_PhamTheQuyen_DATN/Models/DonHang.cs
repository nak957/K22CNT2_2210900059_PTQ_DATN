using _221090005_PhamTheQuyen_DATN.Models;

public class DonHang
{
    public int MaDonHang { get; set; }
    public string HoTen { get; set; }
    public string SoDienThoai { get; set; }
    public string Email { get; set; }
    public string DiaChi { get; set; }
    public decimal? TongTien { get; set; }
    public DateTime? NgayDat { get; set; }
    public string TrangThai { get; set; }

    public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
}

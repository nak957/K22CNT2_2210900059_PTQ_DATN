public class GioHangChiTiet
{
    public int MaCT { get; set; }
    public int MaGioHang { get; set; }
    public int MaSanPham { get; set; }

    public int SoLuong { get; set; }
    public decimal DonGia { get; set; }
    public DateTime? NgayCapNhat { get; set; }

    public GioHang GioHang { get; set; }
    public SanPham SanPham { get; set; }
}

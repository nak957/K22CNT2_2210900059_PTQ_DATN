namespace _2210900059_PTQ_DATN.ViewModels
{
    public class DonHangDichVuVM
    {
        public int MaDonHang { get; set; }
        public string? MaDonHangCode { get; set; }

        public string TenDichVu { get; set; } = string.Empty;
        public int SoLuong { get; set; }

        public DateTime NgayHen { get; set; }
        public string GioHen { get; set; } = "09:00 - 11:00";

        public string? TrangThai { get; set; }
    }
}

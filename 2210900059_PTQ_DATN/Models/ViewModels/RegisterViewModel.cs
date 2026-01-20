using System.ComponentModel.DataAnnotations;

namespace _2210900059_PTQ_DATN.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string TenDangNhap { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string MatKhau { get; set; } = null!;

        public string? HoTen { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        // ===== THÔNG TIN LIÊN HỆ =====
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string SoDienThoai { get; set; } = null!;

        public string? DiaChi { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace _2210900059_PTQ_DATN.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string TenDangNhap { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        public string MatKhau { get; set; } = null!;

        public string? HoTen { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }
    }
}

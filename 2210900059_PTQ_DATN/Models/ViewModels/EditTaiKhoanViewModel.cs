using System.ComponentModel.DataAnnotations;

namespace _2210900059_PTQ_DATN.Models.ViewModels
{
    public class EditTaiKhoanViewModel
    {
        public int MaNguoiDung { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string? HoTen { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        // ====== ĐỔI MẬT KHẨU ======

        [DataType(DataType.Password)]
        public string? MatKhauCu { get; set; }

        [DataType(DataType.Password)]
        public string? MatKhauMoi { get; set; }

        [DataType(DataType.Password)]
        [Compare("MatKhauMoi", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string? XacNhanMatKhauMoi { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _2210900059_PTQ_DATN.Models
{
    public partial class SanPham
    {
        public int MaSanPham { get; set; }

        public int? MaDanhMuc { get; set; }

        [Required]
        public string TenSanPham { get; set; } = null!;

        public string? Slug { get; set; }

        public string? LoaiSanPham { get; set; }

        public string? ThuongHieu { get; set; }

        public string? DungTich { get; set; }

        public string? DoiTuongSuDung { get; set; }

        public string? CongDungChinh { get; set; }

        public string? ThanhPhanNoiBat { get; set; }

        public string? MoTaNgan { get; set; }

        public string? MoTaChiTiet { get; set; }

        public string? HinhAnh { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Gia { get; set; }

        // 🔹 THÊM MỚI – số lượng tồn kho
        public int SoLuong { get; set; } = 0;

        public bool? NoiBat { get; set; }

        public DateTime? NgayTao { get; set; }

        public bool? TrangThai { get; set; }

        public int? MaNguoiTao { get; set; }

        public int? MaNguoiCapNhat { get; set; }
        
  

        public virtual DanhMucSanPham? MaDanhMucNavigation { get; set; }

        public virtual NguoiDung? MaNguoiCapNhatNavigation { get; set; }

        public virtual NguoiDung? MaNguoiTaoNavigation { get; set; }

       

        [NotMapped]
        public virtual ICollection<GioHangChiTiet> GioHangChiTiets { get; set; } = new List<GioHangChiTiet>();

        [NotMapped]
        public IFormFile? HinhAnhFile { get; set; }

        [NotMapped]
        public bool IsEditing { get; set; } = false;

        [NotMapped]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
    }
}

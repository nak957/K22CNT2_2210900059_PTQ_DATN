using System;
using System.Collections.Generic;

namespace _221090005_PhamTheQuyen_DATN.Models
{
    public class NguoiDung
    {
        public int MaNguoiDung { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string VaiTro { get; set; }
        public DateTime? NgayTao { get; set; }

        // Navigation
        public ICollection<DanhMucDichVu> DanhMucDichVus_Tao { get; set; }
        public ICollection<DanhMucDichVu> DanhMucDichVus_Sua { get; set; }

        public ICollection<DichVu> DichVus_Tao { get; set; }
        public ICollection<DichVu> DichVus_Sua { get; set; }

        public ICollection<DanhMucSanPham> DanhMucSanPhams_Tao { get; set; }
        public ICollection<DanhMucSanPham> DanhMucSanPhams_Sua { get; set; }

        public ICollection<SanPham> SanPhams_Tao { get; set; }
        public ICollection<SanPham> SanPhams_Sua { get; set; }

        public ICollection<BaiViet> BaiViets_Tao { get; set; }
        public ICollection<BaiViet> BaiViets_Sua { get; set; }

        public ICollection<TrangNoiDung> TrangNoiDungs_Tao { get; set; }
        public ICollection<TrangNoiDung> TrangNoiDungs_Sua { get; set; }

        public ICollection<LienHe> LienHes { get; set; }
        public ICollection<GioHang> GioHangs { get; set; }
    }
}

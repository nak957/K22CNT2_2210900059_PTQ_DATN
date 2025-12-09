using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace _221090005_PhamTheQuyen_DATN.Models
{
    public class LeSkinDbContext : DbContext
    {
        public LeSkinDbContext(DbContextOptions<LeSkinDbContext> options)
            : base(options)
        {
        }

        public DbSet<NguoiDung> NguoiDung { get; set; }

       
        public DbSet<DanhMucDichVu> DanhMucDichVu { get; set; }
        public DbSet<DichVu> DichVu { get; set; }

       
        public DbSet<DanhMucSanPham> DanhMucSanPham { get; set; }
        public DbSet<SanPham> SanPham { get; set; }

        // BÀI VIẾT
        public DbSet<BaiViet> BaiViet { get; set; }

        // TRANG NỘI DUNG
        public DbSet<TrangNoiDung> TrangNoiDung { get; set; }

        // LIÊN HỆ
        public DbSet<LienHe> LienHe { get; set; }

        // ĐƠN HÀNG
        public DbSet<DonHang> DonHang { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }

        // GIỎ HÀNG
        public DbSet<GioHang> GioHang { get; set; }
        public DbSet<GioHangChiTiet> GioHangChiTiet { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // KHÓA NGOẠI – NguoiDung
            modelBuilder.Entity<DanhMucDichVu>()
                .HasOne<NguoiDung>()
                .WithMany()
                .HasForeignKey(x => x.maNguoiTao)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DanhMucDichVu>()
               .HasOne<NguoiDung>()
               .WithMany()
               .HasForeignKey(x => x.maNguoiCapNhat)
               .OnDelete(DeleteBehavior.Restrict);

          
        }
    }
}

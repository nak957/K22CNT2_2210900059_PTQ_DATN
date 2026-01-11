using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _2210900059_PTQ_DATN.Models;

public partial class LeSkinDbContext : DbContext
{
    public LeSkinDbContext()
    {
    }

    public LeSkinDbContext(DbContextOptions<LeSkinDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaiViet> BaiViets { get; set; }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<DanhMucDichVu> DanhMucDichVus { get; set; }

    public virtual DbSet<DanhMucSanPham> DanhMucSanPhams { get; set; }

    public virtual DbSet<DichVu> DichVus { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<GioHangChiTiet> GioHangChiTiets { get; set; }

    public virtual DbSet<LienHe> LienHes { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<TrangNoiDung> TrangNoiDungs { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS01;Database=LeSkinDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaiViet>(entity =>
        {
            entity.HasKey(e => e.MaBaiViet).HasName("PK__BaiViet__51CC343A1056E446");

            entity.ToTable("BaiViet");

            entity.HasIndex(e => e.Slug, "UQ__BaiViet__32DD1E4C09757D22").IsUnique();

            entity.Property(e => e.MaBaiViet).HasColumnName("maBaiViet");
            entity.Property(e => e.HinhAnh)
                .HasMaxLength(255)
                .HasColumnName("hinhAnh");
            entity.Property(e => e.Loai)
                .HasMaxLength(50)
                .HasColumnName("loai");
            entity.Property(e => e.MaNguoiCapNhat).HasColumnName("maNguoiCapNhat");
            entity.Property(e => e.MaNguoiTao).HasColumnName("maNguoiTao");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.NgayDang)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayDang");
            entity.Property(e => e.NoiBat)
                .HasDefaultValue(false)
                .HasColumnName("noiBat");
            entity.Property(e => e.NoiDung).HasColumnName("noiDung");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.TieuDe)
                .HasMaxLength(255)
                .HasColumnName("tieuDe");

            entity.HasOne(d => d.MaNguoiCapNhatNavigation).WithMany(p => p.BaiVietMaNguoiCapNhatNavigations)
                .HasForeignKey(d => d.MaNguoiCapNhat)
                .HasConstraintName("FK_BaiViet_NguoiCapNhat");

            entity.HasOne(d => d.MaNguoiTaoNavigation).WithMany(p => p.BaiVietMaNguoiTaoNavigations)
                .HasForeignKey(d => d.MaNguoiTao)
                .HasConstraintName("FK_BaiViet_NguoiTao");
        });

        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.MaChiTiet).HasName("PK__ChiTietD__9996488862548E05");

            entity.ToTable("ChiTietDonHang");

            entity.Property(e => e.MaChiTiet).HasColumnName("maChiTiet");
            entity.Property(e => e.DonGia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("donGia");
            entity.Property(e => e.GhiChu).HasColumnName("ghiChu");
            entity.Property(e => e.MaDonHang).HasColumnName("maDonHang");
            entity.Property(e => e.MaSanPham).HasColumnName("maSanPham");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");
            entity.Property(e => e.ThanhTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("thanhTien");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDH_DonHang");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDH_SanPham");
        });

        modelBuilder.Entity<DanhMucDichVu>(entity =>
        {
            entity.HasKey(e => e.MaDanhMucDv).HasName("PK__DanhMucD__7F835E0F76FABE6B");

            entity.ToTable("DanhMucDichVu");

            entity.HasIndex(e => e.Slug, "UQ__DanhMucD__32DD1E4C7C55BBF3").IsUnique();

            entity.Property(e => e.MaDanhMucDv).HasColumnName("maDanhMucDV");
            entity.Property(e => e.MaNguoiCapNhat).HasColumnName("maNguoiCapNhat");
            entity.Property(e => e.MaNguoiTao).HasColumnName("maNguoiTao");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.Slug)
                .HasMaxLength(200)
                .HasColumnName("slug");
            entity.Property(e => e.TenDanhMuc)
                .HasMaxLength(200)
                .HasColumnName("tenDanhMuc");

            entity.HasOne(d => d.MaNguoiCapNhatNavigation).WithMany(p => p.DanhMucDichVuMaNguoiCapNhatNavigations)
                .HasForeignKey(d => d.MaNguoiCapNhat)
                .HasConstraintName("FK_DMDV_NguoiCapNhat");

            entity.HasOne(d => d.MaNguoiTaoNavigation).WithMany(p => p.DanhMucDichVuMaNguoiTaoNavigations)
                .HasForeignKey(d => d.MaNguoiTao)
                .HasConstraintName("FK_DMDV_NguoiTao");
        });

        modelBuilder.Entity<DanhMucSanPham>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PK__DanhMucS__6B0F914CADB58596");

            entity.ToTable("DanhMucSanPham");

            entity.HasIndex(e => e.Slug, "UQ__DanhMucS__32DD1E4CF8FDC5A7").IsUnique();

            entity.Property(e => e.MaDanhMuc).HasColumnName("maDanhMuc");
            entity.Property(e => e.MaNguoiCapNhat).HasColumnName("maNguoiCapNhat");
            entity.Property(e => e.MaNguoiTao).HasColumnName("maNguoiTao");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.Slug)
                .HasMaxLength(200)
                .HasColumnName("slug");
            entity.Property(e => e.TenDanhMuc)
                .HasMaxLength(200)
                .HasColumnName("tenDanhMuc");

            entity.HasOne(d => d.MaNguoiCapNhatNavigation).WithMany(p => p.DanhMucSanPhamMaNguoiCapNhatNavigations)
                .HasForeignKey(d => d.MaNguoiCapNhat)
                .HasConstraintName("FK_DMSP_NguoiCapNhat");

            entity.HasOne(d => d.MaNguoiTaoNavigation).WithMany(p => p.DanhMucSanPhamMaNguoiTaoNavigations)
                .HasForeignKey(d => d.MaNguoiTao)
                .HasConstraintName("FK_DMSP_NguoiTao");
        });

        modelBuilder.Entity<DichVu>(entity =>
        {
            entity.HasKey(e => e.MaDichVu).HasName("PK__DichVu__80F48B09BC46D10F");

            entity.ToTable("DichVu");

            entity.HasIndex(e => e.Slug, "UQ__DichVu__32DD1E4CD4EBDC2F").IsUnique();

            entity.Property(e => e.MaDichVu).HasColumnName("maDichVu");
            entity.Property(e => e.CongNghe)
                .HasMaxLength(255)
                .HasColumnName("congNghe");
            entity.Property(e => e.DoiTuongPhuHop)
                .HasMaxLength(255)
                .HasColumnName("doiTuongPhuHop");
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("gia");
            entity.Property(e => e.HinhAnh)
                .HasMaxLength(255)
                .HasColumnName("hinhAnh");
            entity.Property(e => e.LoaiDichVu)
                .HasMaxLength(50)
                .HasColumnName("loaiDichVu");
            entity.Property(e => e.MaDanhMucDv).HasColumnName("maDanhMucDV");
            entity.Property(e => e.MaNguoiCapNhat).HasColumnName("maNguoiCapNhat");
            entity.Property(e => e.MaNguoiTao).HasColumnName("maNguoiTao");
            entity.Property(e => e.MoTaNgan).HasColumnName("moTaNgan");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayTao");
            entity.Property(e => e.NoiBat)
                .HasDefaultValue(false)
                .HasColumnName("noiBat");
            entity.Property(e => e.NoiDungChiTiet).HasColumnName("noiDungChiTiet");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.SoBuoi).HasColumnName("soBuoi");
            entity.Property(e => e.TenDichVu)
                .HasMaxLength(255)
                .HasColumnName("tenDichVu");
            entity.Property(e => e.ThoiLuong).HasColumnName("thoiLuong");
            entity.Property(e => e.TrangThai)
                .HasDefaultValue(true)
                .HasColumnName("trangThai");
            entity.Property(e => e.VungTacDong)
                .HasMaxLength(255)
                .HasColumnName("vungTacDong");

            entity.HasOne(d => d.MaDanhMucDvNavigation).WithMany(p => p.DichVus)
                .HasForeignKey(d => d.MaDanhMucDv)
                .HasConstraintName("FK_DichVu_DanhMuc");

            entity.HasOne(d => d.MaNguoiCapNhatNavigation).WithMany(p => p.DichVuMaNguoiCapNhatNavigations)
                .HasForeignKey(d => d.MaNguoiCapNhat)
                .HasConstraintName("FK_DichVu_NguoiCapNhat");

            entity.HasOne(d => d.MaNguoiTaoNavigation).WithMany(p => p.DichVuMaNguoiTaoNavigations)
                .HasForeignKey(d => d.MaNguoiTao)
                .HasConstraintName("FK_DichVu_NguoiTao");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DonHang__871D38191D669091");

            entity.ToTable("DonHang");

            entity.HasIndex(e => e.MaDonHangCode, "UQ__DonHang__5B9062AF35021E75").IsUnique();

            entity.Property(e => e.MaDonHang).HasColumnName("maDonHang");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(255)
                .HasColumnName("diaChi");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.GhiChu).HasColumnName("ghiChu");
            entity.Property(e => e.HoTen)
                .HasMaxLength(100)
                .HasColumnName("hoTen");
            entity.Property(e => e.MaDonHangCode)
                .HasMaxLength(50)
                .HasColumnName("maDonHangCode");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayCapNhat");
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayDat");
            entity.Property(e => e.PhiVanChuyen)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("phiVanChuyen");
            entity.Property(e => e.PhuongThucThanhToan)
                .HasMaxLength(50)
                .HasColumnName("phuongThucThanhToan");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .HasColumnName("soDienThoai");
            entity.Property(e => e.TongTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tongTien");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Chờ xử lý")
                .HasColumnName("trangThai");
            entity.Property(e => e.TrangThaiThanhToan)
                .HasMaxLength(50)
                .HasDefaultValue("Chưa thanh toán")
                .HasColumnName("trangThaiThanhToan");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK_DonHang_NguoiDung");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGioHang).HasName("PK__GioHang__2C76D2032EE6A1EB");

            entity.ToTable("GioHang");

            entity.Property(e => e.MaGioHang).HasColumnName("maGioHang");
            entity.Property(e => e.GhiChu).HasColumnName("ghiChu");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayCapNhat");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayTao");
            entity.Property(e => e.SessionId)
                .HasMaxLength(100)
                .HasColumnName("sessionId");
            entity.Property(e => e.TongTien)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tongTien");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValue("Đang sử dụng")
                .HasColumnName("trangThai");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK_GioHang_NguoiDung");
        });

        modelBuilder.Entity<GioHangChiTiet>(entity =>
        {
            entity.HasKey(e => e.MaCt).HasName("PK__GioHangC__7A3E0CF2E254C8F8");

            entity.ToTable("GioHangChiTiet");

            entity.Property(e => e.MaCt).HasColumnName("maCT");
            entity.Property(e => e.DonGia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("donGia");
            entity.Property(e => e.GhiChu).HasColumnName("ghiChu");
            entity.Property(e => e.MaGioHang).HasColumnName("maGioHang");
            entity.Property(e => e.MaSanPham).HasColumnName("maSanPham");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayCapNhat");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");
            entity.Property(e => e.ThanhTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("thanhTien");

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.GioHangChiTiets)
                .HasForeignKey(d => d.MaGioHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GHCT_GioHang");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.GioHangChiTiets)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GHCT_SanPham");
        });

        modelBuilder.Entity<LienHe>(entity =>
        {
            entity.HasKey(e => e.MaLienHe).HasName("PK__LienHe__C18B7066315E2B76");

            entity.ToTable("LienHe");

            entity.Property(e => e.MaLienHe).HasColumnName("maLienHe");
            entity.Property(e => e.DaDoc)
                .HasDefaultValue(false)
                .HasColumnName("daDoc");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.GhiChu).HasColumnName("ghiChu");
            entity.Property(e => e.HoTen)
                .HasMaxLength(100)
                .HasColumnName("hoTen");
            entity.Property(e => e.LoaiLienHe)
                .HasMaxLength(50)
                .HasColumnName("loaiLienHe");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.NgayGui)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayGui");
            entity.Property(e => e.NoiDung).HasColumnName("noiDung");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .HasColumnName("soDienThoai");
            entity.Property(e => e.TieuDe)
                .HasMaxLength(255)
                .HasColumnName("tieuDe");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Chưa xử lý")
                .HasColumnName("trangThai");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.LienHes)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LienHe_NguoiDung");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__446439EA9A50FEB7");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.TenDangNhap, "UQ__NguoiDun__59267D4A5CDBFAEB").IsUnique();

            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.HoTen)
                .HasMaxLength(100)
                .HasColumnName("hoTen");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .HasColumnName("matKhau");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayTao");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .HasColumnName("tenDangNhap");
            entity.Property(e => e.VaiTro)
                .HasMaxLength(20)
                .HasColumnName("vaiTro");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__SanPham__5B439C4352601915");

            entity.ToTable("SanPham");

            entity.HasIndex(e => e.Slug, "UQ__SanPham__32DD1E4C2AE52371").IsUnique();

            entity.Property(e => e.MaSanPham).HasColumnName("maSanPham");
            entity.Property(e => e.CongDungChinh)
                .HasMaxLength(255)
                .HasColumnName("congDungChinh");
            entity.Property(e => e.DoiTuongSuDung)
                .HasMaxLength(255)
                .HasColumnName("doiTuongSuDung");
            entity.Property(e => e.DungTich)
                .HasMaxLength(50)
                .HasColumnName("dungTich");
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("gia");
            entity.Property(e => e.HinhAnh)
                .HasMaxLength(255)
                .HasColumnName("hinhAnh");
            entity.Property(e => e.LoaiSanPham)
                .HasMaxLength(100)
                .HasColumnName("loaiSanPham");
            entity.Property(e => e.MaDanhMuc).HasColumnName("maDanhMuc");
            entity.Property(e => e.MaNguoiCapNhat).HasColumnName("maNguoiCapNhat");
            entity.Property(e => e.MaNguoiTao).HasColumnName("maNguoiTao");
            entity.Property(e => e.MoTaChiTiet).HasColumnName("moTaChiTiet");
            entity.Property(e => e.MoTaNgan).HasColumnName("moTaNgan");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayTao");
            entity.Property(e => e.NoiBat)
                .HasDefaultValue(false)
                .HasColumnName("noiBat");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.TenSanPham)
                .HasMaxLength(255)
                .HasColumnName("tenSanPham");
            entity.Property(e => e.ThanhPhanNoiBat)
                .HasMaxLength(255)
                .HasColumnName("thanhPhanNoiBat");
            entity.Property(e => e.ThuongHieu)
                .HasMaxLength(255)
                .HasColumnName("thuongHieu");
            entity.Property(e => e.TrangThai)
                .HasDefaultValue(true)
                .HasColumnName("trangThai");

            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaDanhMuc)
                .HasConstraintName("FK_SanPham_DanhMuc");

            entity.HasOne(d => d.MaNguoiCapNhatNavigation).WithMany(p => p.SanPhamMaNguoiCapNhatNavigations)
                .HasForeignKey(d => d.MaNguoiCapNhat)
                .HasConstraintName("FK_SanPham_NguoiCapNhat");

            entity.HasOne(d => d.MaNguoiTaoNavigation).WithMany(p => p.SanPhamMaNguoiTaoNavigations)
                .HasForeignKey(d => d.MaNguoiTao)
                .HasConstraintName("FK_SanPham_NguoiTao");
        });

        modelBuilder.Entity<TrangNoiDung>(entity =>
        {
            entity.HasKey(e => e.MaTrang).HasName("PK__TrangNoi__776A19B9DAC7CA58");

            entity.ToTable("TrangNoiDung");

            entity.HasIndex(e => e.Slug, "UQ__TrangNoi__32DD1E4CDC744992").IsUnique();

            entity.Property(e => e.MaTrang).HasColumnName("maTrang");
            entity.Property(e => e.HienThi)
                .HasDefaultValue(true)
                .HasColumnName("hienThi");
            entity.Property(e => e.LoaiTrang)
                .HasMaxLength(50)
                .HasColumnName("loaiTrang");
            entity.Property(e => e.MaNguoiCapNhat).HasColumnName("maNguoiCapNhat");
            entity.Property(e => e.MaNguoiTao).HasColumnName("maNguoiTao");
            entity.Property(e => e.MoTaNgan).HasColumnName("moTaNgan");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngayCapNhat");
            entity.Property(e => e.NoiBat)
                .HasDefaultValue(false)
                .HasColumnName("noiBat");
            entity.Property(e => e.NoiDung).HasColumnName("noiDung");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.ThuTu)
                .HasDefaultValue(0)
                .HasColumnName("thuTu");
            entity.Property(e => e.TieuDe)
                .HasMaxLength(255)
                .HasColumnName("tieuDe");

            entity.HasOne(d => d.MaNguoiCapNhatNavigation).WithMany(p => p.TrangNoiDungMaNguoiCapNhatNavigations)
                .HasForeignKey(d => d.MaNguoiCapNhat)
                .HasConstraintName("FK_Trang_NguoiCapNhat");

            entity.HasOne(d => d.MaNguoiTaoNavigation).WithMany(p => p.TrangNoiDungMaNguoiTaoNavigations)
                .HasForeignKey(d => d.MaNguoiTao)
                .HasConstraintName("FK_Trang_NguoiTao");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

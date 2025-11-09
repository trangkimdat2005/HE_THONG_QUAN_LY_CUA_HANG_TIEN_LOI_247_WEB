using System;
using System.Collections.Generic;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.EF;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaoCao> BaoCaos { get; set; }

    public virtual DbSet<BaoCaoBanChay> BaoCaoBanChays { get; set; }

    public virtual DbSet<BaoCaoDoanhThu> BaoCaoDoanhThus { get; set; }

    public virtual DbSet<BaoCaoTonKho> BaoCaoTonKhos { get; set; }

    public virtual DbSet<Barcode> Barcodes { get; set; }

    public virtual DbSet<CaLamViec> CaLamViecs { get; set; }

    public virtual DbSet<ChamCong> ChamCongs { get; set; }

    public virtual DbSet<ChiTietDonOnline> ChiTietDonOnlines { get; set; }

    public virtual DbSet<ChiTietGiaoDichNcc> ChiTietGiaoDichNccs { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<ChiTietHoaDonKhuyenMai> ChiTietHoaDonKhuyenMais { get; set; }

    public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

    public virtual DbSet<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; }

    public virtual DbSet<ChinhSachHoanTra> ChinhSachHoanTras { get; set; }

    public virtual DbSet<ChinhSachHoanTraDanhMuc> ChinhSachHoanTraDanhMucs { get; set; }

    public virtual DbSet<ChuongTrinhKhuyenMai> ChuongTrinhKhuyenMais { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<DieuKienApDung> DieuKienApDungs { get; set; }

    public virtual DbSet<DieuKienApDungDanhMuc> DieuKienApDungDanhMucs { get; set; }

    public virtual DbSet<DieuKienApDungSanPham> DieuKienApDungSanPhams { get; set; }

    public virtual DbSet<DieuKienApDungToanBo> DieuKienApDungToanBos { get; set; }

    public virtual DbSet<DonGiaoHang> DonGiaoHangs { get; set; }

    public virtual DbSet<DonHangOnline> DonHangOnlines { get; set; }

    public virtual DbSet<DonViDoLuong> DonViDoLuongs { get; set; }

    public virtual DbSet<GiaoDichThanhToan> GiaoDichThanhToans { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<HinhAnh> HinhAnhs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KenhThanhToan> KenhThanhToans { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KiemKe> KiemKes { get; set; }

    public virtual DbSet<LichSuGiaBan> LichSuGiaBans { get; set; }

    public virtual DbSet<LichSuGiaoDich> LichSuGiaoDiches { get; set; }

    public virtual DbSet<LichSuMuaHang> LichSuMuaHangs { get; set; }

    public virtual DbSet<MaDinhDanhSanPham> MaDinhDanhSanPhams { get; set; }

    public virtual DbSet<MaKhuyenMai> MaKhuyenMais { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanHieu> NhanHieus { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<NhatKyHoatDong> NhatKyHoatDongs { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PhanCongCaLamViec> PhanCongCaLamViecs { get; set; }

    public virtual DbSet<PhiVanChuyen> PhiVanChuyens { get; set; }

    public virtual DbSet<PhieuDoiTra> PhieuDoiTras { get; set; }

    public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; }

    public virtual DbSet<PhieuXuat> PhieuXuats { get; set; }

    public virtual DbSet<Qrcode> Qrcodes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<SanPhamDanhMuc> SanPhamDanhMucs { get; set; }

    public virtual DbSet<SanPhamDonVi> SanPhamDonVis { get; set; }

    public virtual DbSet<SanPhamViTri> SanPhamViTris { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TaiKhoanKhachHang> TaiKhoanKhachHangs { get; set; }

    public virtual DbSet<TaiKhoanNhanVien> TaiKhoanNhanViens { get; set; }

    public virtual DbSet<TemNhan> TemNhans { get; set; }

    public virtual DbSet<TheThanhVien> TheThanhViens { get; set; }

    public virtual DbSet<TonKho> TonKhos { get; set; }

    public virtual DbSet<TrangThaiGiaoHang> TrangThaiGiaoHangs { get; set; }

    public virtual DbSet<TrangThaiXuLy> TrangThaiXuLies { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<ViTri> ViTris { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=QuanLyCuaHangTienLoi;User Id=sa;Password=Password123!;TrustServerCertificate=True;MultipleActiveResultSets=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaoCao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BaoCao__3213E83FE22F5722");

            entity.ToTable("BaoCao", "core");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.DenNgay).HasColumnName("denNgay");
            entity.Property(e => e.FileBaoCao)
                .HasMaxLength(500)
                .HasColumnName("fileBaoCao");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.LoaiBaoCao)
                .HasMaxLength(20)
                .HasColumnName("loaiBaoCao");
            entity.Property(e => e.NgayLap).HasColumnName("ngayLap");
            entity.Property(e => e.TuNgay).HasColumnName("tuNgay");
        });

        modelBuilder.Entity<BaoCaoBanChay>(entity =>
        {
            entity.HasKey(e => new { e.BaoCaoId, e.SanPhamId }).HasName("PK_BCBC");

            entity.ToTable("BaoCaoBanChay", "core");

            entity.HasIndex(e => e.SanPhamId, "IX_BaoCaoBanChay_sanPhamId");

            entity.HasIndex(e => e.Id, "UQ__BaoCaoBa__3213E83E93C476A5")
                .IsUnique()
                .HasFilter("([id] IS NOT NULL)");

            entity.Property(e => e.BaoCaoId)
                .HasMaxLength(50)
                .HasColumnName("baoCaoId");
            entity.Property(e => e.SanPhamId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamId");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.SoLuongBan).HasColumnName("soLuongBan");

            entity.HasOne(d => d.BaoCao).WithMany(p => p.BaoCaoBanChays)
                .HasForeignKey(d => d.BaoCaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BCBC_BC");

            entity.HasOne(d => d.SanPham).WithMany(p => p.BaoCaoBanChays)
                .HasForeignKey(d => d.SanPhamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BCBC_SP");
        });

        modelBuilder.Entity<BaoCaoDoanhThu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BaoCaoDo__3213E83F0D878F66");

            entity.ToTable("BaoCaoDoanhThu", "core");

            entity.HasIndex(e => e.BaoCaoId, "IX_BaoCaoDoanhThu_baoCaoId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.BaoCaoId)
                .HasMaxLength(50)
                .HasColumnName("baoCaoId");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.KyBaoCao)
                .HasMaxLength(100)
                .HasColumnName("kyBaoCao");
            entity.Property(e => e.TongDoanhThu)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tongDoanhThu");

            entity.HasOne(d => d.BaoCao).WithMany(p => p.BaoCaoDoanhThus)
                .HasForeignKey(d => d.BaoCaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BCDT_BC");
        });

        modelBuilder.Entity<BaoCaoTonKho>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BaoCaoTo__3213E83F284EB6E8");

            entity.ToTable("BaoCaoTonKho", "core");

            entity.HasIndex(e => e.BaoCaoId, "IX_BaoCaoTonKho_baoCaoId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.BaoCaoId)
                .HasMaxLength(50)
                .HasColumnName("baoCaoId");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.NhapTrongKy).HasColumnName("nhapTrongKy");
            entity.Property(e => e.SanPhamDonViId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamDonViId");
            entity.Property(e => e.TonCuoiKy).HasColumnName("tonCuoiKy");
            entity.Property(e => e.TonDauKy).HasColumnName("tonDauKy");
            entity.Property(e => e.XuatTrongKy).HasColumnName("xuatTrongKy");

            entity.HasOne(d => d.BaoCao).WithMany(p => p.BaoCaoTonKhos)
                .HasForeignKey(d => d.BaoCaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BCTK_BC");

            entity.HasOne(d => d.SanPhamDonVi).WithMany(p => p.BaoCaoTonKhos)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.SanPhamDonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BCTK_SPDV");
        });

        modelBuilder.Entity<Barcode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Barcode__3213E83F1D9006F9");

            entity.ToTable("Barcode", "core");

            entity.HasIndex(e => e.MaDinhDanhId, "IX_Barcode_maDinhDanhId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.BarcodeImage)
                .HasMaxLength(500)
                .HasColumnName("barcodeImage");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.MaDinhDanhId)
                .HasMaxLength(50)
                .HasColumnName("maDinhDanhId");

            entity.HasOne(d => d.MaDinhDanh).WithMany(p => p.Barcodes)
                .HasForeignKey(d => d.MaDinhDanhId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bar_MDD");
        });

        modelBuilder.Entity<CaLamViec>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CaLamVie__3213E83FCF27A89E");

            entity.ToTable("CaLamViec", "core");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.TenCa)
                .HasMaxLength(100)
                .HasColumnName("tenCa");
            entity.Property(e => e.ThoiGianBatDau)
                .HasPrecision(0)
                .HasColumnName("thoiGianBatDau");
            entity.Property(e => e.ThoiGianKetThuc)
                .HasPrecision(0)
                .HasColumnName("thoiGianKetThuc");
        });

        modelBuilder.Entity<ChamCong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChamCong__3213E83FB805717D");

            entity.ToTable("ChamCong", "core");

            entity.HasIndex(e => e.NhanVienId, "IX_ChamCong_nhanVienId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(500)
                .HasColumnName("ghiChu");
            entity.Property(e => e.GioRa)
                .HasPrecision(0)
                .HasColumnName("gioRa");
            entity.Property(e => e.GioVao)
                .HasPrecision(0)
                .HasColumnName("gioVao");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.Ngay).HasColumnName("ngay");
            entity.Property(e => e.NhanVienId)
                .HasMaxLength(50)
                .HasColumnName("nhanVienId");

            entity.HasOne(d => d.NhanVien).WithMany(p => p.ChamCongs)
                .HasForeignKey(d => d.NhanVienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChamCong_NV");
        });

        modelBuilder.Entity<ChiTietDonOnline>(entity =>
        {
            entity.HasKey(e => new { e.DonHangId, e.SanPhamDonViId }).HasName("PK_CTDON");

            entity.ToTable("ChiTietDonOnline", "core");

            entity.HasIndex(e => e.SanPhamDonViId, "IX_ChiTietDonOnline_sanPhamDonViId");

            entity.HasIndex(e => e.Id, "UQ__ChiTietD__3213E83EC2F0D741")
                .IsUnique()
                .HasFilter("([id] IS NOT NULL)");

            entity.Property(e => e.DonHangId)
                .HasMaxLength(50)
                .HasColumnName("donHangId");
            entity.Property(e => e.SanPhamDonViId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamDonViId");
            entity.Property(e => e.DonGia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("donGia");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");

            entity.HasOne(d => d.DonHang).WithMany(p => p.ChiTietDonOnlines)
                .HasForeignKey(d => d.DonHangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDON_DH");

            entity.HasOne(d => d.SanPhamDonVi).WithMany(p => p.ChiTietDonOnlines)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.SanPhamDonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDON_SPDV");
        });

        modelBuilder.Entity<ChiTietGiaoDichNcc>(entity =>
        {
            entity.HasKey(e => new { e.GiaoDichId, e.SanPhamDonViId }).HasName("PK_CTGD_NCC");

            entity.ToTable("ChiTietGiaoDichNCC", "core");

            entity.HasIndex(e => e.SanPhamDonViId, "IX_ChiTietGiaoDichNCC_sanPhamDonViId");

            entity.Property(e => e.GiaoDichId)
                .HasMaxLength(50)
                .HasColumnName("giaoDichId");
            entity.Property(e => e.SanPhamDonViId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamDonViId");
            entity.Property(e => e.DonGia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("donGia");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");
            entity.Property(e => e.ThanhTien)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("thanhTien");

            entity.HasOne(d => d.GiaoDich).WithMany(p => p.ChiTietGiaoDichNccs)
                .HasForeignKey(d => d.GiaoDichId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTGD_LSGD");

            entity.HasOne(d => d.SanPhamDonVi).WithMany(p => p.ChiTietGiaoDichNccs)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.SanPhamDonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTGD_SPDV");
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => new { e.HoaDonId, e.SanPhamDonViId });

            entity.ToTable("ChiTietHoaDon", "core");

            entity.HasIndex(e => e.HoaDonId, "IX_CTHD_HD");

            entity.HasIndex(e => e.SanPhamDonViId, "IX_ChiTietHoaDon_sanPhamDonViId");

            entity.Property(e => e.HoaDonId)
                .HasMaxLength(50)
                .HasColumnName("hoaDonId");
            entity.Property(e => e.SanPhamDonViId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamDonViId");
            entity.Property(e => e.DonGia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("donGia");
            entity.Property(e => e.GiamGia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("giamGia");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");

            entity.HasOne(d => d.HoaDon).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.HoaDonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHD_HoaDon");

            entity.HasOne(d => d.SanPhamDonVi).WithMany(p => p.ChiTietHoaDons)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.SanPhamDonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHD_SPDV");
        });

        modelBuilder.Entity<ChiTietHoaDonKhuyenMai>(entity =>
        {
            entity.HasKey(e => new { e.HoaDonId, e.SanPhamDonViId, e.MaKhuyenMaiId }).HasName("PK_CTHDKM");

            entity.ToTable("ChiTietHoaDonKhuyenMai", "core");

            entity.HasIndex(e => e.MaKhuyenMaiId, "IX_ChiTietHoaDonKhuyenMai_maKhuyenMaiId");

            entity.HasIndex(e => e.SanPhamDonViId, "IX_ChiTietHoaDonKhuyenMai_sanPhamDonViId");

            entity.HasIndex(e => e.Id, "UQ__ChiTietH__3213E83E64CDB278")
                .IsUnique()
                .HasFilter("([id] IS NOT NULL)");

            entity.Property(e => e.HoaDonId)
                .HasMaxLength(50)
                .HasColumnName("hoaDonId");
            entity.Property(e => e.SanPhamDonViId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamDonViId");
            entity.Property(e => e.MaKhuyenMaiId)
                .HasMaxLength(50)
                .HasColumnName("maKhuyenMaiId");
            entity.Property(e => e.GiaTriApDung)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("giaTriApDung");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");

            entity.HasOne(d => d.HoaDon).WithMany(p => p.ChiTietHoaDonKhuyenMais)
                .HasForeignKey(d => d.HoaDonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHDKM_HD");

            entity.HasOne(d => d.MaKhuyenMai).WithMany(p => p.ChiTietHoaDonKhuyenMais)
                .HasForeignKey(d => d.MaKhuyenMaiId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHDKM_MKM");

            entity.HasOne(d => d.SanPhamDonVi).WithMany(p => p.ChiTietHoaDonKhuyenMais)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.SanPhamDonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHDKM_SPDV");
        });

        modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
        {
            entity.HasKey(e => new { e.PhieuNhapId, e.SanPhamDonViId }).HasName("PK_CTPN");

            entity.ToTable("ChiTietPhieuNhap", "core");

            entity.HasIndex(e => e.SanPhamDonViId, "IX_ChiTietPhieuNhap_sanPhamDonViId");

            entity.Property(e => e.PhieuNhapId)
                .HasMaxLength(50)
                .HasColumnName("phieuNhapId");
            entity.Property(e => e.SanPhamDonViId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamDonViId");
            entity.Property(e => e.DonGia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("donGia");
            entity.Property(e => e.HanSuDung).HasColumnName("hanSuDung");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");

            entity.HasOne(d => d.PhieuNhap).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.PhieuNhapId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTPN_PN");

            entity.HasOne(d => d.SanPhamDonVi).WithMany(p => p.ChiTietPhieuNhaps)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.SanPhamDonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTPN_SPDV");
        });

        modelBuilder.Entity<ChiTietPhieuXuat>(entity =>
        {
            entity.HasKey(e => new { e.PhieuXuatId, e.SanPhamDonViId }).HasName("PK_CTPX");

            entity.ToTable("ChiTietPhieuXuat", "core");

            entity.HasIndex(e => e.SanPhamDonViId, "IX_ChiTietPhieuXuat_sanPhamDonViId");

            entity.Property(e => e.PhieuXuatId)
                .HasMaxLength(50)
                .HasColumnName("phieuXuatId");
            entity.Property(e => e.SanPhamDonViId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamDonViId");
            entity.Property(e => e.DonGia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("donGia");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");

            entity.HasOne(d => d.PhieuXuat).WithMany(p => p.ChiTietPhieuXuats)
                .HasForeignKey(d => d.PhieuXuatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTPX_PX");

            entity.HasOne(d => d.SanPhamDonVi).WithMany(p => p.ChiTietPhieuXuats)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.SanPhamDonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTPX_SPDV");
        });

        modelBuilder.Entity<ChinhSachHoanTra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChinhSac__3213E83FD509BE56");

            entity.ToTable("ChinhSachHoanTra", "core");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.ApDungDenNgay).HasColumnName("apDungDenNgay");
            entity.Property(e => e.ApDungToanBo)
                .HasDefaultValue(false)
                .HasColumnName("apDungToanBo");
            entity.Property(e => e.ApDungTuNgay).HasColumnName("apDungTuNgay");
            entity.Property(e => e.DieuKien)
                .HasMaxLength(500)
                .HasColumnName("dieuKien");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.TenChinhSach)
                .HasMaxLength(200)
                .HasColumnName("tenChinhSach");
            entity.Property(e => e.ThoiHan).HasColumnName("thoiHan");
        });

        modelBuilder.Entity<ChinhSachHoanTraDanhMuc>(entity =>
        {
            entity.HasKey(e => new { e.ChinhSachId, e.DanhMucId }).HasName("PK_CSHTDM");

            entity.ToTable("ChinhSachHoanTra_DanhMuc", "core");

            entity.HasIndex(e => e.DanhMucId, "IX_ChinhSachHoanTra_DanhMuc_danhMucId");

            entity.HasIndex(e => e.Id, "UQ__ChinhSac__3213E83ED81B46B2")
                .IsUnique()
                .HasFilter("([id] IS NOT NULL)");

            entity.Property(e => e.ChinhSachId)
                .HasMaxLength(50)
                .HasColumnName("chinhSachId");
            entity.Property(e => e.DanhMucId)
                .HasMaxLength(50)
                .HasColumnName("danhMucId");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");

            entity.HasOne(d => d.ChinhSach).WithMany(p => p.ChinhSachHoanTraDanhMucs)
                .HasForeignKey(d => d.ChinhSachId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CSHTDM_CS");

            entity.HasOne(d => d.DanhMuc).WithMany(p => p.ChinhSachHoanTraDanhMucs)
                .HasForeignKey(d => d.DanhMucId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CSHTDM_DM");
        });

        modelBuilder.Entity<ChuongTrinhKhuyenMai>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChuongTr__3213E83F918CF90D");

            entity.ToTable("ChuongTrinhKhuyenMai", "core");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.Loai)
                .HasMaxLength(30)
                .HasColumnName("loai");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.NgayBatDau).HasColumnName("ngayBatDau");
            entity.Property(e => e.NgayKetThuc).HasColumnName("ngayKetThuc");
            entity.Property(e => e.Ten)
                .HasMaxLength(255)
                .HasColumnName("ten");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DanhMuc__3213E83FECBD45A0");

            entity.ToTable("DanhMuc", "core");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.Ten)
                .HasMaxLength(200)
                .HasColumnName("ten");
        });

        modelBuilder.Entity<DieuKienApDung>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DieuKien__3213E83F9398724C");

            entity.ToTable("DieuKienApDung", "core");

            entity.HasIndex(e => e.ChuongTrinhId, "IX_DieuKienApDung_chuongTrinhId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.ChuongTrinhId)
                .HasMaxLength(50)
                .HasColumnName("chuongTrinhId");
            entity.Property(e => e.DieuKien)
                .HasMaxLength(500)
                .HasColumnName("dieuKien");
            entity.Property(e => e.GiaTriToiDa)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("giaTriToiDa");
            entity.Property(e => e.GiaTriToiThieu)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("giaTriToiThieu");
            entity.Property(e => e.GiamTheo)
                .HasMaxLength(20)
                .HasColumnName("giamTheo");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");

            entity.HasOne(d => d.ChuongTrinh).WithMany(p => p.DieuKienApDungs)
                .HasForeignKey(d => d.ChuongTrinhId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DKAD_CTKM");
        });

        modelBuilder.Entity<DieuKienApDungDanhMuc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DieuKien__3213E83F8C100A0B");

            entity.ToTable("DieuKienApDungDanhMuc", "core");

            entity.HasIndex(e => e.DanhMucId, "IX_DieuKienApDungDanhMuc_danhMucId");

            entity.HasIndex(e => e.DieuKienId, "IX_DieuKienApDungDanhMuc_dieuKienId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.DanhMucId)
                .HasMaxLength(50)
                .HasColumnName("danhMucId");
            entity.Property(e => e.DieuKienId)
                .HasMaxLength(50)
                .HasColumnName("dieuKienId");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");

            entity.HasOne(d => d.DanhMuc).WithMany(p => p.DieuKienApDungDanhMucs)
                .HasForeignKey(d => d.DanhMucId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DKADDM_DM");

            entity.HasOne(d => d.DieuKien).WithMany(p => p.DieuKienApDungDanhMucs)
                .HasForeignKey(d => d.DieuKienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DKADDM_DK");
        });

        modelBuilder.Entity<DieuKienApDungSanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DieuKien__3213E83F7827BCA6");

            entity.ToTable("DieuKienApDungSanPham", "core");

            entity.HasIndex(e => e.DieuKienId, "IX_DieuKienApDungSanPham_dieuKienId");

            entity.HasIndex(e => e.SanPhamId, "IX_DieuKienApDungSanPham_sanPhamId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.DieuKienId)
                .HasMaxLength(50)
                .HasColumnName("dieuKienId");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.SanPhamId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamId");

            entity.HasOne(d => d.DieuKien).WithMany(p => p.DieuKienApDungSanPhams)
                .HasForeignKey(d => d.DieuKienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DKADSP_DK");

            entity.HasOne(d => d.SanPham).WithMany(p => p.DieuKienApDungSanPhams)
                .HasForeignKey(d => d.SanPhamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DKADSP_SP");
        });

        modelBuilder.Entity<DieuKienApDungToanBo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DieuKien__3213E83F8B829C4F");

            entity.ToTable("DieuKienApDungToanBo", "core");

            entity.HasIndex(e => e.DieuKienId, "IX_DieuKienApDungToanBo_dieuKienId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.DieuKienId)
                .HasMaxLength(50)
                .HasColumnName("dieuKienId");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(500)
                .HasColumnName("ghiChu");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");

            entity.HasOne(d => d.DieuKien).WithMany(p => p.DieuKienApDungToanBos)
                .HasForeignKey(d => d.DieuKienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DKADTB_DK");
        });

        modelBuilder.Entity<DonGiaoHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_DGH");

            entity.ToTable("DonGiaoHang", "core");

            entity.HasIndex(e => e.HoaDonId, "IX_DonGiaoHang_hoaDonId");

            entity.HasIndex(e => e.PhiVanChuyenId, "IX_DonGiaoHang_phiVanChuyenId");

            entity.HasIndex(e => e.ShipperId, "IX_DonGiaoHang_shipperId");

            entity.HasIndex(e => e.Id, "UQ__tmp_ms_x__3213E83EEE107A4B").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.HoaDonId)
                .HasMaxLength(50)
                .HasColumnName("hoaDonId");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.NgayGiao).HasColumnName("ngayGiao");
            entity.Property(e => e.PhiVanChuyenId)
                .HasMaxLength(50)
                .HasColumnName("phiVanChuyenId");
            entity.Property(e => e.ShipperId)
                .HasMaxLength(50)
                .HasColumnName("shipperId");
            entity.Property(e => e.TrangThaiHienTai)
                .HasMaxLength(20)
                .HasColumnName("trangThaiHienTai");

            entity.HasOne(d => d.HoaDon).WithMany(p => p.DonGiaoHangs)
                .HasForeignKey(d => d.HoaDonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DGH_HD");

            entity.HasOne(d => d.PhiVanChuyen).WithMany(p => p.DonGiaoHangs)
                .HasForeignKey(d => d.PhiVanChuyenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DGH_PVC");

            entity.HasOne(d => d.Shipper).WithMany(p => p.DonGiaoHangs)
                .HasForeignKey(d => d.ShipperId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DGH_SP");
        });

        modelBuilder.Entity<DonHangOnline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DonHangO__3213E83FAA6F60FF");

            entity.ToTable("DonHangOnline", "core");

            entity.HasIndex(e => e.HoaDonId, "IX_DHO_HD");

            entity.HasIndex(e => e.KhachHangId, "IX_DonHangOnline_khachHangId");

            entity.HasIndex(e => e.HoaDonId, "UQ__DonHangO__4643563BBFAC1AA5").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.HoaDonId)
                .HasMaxLength(50)
                .HasColumnName("hoaDonId");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.KenhDat)
                .HasMaxLength(20)
                .HasColumnName("kenhDat");
            entity.Property(e => e.KhachHangId)
                .HasMaxLength(50)
                .HasColumnName("khachHangId");
            entity.Property(e => e.NgayDat).HasColumnName("ngayDat");
            entity.Property(e => e.TongTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tongTien");

            entity.HasOne(d => d.HoaDon).WithOne(p => p.DonHangOnline)
                .HasForeignKey<DonHangOnline>(d => d.HoaDonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DHO_HD");

            entity.HasOne(d => d.KhachHang).WithMany(p => p.DonHangOnlines)
                .HasForeignKey(d => d.KhachHangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DHO_KH");
        });

        modelBuilder.Entity<DonViDoLuong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DonViDoL__3213E83F05CA6401");

            entity.ToTable("DonViDoLuong", "core");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.KyHieu)
                .HasMaxLength(50)
                .HasColumnName("kyHieu");
            entity.Property(e => e.Ten)
                .HasMaxLength(200)
                .HasColumnName("ten");
        });

        modelBuilder.Entity<GiaoDichThanhToan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GiaoDich__3213E83F603FDFFA");

            entity.ToTable("GiaoDichThanhToan", "core");

            entity.HasIndex(e => e.HoaDonId, "IX_GiaoDichThanhToan_hoaDonId");

            entity.HasIndex(e => e.KenhThanhToanId, "IX_GiaoDichThanhToan_kenhThanhToanId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.HoaDonId)
                .HasMaxLength(50)
                .HasColumnName("hoaDonId");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.KenhThanhToanId)
                .HasMaxLength(50)
                .HasColumnName("kenhThanhToanId");
            entity.Property(e => e.MoTa)
                .HasMaxLength(500)
                .HasColumnName("moTa");
            entity.Property(e => e.NgayGd).HasColumnName("ngayGD");
            entity.Property(e => e.SoTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("soTien");

            entity.HasOne(d => d.HoaDon).WithMany(p => p.GiaoDichThanhToans)
                .HasForeignKey(d => d.HoaDonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GDTT_HD");

            entity.HasOne(d => d.KenhThanhToan).WithMany(p => p.GiaoDichThanhToans)
                .HasForeignKey(d => d.KenhThanhToanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GDTT_KTT");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => new { e.TaiKhoanId, e.SanPhamDonViId });

            entity.ToTable("GioHang", "core");

            entity.Property(e => e.TaiKhoanId)
                .HasMaxLength(50)
                .HasColumnName("taiKhoanId");
            entity.Property(e => e.SanPhamDonViId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamDonViId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.SanPhamDonVi).WithMany(p => p.GioHangs)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.SanPhamDonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GioHang_SanPhamDonVi");

            entity.HasOne(d => d.TaiKhoan).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.TaiKhoanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GioHang_TaiKhoan");
        });

        modelBuilder.Entity<HinhAnh>(entity =>
        {
            entity.ToTable("HinhAnh");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.TenAnh).HasMaxLength(100);
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HoaDon__3213E83F5212F956");

            entity.ToTable("HoaDon", "core");

            entity.HasIndex(e => e.KhachHangId, "IX_HD_khach");

            entity.HasIndex(e => e.NhanVienId, "IX_HoaDon_nhanVienId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.KhachHangId)
                .HasMaxLength(50)
                .HasColumnName("khachHangId");
            entity.Property(e => e.NgayLap)
                .HasPrecision(0)
                .HasColumnName("ngayLap");
            entity.Property(e => e.NhanVienId)
                .HasMaxLength(50)
                .HasColumnName("nhanVienId");
            entity.Property(e => e.TongTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tongTien");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasColumnName("trangThai");

            entity.HasOne(d => d.KhachHang).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.KhachHangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoaDon_KhachHang");

            entity.HasOne(d => d.NhanVien).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.NhanVienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoaDon_NhanVien");
        });

        modelBuilder.Entity<KenhThanhToan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KenhThan__3213E83F2F0444F7");

            entity.ToTable("KenhThanhToan", "core");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.CauHinh).HasColumnName("cauHinh");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("createdAt");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.LoaiKenh)
                .HasMaxLength(20)
                .HasColumnName("loaiKenh");
            entity.Property(e => e.PhiGiaoDich)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("phiGiaoDich");
            entity.Property(e => e.TenKenh)
                .HasMaxLength(200)
                .HasColumnName("tenKenh");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasColumnName("trangThai");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updatedAt");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KhachHan__3213E83F168CE425");

            entity.ToTable("KhachHang", "core");

            entity.HasIndex(e => e.SoDienThoai, "Index_KhachHang_1").IsUnique();

            entity.HasIndex(e => e.Email, "Index_KhachHang_2").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(500)
                .HasColumnName("diaChi");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.GioiTinh).HasColumnName("gioiTinh");
            entity.Property(e => e.HoTen)
                .HasMaxLength(200)
                .HasColumnName("hoTen");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.NgayDangKy).HasColumnName("ngayDangKy");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(50)
                .HasColumnName("soDienThoai");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasColumnName("trangThai");
        });

        modelBuilder.Entity<KiemKe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KiemKe__3213E83F4355DB52");

            entity.ToTable("KiemKe", "core");

            entity.HasIndex(e => e.NhanVienId, "IX_KiemKe_nhanVienId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.KetQua)
                .HasMaxLength(500)
                .HasColumnName("ketQua");
            entity.Property(e => e.NgayKiemKe).HasColumnName("ngayKiemKe");
            entity.Property(e => e.NhanVienId)
                .HasMaxLength(50)
                .HasColumnName("nhanVienId");
            entity.Property(e => e.SanPhamDonViId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamDonViID");

            entity.HasOne(d => d.NhanVien).WithMany(p => p.KiemKes)
                .HasForeignKey(d => d.NhanVienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KiemKe_NV");

            entity.HasOne(d => d.SanPhamDonVi).WithMany(p => p.KiemKes)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.SanPhamDonViId)
                .HasConstraintName("FK_KiemKe_SP");
        });

        modelBuilder.Entity<LichSuGiaBan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LichSuGi__3213E83F5BEC2A0A");

            entity.ToTable("LichSuGiaBan", "core");

            entity.HasIndex(e => e.DonViId, "IX_LichSuGiaBan_donViId");

            entity.HasIndex(e => e.SanPhamId, "IX_LichSuGiaBan_sanPhamId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.DonViId)
                .HasMaxLength(50)
                .HasColumnName("donViId");
            entity.Property(e => e.GiaBan)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("giaBan");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.NgayBatDau).HasColumnName("ngayBatDau");
            entity.Property(e => e.NgayKetThuc).HasColumnName("ngayKetThuc");
            entity.Property(e => e.SanPhamId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamId");

            entity.HasOne(d => d.DonVi).WithMany(p => p.LichSuGiaBans)
                .HasForeignKey(d => d.DonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LSGB_DonVi");

            entity.HasOne(d => d.SanPham).WithMany(p => p.LichSuGiaBans)
                .HasForeignKey(d => d.SanPhamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LSGB_SanPham");
        });

        modelBuilder.Entity<LichSuGiaoDich>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LichSuGi__3213E83FEE6A44AB");

            entity.ToTable("LichSuGiaoDich", "core");

            entity.HasIndex(e => e.NhaCungCapId, "IX_LichSuGiaoDich_nhaCungCapId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.NgayGd).HasColumnName("ngayGD");
            entity.Property(e => e.NhaCungCapId)
                .HasMaxLength(50)
                .HasColumnName("nhaCungCapId");
            entity.Property(e => e.TongTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tongTien");

            entity.HasOne(d => d.NhaCungCap).WithMany(p => p.LichSuGiaoDiches)
                .HasForeignKey(d => d.NhaCungCapId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LSGD_NCC");
        });

        modelBuilder.Entity<LichSuMuaHang>(entity =>
        {
            entity.HasKey(e => new { e.KhachHangId, e.HoaDonId }).HasName("PK_LSMH");

            entity.ToTable("LichSuMuaHang", "core");

            entity.HasIndex(e => e.HoaDonId, "IX_LichSuMuaHang_hoaDonId");

            entity.Property(e => e.KhachHangId)
                .HasMaxLength(50)
                .HasColumnName("khachHangId");
            entity.Property(e => e.HoaDonId)
                .HasMaxLength(50)
                .HasColumnName("hoaDonId");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.NgayMua).HasColumnName("ngayMua");
            entity.Property(e => e.TongTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tongTien");

            entity.HasOne(d => d.HoaDon).WithMany(p => p.LichSuMuaHangs)
                .HasForeignKey(d => d.HoaDonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LSMH_HD");

            entity.HasOne(d => d.KhachHang).WithMany(p => p.LichSuMuaHangs)
                .HasForeignKey(d => d.KhachHangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LSMH_KH");
        });

        modelBuilder.Entity<MaDinhDanhSanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MaDinhDa__3213E83FB8B9410D");

            entity.ToTable("MaDinhDanhSanPham", "core");

            entity.HasIndex(e => e.SanPhamDonViId, "IX_MaDinhDanhSanPham_sanPhamDonViId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.DuongDan)
                .HasMaxLength(500)
                .HasColumnName("duongDan");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.LoaiMa)
                .HasMaxLength(10)
                .HasColumnName("loaiMa");
            entity.Property(e => e.MaCode)
                .HasMaxLength(200)
                .HasColumnName("maCode");
            entity.Property(e => e.SanPhamDonViId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamDonViId");

            entity.HasOne(d => d.SanPhamDonVi).WithMany(p => p.MaDinhDanhSanPhams)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.SanPhamDonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MDD_SPDV");
        });

        modelBuilder.Entity<MaKhuyenMai>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MaKhuyen__3213E83FAAB31469");

            entity.ToTable("MaKhuyenMai", "core");

            entity.HasIndex(e => e.ChuongTrinhId, "IX_MaKhuyenMai_chuongTrinhId");

            entity.HasIndex(e => e.Code, "UQ__MaKhuyen__357D4CF9340F2F1C").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.ChuongTrinhId)
                .HasMaxLength(50)
                .HasColumnName("chuongTrinhId");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.GiaTri)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("giaTri");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.NgayBatDau).HasColumnName("ngayBatDau");
            entity.Property(e => e.NgayKetThuc).HasColumnName("ngayKetThuc");
            entity.Property(e => e.SoLanSuDung).HasColumnName("soLanSuDung");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasColumnName("trangThai");

            entity.HasOne(d => d.ChuongTrinh).WithMany(p => p.MaKhuyenMais)
                .HasForeignKey(d => d.ChuongTrinhId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MKM_CTKM");
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhaCungC__3213E83F2D28D041");

            entity.ToTable("NhaCungCap", "core");

            entity.HasIndex(e => e.SoDienThoai, "Index_NhaCungCap_1").IsUnique();

            entity.HasIndex(e => e.Email, "Index_NhaCungCap_2").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(500)
                .HasColumnName("diaChi");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.MaSoThue)
                .HasMaxLength(50)
                .HasColumnName("maSoThue");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(50)
                .HasColumnName("soDienThoai");
            entity.Property(e => e.Ten)
                .HasMaxLength(200)
                .HasColumnName("ten");
        });

        modelBuilder.Entity<NhanHieu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhanHieu__3213E83FB4538AA9");

            entity.ToTable("NhanHieu", "core");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.Ten)
                .HasMaxLength(200)
                .HasColumnName("ten");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhanVien__3213E83F4D14CBFC");

            entity.ToTable("NhanVien", "core");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.ChucVu)
                .HasMaxLength(100)
                .HasColumnName("chucVu");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(500)
                .HasColumnName("diaChi");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.GioiTinh).HasColumnName("gioiTinh");
            entity.Property(e => e.HoTen)
                .HasMaxLength(200)
                .HasColumnName("hoTen");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.LuongCoBan)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("luongCoBan");
            entity.Property(e => e.NgayVaoLam).HasColumnName("ngayVaoLam");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(50)
                .HasColumnName("soDienThoai");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasColumnName("trangThai");
        });

        modelBuilder.Entity<NhatKyHoatDong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhatKyHo__3213E83F73810801");

            entity.ToTable("NhatKyHoatDong", "core");

            entity.HasIndex(e => e.TaiKhoanId, "IX_NhatKyHoatDong_taiKhoanId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.HanhDong)
                .HasMaxLength(500)
                .HasColumnName("hanhDong");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.TaiKhoanId)
                .HasMaxLength(50)
                .HasColumnName("taiKhoanId");
            entity.Property(e => e.ThoiGian)
                .HasPrecision(0)
                .HasColumnName("thoiGian");

            entity.HasOne(d => d.TaiKhoan).WithMany(p => p.NhatKyHoatDongs)
                .HasForeignKey(d => d.TaiKhoanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NKHD_TK");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3213E83FA402C283");

            entity.ToTable("Permission", "core");

            entity.HasIndex(e => e.Code, "UQ__Permissi__357D4CF9AB825447").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.MoTa)
                .HasMaxLength(500)
                .HasColumnName("moTa");
            entity.Property(e => e.Ten)
                .HasMaxLength(200)
                .HasColumnName("ten");
        });

        modelBuilder.Entity<PhanCongCaLamViec>(entity =>
        {
            entity.HasKey(e => new { e.NhanVienId, e.CaLamViecId, e.Ngay }).HasName("PK_PhanCong");

            entity.ToTable("PhanCongCaLamViec", "core");

            entity.HasIndex(e => e.CaLamViecId, "IX_PhanCongCaLamViec_caLamViecId");

            entity.HasIndex(e => e.Id, "UQ__PhanCong__3213E83E23C543A0")
                .IsUnique()
                .HasFilter("([id] IS NOT NULL)");

            entity.Property(e => e.NhanVienId)
                .HasMaxLength(50)
                .HasColumnName("nhanVienId");
            entity.Property(e => e.CaLamViecId)
                .HasMaxLength(50)
                .HasColumnName("caLamViecId");
            entity.Property(e => e.Ngay).HasColumnName("ngay");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");

            entity.HasOne(d => d.CaLamViec).WithMany(p => p.PhanCongCaLamViecs)
                .HasForeignKey(d => d.CaLamViecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhanCong_Ca");

            entity.HasOne(d => d.NhanVien).WithMany(p => p.PhanCongCaLamViecs)
                .HasForeignKey(d => d.NhanVienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhanCong_NV");
        });

        modelBuilder.Entity<PhiVanChuyen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhiVanCh__3213E83F88433FDE");

            entity.ToTable("PhiVanChuyen", "core");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.PhuongThucTinh)
                .HasMaxLength(100)
                .HasColumnName("phuongThucTinh");
            entity.Property(e => e.SoTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("soTien");
        });

        modelBuilder.Entity<PhieuDoiTra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhieuDoi__3213E83FF7A1E82D");

            entity.ToTable("PhieuDoiTra", "core");

            entity.HasIndex(e => e.ChinhSachId, "IX_PhieuDoiTra_chinhSachId");

            entity.HasIndex(e => e.HoaDonId, "IX_PhieuDoiTra_hoaDonId");

            entity.HasIndex(e => e.SanPhamDonViId, "IX_PhieuDoiTra_sanPhamDonViId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.ChinhSachId)
                .HasMaxLength(50)
                .HasColumnName("chinhSachId");
            entity.Property(e => e.HoaDonId)
                .HasMaxLength(50)
                .HasColumnName("hoaDonId");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.LyDo)
                .HasMaxLength(500)
                .HasColumnName("lyDo");
            entity.Property(e => e.NgayDoiTra).HasColumnName("ngayDoiTra");
            entity.Property(e => e.SanPhamDonViId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamDonViId");
            entity.Property(e => e.SoTienHoan)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("soTienHoan");

            entity.HasOne(d => d.ChinhSach).WithMany(p => p.PhieuDoiTras)
                .HasForeignKey(d => d.ChinhSachId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PDT_CS");

            entity.HasOne(d => d.HoaDon).WithMany(p => p.PhieuDoiTras)
                .HasForeignKey(d => d.HoaDonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PDT_HD");

            entity.HasOne(d => d.SanPhamDonVi).WithMany(p => p.PhieuDoiTras)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.SanPhamDonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PDT_SPDV");
        });

        modelBuilder.Entity<PhieuNhap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhieuNha__3213E83F632478F9");

            entity.ToTable("PhieuNhap", "core");

            entity.HasIndex(e => e.NhaCungCapId, "IX_PhieuNhap_nhaCungCapId");

            entity.HasIndex(e => e.NhanVienId, "IX_PhieuNhap_nhanVienId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.NgayNhap).HasColumnName("ngayNhap");
            entity.Property(e => e.NhaCungCapId)
                .HasMaxLength(50)
                .HasColumnName("nhaCungCapId");
            entity.Property(e => e.NhanVienId)
                .HasMaxLength(50)
                .HasColumnName("nhanVienId");
            entity.Property(e => e.TongTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tongTien");

            entity.HasOne(d => d.NhaCungCap).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.NhaCungCapId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PN_NCC");

            entity.HasOne(d => d.NhanVien).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.NhanVienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PN_NV");
        });

        modelBuilder.Entity<PhieuXuat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PhieuXua__3213E83F3C8AF210");

            entity.ToTable("PhieuXuat", "core");

            entity.HasIndex(e => e.KhachHangId, "IX_PhieuXuat_khachHangId");

            entity.HasIndex(e => e.NhanVienId, "IX_PhieuXuat_nhanVienId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.KhachHangId)
                .HasMaxLength(50)
                .HasColumnName("khachHangId");
            entity.Property(e => e.NgayXuat).HasColumnName("ngayXuat");
            entity.Property(e => e.NhanVienId)
                .HasMaxLength(50)
                .HasColumnName("nhanVienId");
            entity.Property(e => e.TongTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tongTien");

            entity.HasOne(d => d.KhachHang).WithMany(p => p.PhieuXuats)
                .HasForeignKey(d => d.KhachHangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PX_KH");

            entity.HasOne(d => d.NhanVien).WithMany(p => p.PhieuXuats)
                .HasForeignKey(d => d.NhanVienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PX_NV");
        });

        modelBuilder.Entity<Qrcode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QRCode__3213E83FF13BADA6");

            entity.ToTable("QRCode", "core");

            entity.HasIndex(e => e.MaDinhDanhId, "IX_QRCode_maDinhDanhId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.MaDinhDanhId)
                .HasMaxLength(50)
                .HasColumnName("maDinhDanhId");
            entity.Property(e => e.QrCodeImage)
                .HasMaxLength(500)
                .HasColumnName("qrCodeImage");

            entity.HasOne(d => d.MaDinhDanh).WithMany(p => p.Qrcodes)
                .HasForeignKey(d => d.MaDinhDanhId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QR_MDD");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3213E83F6D7FA3BA");

            entity.ToTable("Role", "core");

            entity.HasIndex(e => e.Code, "UQ__Role__357D4CF9DD99A6B0").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.MoTa)
                .HasMaxLength(500)
                .HasColumnName("moTa");
            entity.Property(e => e.Ten)
                .HasMaxLength(200)
                .HasColumnName("ten");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasColumnName("trangThai");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("PK_RP");

            entity.ToTable("RolePermission", "core");

            entity.HasIndex(e => e.PermissionId, "IX_RolePermission_permissionId");

            entity.HasIndex(e => e.Id, "UQ__RolePerm__3213E83EEEFE6DC5")
                .IsUnique()
                .HasFilter("([id] IS NOT NULL)");

            entity.Property(e => e.RoleId)
                .HasMaxLength(50)
                .HasColumnName("roleId");
            entity.Property(e => e.PermissionId)
                .HasMaxLength(50)
                .HasColumnName("permissionId");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RP_Permission");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RP_Role");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SanPham__3213E83F9FAE08F2");

            entity.ToTable("SanPham", "core");

            entity.HasIndex(e => e.NhanHieuId, "IX_SP_nhanHieu");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.MoTa).HasColumnName("moTa");
            entity.Property(e => e.NhanHieuId)
                .HasMaxLength(50)
                .HasColumnName("nhanHieuId");
            entity.Property(e => e.Ten)
                .HasMaxLength(255)
                .HasColumnName("ten");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasColumnName("trangThai");

            entity.HasOne(d => d.NhanHieu).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.NhanHieuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SanPham_NhanHieu");
        });

        modelBuilder.Entity<SanPhamDanhMuc>(entity =>
        {
            entity.HasKey(e => new { e.SanPhamId, e.DanhMucId });

            entity.ToTable("SanPhamDanhMuc", "core");

            entity.HasIndex(e => e.DanhMucId, "IX_SanPhamDanhMuc_danhMucId");

            entity.HasIndex(e => e.Id, "UQ__SanPhamD__3213E83E62FEA25D")
                .IsUnique()
                .HasFilter("([id] IS NOT NULL)");

            entity.Property(e => e.SanPhamId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamId");
            entity.Property(e => e.DanhMucId)
                .HasMaxLength(50)
                .HasColumnName("danhMucId");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");

            entity.HasOne(d => d.DanhMuc).WithMany(p => p.SanPhamDanhMucs)
                .HasForeignKey(d => d.DanhMucId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SanPhamDanhMuc_DanhMuc");

            entity.HasOne(d => d.SanPham).WithMany(p => p.SanPhamDanhMucs)
                .HasForeignKey(d => d.SanPhamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SanPhamDanhMuc_SanPham");
        });

        modelBuilder.Entity<SanPhamDonVi>(entity =>
        {
            entity.HasKey(e => new { e.SanPhamId, e.DonViId });

            entity.ToTable("SanPhamDonVi", "core");

            entity.HasIndex(e => e.Id, "AK_SanPhamDonVi_id").IsUnique();

            entity.HasIndex(e => e.Id, "IX_SPDV_id");

            entity.HasIndex(e => e.DonViId, "IX_SanPhamDonVi_donViId");

            entity.HasIndex(e => e.Id, "UQ__SanPhamD__3213E83ECC46A234").IsUnique();

            entity.Property(e => e.SanPhamId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamId");
            entity.Property(e => e.DonViId)
                .HasMaxLength(50)
                .HasColumnName("donViId");
            entity.Property(e => e.GiaBan)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("giaBan");
            entity.Property(e => e.HeSoQuyDoi)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("heSoQuyDoi");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");

            entity.HasOne(d => d.DonVi).WithMany(p => p.SanPhamDonVis)
                .HasForeignKey(d => d.DonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SanPhamDonVi_DonVi");

            entity.HasOne(d => d.SanPham).WithMany(p => p.SanPhamDonVis)
                .HasForeignKey(d => d.SanPhamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SanPhamDonVi_SanPham");
        });

        modelBuilder.Entity<SanPhamViTri>(entity =>
        {
            entity.HasKey(e => new { e.SanPhamDonViId, e.ViTriId }).HasName("PK_SPVT");

            entity.ToTable("SanPhamViTri", "core");

            entity.HasIndex(e => e.ViTriId, "IX_SanPhamViTri_viTriId");

            entity.HasIndex(e => e.Id, "UQ__SanPhamV__3213E83EB9656589")
                .IsUnique()
                .HasFilter("([id] IS NOT NULL)");

            entity.Property(e => e.SanPhamDonViId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamDonViId");
            entity.Property(e => e.ViTriId)
                .HasMaxLength(50)
                .HasColumnName("viTriId");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");

            entity.HasOne(d => d.SanPhamDonVi).WithMany(p => p.SanPhamViTris)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.SanPhamDonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SPVT_SPDV");

            entity.HasOne(d => d.ViTri).WithMany(p => p.SanPhamViTris)
                .HasForeignKey(d => d.ViTriId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SPVT_ViTri");
        });

        modelBuilder.Entity<Shipper>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shipper__3213E83FC4207263");

            entity.ToTable("Shipper", "core");

            entity.HasIndex(e => e.SoDienThoai, "Index_Shipper_1").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.HoTen)
                .HasMaxLength(200)
                .HasColumnName("hoTen");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(50)
                .HasColumnName("soDienThoai");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaiKhoan__3213E83F8B2017A0");

            entity.ToTable("TaiKhoan", "core");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.MatKhauHash)
                .HasMaxLength(255)
                .HasColumnName("matKhauHash");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(100)
                .HasColumnName("tenDangNhap");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasColumnName("trangThai");
        });

        modelBuilder.Entity<TaiKhoanKhachHang>(entity =>
        {
            entity.HasKey(e => new { e.KhachHangId, e.TaiKhoanid }).HasName("PK_TKKH");

            entity.ToTable("TaiKhoanKhachHang", "core");

            entity.HasIndex(e => e.TaiKhoanid, "UQ__tmp_ms_x__701B5126D1F8EDF3").IsUnique();

            entity.Property(e => e.KhachHangId)
                .HasMaxLength(50)
                .HasColumnName("khachHangId");
            entity.Property(e => e.TaiKhoanid)
                .HasMaxLength(50)
                .HasColumnName("taiKhoanid");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");

            entity.HasOne(d => d.KhachHang).WithMany(p => p.TaiKhoanKhachHangs)
                .HasForeignKey(d => d.KhachHangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TKKH_KH");

            entity.HasOne(d => d.TaiKhoan).WithOne(p => p.TaiKhoanKhachHang)
                .HasForeignKey<TaiKhoanKhachHang>(d => d.TaiKhoanid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TKKH_TK");
        });

        modelBuilder.Entity<TaiKhoanNhanVien>(entity =>
        {
            entity.HasKey(e => new { e.NhanVienId, e.TaiKhoanId }).HasName("PK_TKNV");

            entity.ToTable("TaiKhoanNhanVien", "core");

            entity.HasIndex(e => e.TaiKhoanId, "UQ__tmp_ms_x__701A5D1E31FA1692").IsUnique();

            entity.Property(e => e.NhanVienId)
                .HasMaxLength(50)
                .HasColumnName("nhanVienId");
            entity.Property(e => e.TaiKhoanId)
                .HasMaxLength(50)
                .HasColumnName("taiKhoanId");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");

            entity.HasOne(d => d.NhanVien).WithMany(p => p.TaiKhoanNhanViens)
                .HasForeignKey(d => d.NhanVienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TKNV_NV");

            entity.HasOne(d => d.TaiKhoan).WithOne(p => p.TaiKhoanNhanVien)
                .HasForeignKey<TaiKhoanNhanVien>(d => d.TaiKhoanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TKNV_TK");
        });

        modelBuilder.Entity<TemNhan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TemNhan__3213E83F9EB50D82");

            entity.ToTable("TemNhan", "core");

            entity.HasIndex(e => e.MaDinhDanhId, "IX_TemNhan_maDinhDanhId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.MaDinhDanhId)
                .HasMaxLength(50)
                .HasColumnName("maDinhDanhId");
            entity.Property(e => e.NgayIn).HasColumnName("ngayIn");
            entity.Property(e => e.NoiDungTem).HasColumnName("noiDungTem");

            entity.HasOne(d => d.MaDinhDanh).WithMany(p => p.TemNhans)
                .HasForeignKey(d => d.MaDinhDanhId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tem_MDD");
        });

        modelBuilder.Entity<TheThanhVien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TheThanh__3213E83FCD40305D");

            entity.ToTable("TheThanhVien", "core");

            entity.HasIndex(e => e.KhachHangId, "IX_TheThanhVien_khachHangId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.DiemTichLuy).HasColumnName("diemTichLuy");
            entity.Property(e => e.Hang)
                .HasMaxLength(20)
                .HasColumnName("hang");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.KhachHangId)
                .HasMaxLength(50)
                .HasColumnName("khachHangId");
            entity.Property(e => e.NgayCap).HasColumnName("ngayCap");

            entity.HasOne(d => d.KhachHang).WithMany(p => p.TheThanhViens)
                .HasForeignKey(d => d.KhachHangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TTV_KH");
        });

        modelBuilder.Entity<TonKho>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TonKho__3213E83FE16F2840");

            entity.ToTable("TonKho", "core");

            entity.HasIndex(e => e.SanPhamDonViId, "IX_TonKho_sanPhamDonViId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.SanPhamDonViId)
                .HasMaxLength(50)
                .HasColumnName("sanPhamDonViId");
            entity.Property(e => e.SoLuongTon).HasColumnName("soLuongTon");

            entity.HasOne(d => d.SanPhamDonVi).WithMany(p => p.TonKhos)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.SanPhamDonViId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TonKho_SPDV");
        });

        modelBuilder.Entity<TrangThaiGiaoHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TrangTha__3213E83F4A6FED2F");

            entity.ToTable("TrangThaiGiaoHang", "core");

            entity.HasIndex(e => e.DonGiaoHangId, "IX_TrangThaiGiaoHang_donGiaoHangId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.DonGiaoHangId)
                .HasMaxLength(50)
                .HasColumnName("donGiaoHangId");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(500)
                .HasColumnName("ghiChu");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.NgayCapNhat)
                .HasPrecision(0)
                .HasColumnName("ngayCapNhat");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasColumnName("trangThai");

            entity.HasOne(d => d.DonGiaoHang).WithMany(p => p.TrangThaiGiaoHangs)
                .HasForeignKey(d => d.DonGiaoHangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TTGH_DGH");
        });

        modelBuilder.Entity<TrangThaiXuLy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TrangTha__3213E83FF8C7651F");

            entity.ToTable("TrangThaiXuLy", "core");

            entity.HasIndex(e => e.DonHangId, "IX_TrangThaiXuLy_donHangId");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.DonHangId)
                .HasMaxLength(50)
                .HasColumnName("donHangId");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.NgayCapNhat)
                .HasPrecision(0)
                .HasColumnName("ngayCapNhat");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasColumnName("trangThai");

            entity.HasOne(d => d.DonHang).WithMany(p => p.TrangThaiXuLies)
                .HasForeignKey(d => d.DonHangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TTXL_DH");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.TaiKhoanId, e.RoleId }).HasName("PK_UR");

            entity.ToTable("UserRole", "core");

            entity.HasIndex(e => e.RoleId, "IX_UserRole_roleId");

            entity.HasIndex(e => e.Id, "UQ__UserRole__3213E83E44209D3B")
                .IsUnique()
                .HasFilter("([id] IS NOT NULL)");

            entity.Property(e => e.TaiKhoanId)
                .HasMaxLength(50)
                .HasColumnName("taiKhoanId");
            entity.Property(e => e.RoleId)
                .HasMaxLength(50)
                .HasColumnName("roleId");
            entity.Property(e => e.HieuLucDen).HasColumnName("hieuLucDen");
            entity.Property(e => e.HieuLucTu).HasColumnName("hieuLucTu");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UR_Role");

            entity.HasOne(d => d.TaiKhoan).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.TaiKhoanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UR_TK");
        });

        modelBuilder.Entity<ViTri>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ViTri__3213E83FC6CB7FE6");

            entity.ToTable("ViTri", "core");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.LoaiViTri)
                .HasMaxLength(20)
                .HasColumnName("loaiViTri");
            entity.Property(e => e.MaViTri)
                .HasMaxLength(100)
                .HasColumnName("maViTri");
            entity.Property(e => e.MoTa)
                .HasMaxLength(500)
                .HasColumnName("moTa");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

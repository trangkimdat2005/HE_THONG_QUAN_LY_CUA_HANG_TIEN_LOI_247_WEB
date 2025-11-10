IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF SCHEMA_ID(N'core') IS NULL EXEC(N'CREATE SCHEMA [core];');

CREATE TABLE [core].[BaoCao] (
    [id] nvarchar(50) NOT NULL,
    [loaiBaoCao] nvarchar(20) NOT NULL,
    [ngayLap] date NOT NULL,
    [fileBaoCao] nvarchar(500) NOT NULL,
    [isDelete] bit NOT NULL,
    [tuNgay] date NOT NULL,
    [denNgay] date NOT NULL,
    CONSTRAINT [PK__BaoCao__3213E83FE22F5722] PRIMARY KEY ([id])
);

CREATE TABLE [core].[CaLamViec] (
    [id] nvarchar(50) NOT NULL,
    [tenCa] nvarchar(100) NOT NULL,
    [thoiGianBatDau] time(0) NOT NULL,
    [thoiGianKetThuc] time(0) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__CaLamVie__3213E83FCF27A89E] PRIMARY KEY ([id])
);

CREATE TABLE [core].[ChinhSachHoanTra] (
    [id] nvarchar(50) NOT NULL,
    [tenChinhSach] nvarchar(200) NOT NULL,
    [thoiHan] int NULL,
    [dieuKien] nvarchar(500) NOT NULL,
    [apDungToanBo] bit NULL DEFAULT CAST(0 AS bit),
    [apDungTuNgay] date NOT NULL,
    [apDungDenNgay] date NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__ChinhSac__3213E83FD509BE56] PRIMARY KEY ([id])
);

CREATE TABLE [core].[ChuongTrinhKhuyenMai] (
    [id] nvarchar(50) NOT NULL,
    [ten] nvarchar(255) NOT NULL,
    [loai] nvarchar(30) NOT NULL,
    [ngayBatDau] date NOT NULL,
    [ngayKetThuc] date NOT NULL,
    [moTa] nvarchar(max) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__ChuongTr__3213E83F918CF90D] PRIMARY KEY ([id])
);

CREATE TABLE [core].[DanhMuc] (
    [id] nvarchar(50) NOT NULL,
    [ten] nvarchar(200) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__DanhMuc__3213E83FECBD45A0] PRIMARY KEY ([id])
);

CREATE TABLE [core].[DonViDoLuong] (
    [id] nvarchar(50) NOT NULL,
    [ten] nvarchar(200) NOT NULL,
    [kyHieu] nvarchar(50) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__DonViDoL__3213E83F05CA6401] PRIMARY KEY ([id])
);

CREATE TABLE [HinhAnh] (
    [Id] nvarchar(50) NOT NULL,
    [TenAnh] nvarchar(100) NOT NULL,
    [Anh] varbinary(max) NOT NULL,
    CONSTRAINT [PK_HinhAnh] PRIMARY KEY ([Id])
);

CREATE TABLE [core].[KenhThanhToan] (
    [id] nvarchar(50) NOT NULL,
    [tenKenh] nvarchar(200) NOT NULL,
    [loaiKenh] nvarchar(20) NOT NULL,
    [phiGiaoDich] decimal(18,2) NOT NULL,
    [trangThai] nvarchar(20) NOT NULL DEFAULT N'Active',
    [cauHinh] nvarchar(max) NULL,
    [createdAt] datetime2(0) NOT NULL DEFAULT ((sysutcdatetime())),
    [updatedAt] datetime2(0) NOT NULL DEFAULT ((sysutcdatetime())),
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__KenhThan__3213E83F2F0444F7] PRIMARY KEY ([id])
);

CREATE TABLE [core].[KhachHang] (
    [id] nvarchar(50) NOT NULL,
    [hoTen] nvarchar(200) NOT NULL,
    [soDienThoai] nvarchar(50) NOT NULL,
    [email] nvarchar(200) NULL,
    [diaChi] nvarchar(500) NOT NULL,
    [ngayDangKy] date NOT NULL,
    [trangThai] nvarchar(20) NOT NULL,
    [isDelete] bit NOT NULL,
    [gioiTinh] bit NOT NULL,
    CONSTRAINT [PK__KhachHan__3213E83F168CE425] PRIMARY KEY ([id])
);

CREATE TABLE [core].[NhaCungCap] (
    [id] nvarchar(50) NOT NULL,
    [ten] nvarchar(200) NOT NULL,
    [soDienThoai] nvarchar(50) NOT NULL,
    [email] nvarchar(200) NULL,
    [diaChi] nvarchar(500) NOT NULL,
    [maSoThue] nvarchar(50) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__NhaCungC__3213E83F2D28D041] PRIMARY KEY ([id])
);

CREATE TABLE [core].[NhanHieu] (
    [id] nvarchar(50) NOT NULL,
    [ten] nvarchar(200) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__NhanHieu__3213E83FB4538AA9] PRIMARY KEY ([id])
);

CREATE TABLE [core].[NhanVien] (
    [id] nvarchar(50) NOT NULL,
    [hoTen] nvarchar(200) NOT NULL,
    [chucVu] nvarchar(100) NOT NULL,
    [luongCoBan] decimal(18,2) NOT NULL,
    [soDienThoai] nvarchar(50) NOT NULL,
    [email] nvarchar(200) NULL,
    [diaChi] nvarchar(500) NOT NULL,
    [ngayVaoLam] date NOT NULL,
    [trangThai] nvarchar(20) NOT NULL,
    [isDelete] bit NOT NULL,
    [gioiTinh] bit NOT NULL,
    CONSTRAINT [PK__NhanVien__3213E83F4D14CBFC] PRIMARY KEY ([id])
);

CREATE TABLE [core].[Permission] (
    [id] nvarchar(50) NOT NULL,
    [code] nvarchar(100) NOT NULL,
    [ten] nvarchar(200) NOT NULL,
    [moTa] nvarchar(500) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__Permissi__3213E83FA402C283] PRIMARY KEY ([id])
);

CREATE TABLE [core].[PhiVanChuyen] (
    [id] nvarchar(50) NOT NULL,
    [soTien] decimal(18,2) NOT NULL,
    [phuongThucTinh] nvarchar(100) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__PhiVanCh__3213E83F88433FDE] PRIMARY KEY ([id])
);

CREATE TABLE [core].[Role] (
    [id] nvarchar(50) NOT NULL,
    [code] nvarchar(100) NOT NULL,
    [ten] nvarchar(200) NOT NULL,
    [moTa] nvarchar(500) NULL,
    [trangThai] nvarchar(20) NOT NULL DEFAULT N'Active',
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__Role__3213E83F6D7FA3BA] PRIMARY KEY ([id])
);

CREATE TABLE [core].[Shipper] (
    [id] nvarchar(50) NOT NULL,
    [hoTen] nvarchar(200) NOT NULL,
    [soDienThoai] nvarchar(50) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__Shipper__3213E83FC4207263] PRIMARY KEY ([id])
);

CREATE TABLE [core].[TaiKhoan] (
    [id] nvarchar(50) NOT NULL,
    [tenDangNhap] nvarchar(100) NOT NULL,
    [matKhauHash] nvarchar(255) NOT NULL,
    [email] nvarchar(200) NOT NULL,
    [trangThai] nvarchar(20) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__TaiKhoan__3213E83F8B2017A0] PRIMARY KEY ([id])
);

CREATE TABLE [core].[ViTri] (
    [id] nvarchar(50) NOT NULL,
    [maViTri] nvarchar(100) NOT NULL,
    [loaiViTri] nvarchar(20) NOT NULL,
    [moTa] nvarchar(500) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__ViTri__3213E83FC6CB7FE6] PRIMARY KEY ([id])
);

CREATE TABLE [core].[BaoCaoDoanhThu] (
    [id] nvarchar(50) NOT NULL,
    [baoCaoId] nvarchar(50) NOT NULL,
    [tongDoanhThu] decimal(18,2) NOT NULL,
    [kyBaoCao] nvarchar(100) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__BaoCaoDo__3213E83F0D878F66] PRIMARY KEY ([id]),
    CONSTRAINT [FK_BCDT_BC] FOREIGN KEY ([baoCaoId]) REFERENCES [core].[BaoCao] ([id])
);

CREATE TABLE [core].[DieuKienApDung] (
    [id] nvarchar(50) NOT NULL,
    [chuongTrinhId] nvarchar(50) NOT NULL,
    [dieuKien] nvarchar(500) NOT NULL,
    [giaTriToiThieu] decimal(18,2) NOT NULL,
    [giamTheo] nvarchar(20) NOT NULL,
    [giaTriToiDa] decimal(18,2) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__DieuKien__3213E83F9398724C] PRIMARY KEY ([id]),
    CONSTRAINT [FK_DKAD_CTKM] FOREIGN KEY ([chuongTrinhId]) REFERENCES [core].[ChuongTrinhKhuyenMai] ([id])
);

CREATE TABLE [core].[MaKhuyenMai] (
    [id] nvarchar(50) NOT NULL,
    [chuongTrinhId] nvarchar(50) NOT NULL,
    [code] nvarchar(100) NOT NULL,
    [giaTri] decimal(18,2) NOT NULL,
    [soLanSuDung] int NOT NULL,
    [trangThai] nvarchar(20) NOT NULL,
    [ngayBatDau] date NOT NULL,
    [ngayKetThuc] date NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__MaKhuyen__3213E83FAAB31469] PRIMARY KEY ([id]),
    CONSTRAINT [FK_MKM_CTKM] FOREIGN KEY ([chuongTrinhId]) REFERENCES [core].[ChuongTrinhKhuyenMai] ([id])
);

CREATE TABLE [core].[ChinhSachHoanTra_DanhMuc] (
    [chinhSachId] nvarchar(50) NOT NULL,
    [danhMucId] nvarchar(50) NOT NULL,
    [id] nvarchar(50) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_CSHTDM] PRIMARY KEY ([chinhSachId], [danhMucId]),
    CONSTRAINT [FK_CSHTDM_CS] FOREIGN KEY ([chinhSachId]) REFERENCES [core].[ChinhSachHoanTra] ([id]),
    CONSTRAINT [FK_CSHTDM_DM] FOREIGN KEY ([danhMucId]) REFERENCES [core].[DanhMuc] ([id])
);

CREATE TABLE [core].[TheThanhVien] (
    [id] nvarchar(50) NOT NULL,
    [khachHangId] nvarchar(50) NOT NULL,
    [hang] nvarchar(20) NOT NULL,
    [diemTichLuy] int NOT NULL,
    [ngayCap] date NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__TheThanh__3213E83FCD40305D] PRIMARY KEY ([id]),
    CONSTRAINT [FK_TTV_KH] FOREIGN KEY ([khachHangId]) REFERENCES [core].[KhachHang] ([id])
);

CREATE TABLE [core].[LichSuGiaoDich] (
    [id] nvarchar(50) NOT NULL,
    [nhaCungCapId] nvarchar(50) NOT NULL,
    [ngayGD] date NOT NULL,
    [tongTien] decimal(18,2) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__LichSuGi__3213E83FEE6A44AB] PRIMARY KEY ([id]),
    CONSTRAINT [FK_LSGD_NCC] FOREIGN KEY ([nhaCungCapId]) REFERENCES [core].[NhaCungCap] ([id])
);

CREATE TABLE [core].[SanPham] (
    [id] nvarchar(50) NOT NULL,
    [ten] nvarchar(255) NOT NULL,
    [nhanHieuId] nvarchar(50) NOT NULL,
    [moTa] nvarchar(max) NULL,
    [trangThai] nvarchar(30) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__SanPham__3213E83F9FAE08F2] PRIMARY KEY ([id]),
    CONSTRAINT [FK_SanPham_NhanHieu] FOREIGN KEY ([nhanHieuId]) REFERENCES [core].[NhanHieu] ([id])
);

CREATE TABLE [core].[ChamCong] (
    [id] nvarchar(50) NOT NULL,
    [nhanVienId] nvarchar(50) NOT NULL,
    [ngay] date NOT NULL,
    [gioVao] datetime2(0) NOT NULL,
    [gioRa] datetime2(0) NOT NULL,
    [ghiChu] nvarchar(500) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__ChamCong__3213E83FB805717D] PRIMARY KEY ([id]),
    CONSTRAINT [FK_ChamCong_NV] FOREIGN KEY ([nhanVienId]) REFERENCES [core].[NhanVien] ([id])
);

CREATE TABLE [core].[HoaDon] (
    [id] nvarchar(50) NOT NULL,
    [ngayLap] datetime2(0) NOT NULL,
    [tongTien] decimal(18,2) NULL,
    [nhanVienId] nvarchar(50) NOT NULL,
    [khachHangId] nvarchar(50) NOT NULL,
    [trangThai] nvarchar(20) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__HoaDon__3213E83F5212F956] PRIMARY KEY ([id]),
    CONSTRAINT [FK_HoaDon_KhachHang] FOREIGN KEY ([khachHangId]) REFERENCES [core].[KhachHang] ([id]),
    CONSTRAINT [FK_HoaDon_NhanVien] FOREIGN KEY ([nhanVienId]) REFERENCES [core].[NhanVien] ([id])
);

CREATE TABLE [core].[PhanCongCaLamViec] (
    [nhanVienId] nvarchar(50) NOT NULL,
    [caLamViecId] nvarchar(50) NOT NULL,
    [ngay] date NOT NULL,
    [id] nvarchar(50) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_PhanCong] PRIMARY KEY ([nhanVienId], [caLamViecId], [ngay]),
    CONSTRAINT [FK_PhanCong_Ca] FOREIGN KEY ([caLamViecId]) REFERENCES [core].[CaLamViec] ([id]),
    CONSTRAINT [FK_PhanCong_NV] FOREIGN KEY ([nhanVienId]) REFERENCES [core].[NhanVien] ([id])
);

CREATE TABLE [core].[PhieuNhap] (
    [id] nvarchar(50) NOT NULL,
    [nhaCungCapId] nvarchar(50) NOT NULL,
    [ngayNhap] date NOT NULL,
    [tongTien] decimal(18,2) NOT NULL,
    [isDelete] bit NOT NULL,
    [nhanVienId] nvarchar(50) NOT NULL,
    CONSTRAINT [PK__PhieuNha__3213E83F632478F9] PRIMARY KEY ([id]),
    CONSTRAINT [FK_PN_NCC] FOREIGN KEY ([nhaCungCapId]) REFERENCES [core].[NhaCungCap] ([id]),
    CONSTRAINT [FK_PN_NV] FOREIGN KEY ([nhanVienId]) REFERENCES [core].[NhanVien] ([id])
);

CREATE TABLE [core].[PhieuXuat] (
    [id] nvarchar(50) NOT NULL,
    [khachHangId] nvarchar(50) NOT NULL,
    [ngayXuat] date NOT NULL,
    [tongTien] decimal(18,2) NOT NULL,
    [isDelete] bit NOT NULL,
    [nhanVienId] nvarchar(50) NOT NULL,
    CONSTRAINT [PK__PhieuXua__3213E83F3C8AF210] PRIMARY KEY ([id]),
    CONSTRAINT [FK_PX_KH] FOREIGN KEY ([khachHangId]) REFERENCES [core].[KhachHang] ([id]),
    CONSTRAINT [FK_PX_NV] FOREIGN KEY ([nhanVienId]) REFERENCES [core].[NhanVien] ([id])
);

CREATE TABLE [core].[RolePermission] (
    [roleId] nvarchar(50) NOT NULL,
    [permissionId] nvarchar(50) NOT NULL,
    [id] nvarchar(50) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_RP] PRIMARY KEY ([roleId], [permissionId]),
    CONSTRAINT [FK_RP_Permission] FOREIGN KEY ([permissionId]) REFERENCES [core].[Permission] ([id]),
    CONSTRAINT [FK_RP_Role] FOREIGN KEY ([roleId]) REFERENCES [core].[Role] ([id])
);

CREATE TABLE [core].[NhatKyHoatDong] (
    [id] nvarchar(50) NOT NULL,
    [taiKhoanId] nvarchar(50) NOT NULL,
    [thoiGian] datetime2(0) NOT NULL,
    [hanhDong] nvarchar(500) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__NhatKyHo__3213E83F73810801] PRIMARY KEY ([id]),
    CONSTRAINT [FK_NKHD_TK] FOREIGN KEY ([taiKhoanId]) REFERENCES [core].[TaiKhoan] ([id])
);

CREATE TABLE [core].[TaiKhoanKhachHang] (
    [khachHangId] nvarchar(50) NOT NULL,
    [taiKhoanid] nvarchar(50) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_TKKH] PRIMARY KEY ([khachHangId], [taiKhoanid]),
    CONSTRAINT [FK_TKKH_KH] FOREIGN KEY ([khachHangId]) REFERENCES [core].[KhachHang] ([id]),
    CONSTRAINT [FK_TKKH_TK] FOREIGN KEY ([taiKhoanid]) REFERENCES [core].[TaiKhoan] ([id])
);

CREATE TABLE [core].[TaiKhoanNhanVien] (
    [nhanVienId] nvarchar(50) NOT NULL,
    [taiKhoanId] nvarchar(50) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_TKNV] PRIMARY KEY ([nhanVienId], [taiKhoanId]),
    CONSTRAINT [FK_TKNV_NV] FOREIGN KEY ([nhanVienId]) REFERENCES [core].[NhanVien] ([id]),
    CONSTRAINT [FK_TKNV_TK] FOREIGN KEY ([taiKhoanId]) REFERENCES [core].[TaiKhoan] ([id])
);

CREATE TABLE [core].[UserRole] (
    [taiKhoanId] nvarchar(50) NOT NULL,
    [roleId] nvarchar(50) NOT NULL,
    [hieuLucTu] date NULL,
    [hieuLucDen] date NULL,
    [id] nvarchar(50) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_UR] PRIMARY KEY ([taiKhoanId], [roleId]),
    CONSTRAINT [FK_UR_Role] FOREIGN KEY ([roleId]) REFERENCES [core].[Role] ([id]),
    CONSTRAINT [FK_UR_TK] FOREIGN KEY ([taiKhoanId]) REFERENCES [core].[TaiKhoan] ([id])
);

CREATE TABLE [core].[DieuKienApDungDanhMuc] (
    [id] nvarchar(50) NOT NULL,
    [dieuKienId] nvarchar(50) NOT NULL,
    [danhMucId] nvarchar(50) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__DieuKien__3213E83F8C100A0B] PRIMARY KEY ([id]),
    CONSTRAINT [FK_DKADDM_DK] FOREIGN KEY ([dieuKienId]) REFERENCES [core].[DieuKienApDung] ([id]),
    CONSTRAINT [FK_DKADDM_DM] FOREIGN KEY ([danhMucId]) REFERENCES [core].[DanhMuc] ([id])
);

CREATE TABLE [core].[DieuKienApDungToanBo] (
    [id] nvarchar(50) NOT NULL,
    [dieuKienId] nvarchar(50) NOT NULL,
    [ghiChu] nvarchar(500) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__DieuKien__3213E83F8B829C4F] PRIMARY KEY ([id]),
    CONSTRAINT [FK_DKADTB_DK] FOREIGN KEY ([dieuKienId]) REFERENCES [core].[DieuKienApDung] ([id])
);

CREATE TABLE [core].[BaoCaoBanChay] (
    [baoCaoId] nvarchar(50) NOT NULL,
    [sanPhamId] nvarchar(50) NOT NULL,
    [soLuongBan] int NOT NULL,
    [id] nvarchar(50) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_BCBC] PRIMARY KEY ([baoCaoId], [sanPhamId]),
    CONSTRAINT [FK_BCBC_BC] FOREIGN KEY ([baoCaoId]) REFERENCES [core].[BaoCao] ([id]),
    CONSTRAINT [FK_BCBC_SP] FOREIGN KEY ([sanPhamId]) REFERENCES [core].[SanPham] ([id])
);

CREATE TABLE [core].[DieuKienApDungSanPham] (
    [id] nvarchar(50) NOT NULL,
    [dieuKienId] nvarchar(50) NOT NULL,
    [sanPhamId] nvarchar(50) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__DieuKien__3213E83F7827BCA6] PRIMARY KEY ([id]),
    CONSTRAINT [FK_DKADSP_DK] FOREIGN KEY ([dieuKienId]) REFERENCES [core].[DieuKienApDung] ([id]),
    CONSTRAINT [FK_DKADSP_SP] FOREIGN KEY ([sanPhamId]) REFERENCES [core].[SanPham] ([id])
);

CREATE TABLE [core].[LichSuGiaBan] (
    [id] nvarchar(50) NOT NULL,
    [sanPhamId] nvarchar(50) NOT NULL,
    [donViId] nvarchar(50) NOT NULL,
    [giaBan] decimal(18,2) NOT NULL,
    [ngayBatDau] date NOT NULL,
    [ngayKetThuc] date NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__LichSuGi__3213E83F5BEC2A0A] PRIMARY KEY ([id]),
    CONSTRAINT [FK_LSGB_DonVi] FOREIGN KEY ([donViId]) REFERENCES [core].[DonViDoLuong] ([id]),
    CONSTRAINT [FK_LSGB_SanPham] FOREIGN KEY ([sanPhamId]) REFERENCES [core].[SanPham] ([id])
);

CREATE TABLE [core].[SanPhamDanhMuc] (
    [sanPhamId] nvarchar(50) NOT NULL,
    [danhMucId] nvarchar(50) NOT NULL,
    [id] nvarchar(50) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_SanPhamDanhMuc] PRIMARY KEY ([sanPhamId], [danhMucId]),
    CONSTRAINT [FK_SanPhamDanhMuc_DanhMuc] FOREIGN KEY ([danhMucId]) REFERENCES [core].[DanhMuc] ([id]),
    CONSTRAINT [FK_SanPhamDanhMuc_SanPham] FOREIGN KEY ([sanPhamId]) REFERENCES [core].[SanPham] ([id])
);

CREATE TABLE [core].[SanPhamDonVi] (
    [sanPhamId] nvarchar(50) NOT NULL,
    [donViId] nvarchar(50) NOT NULL,
    [id] nvarchar(50) NOT NULL,
    [heSoQuyDoi] decimal(18,4) NOT NULL,
    [giaBan] decimal(18,2) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_SanPhamDonVi] PRIMARY KEY ([sanPhamId], [donViId]),
    CONSTRAINT [AK_SanPhamDonVi_id] UNIQUE ([id]),
    CONSTRAINT [FK_SanPhamDonVi_DonVi] FOREIGN KEY ([donViId]) REFERENCES [core].[DonViDoLuong] ([id]),
    CONSTRAINT [FK_SanPhamDonVi_SanPham] FOREIGN KEY ([sanPhamId]) REFERENCES [core].[SanPham] ([id])
);

CREATE TABLE [core].[DonGiaoHang] (
    [id] nvarchar(50) NOT NULL,
    [hoaDonId] nvarchar(50) NOT NULL,
    [shipperId] nvarchar(50) NOT NULL,
    [phiVanChuyenId] nvarchar(50) NOT NULL,
    [ngayGiao] date NOT NULL,
    [trangThaiHienTai] nvarchar(20) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_DGH] PRIMARY KEY ([id]),
    CONSTRAINT [FK_DGH_HD] FOREIGN KEY ([hoaDonId]) REFERENCES [core].[HoaDon] ([id]),
    CONSTRAINT [FK_DGH_PVC] FOREIGN KEY ([phiVanChuyenId]) REFERENCES [core].[PhiVanChuyen] ([id]),
    CONSTRAINT [FK_DGH_SP] FOREIGN KEY ([shipperId]) REFERENCES [core].[Shipper] ([id])
);

CREATE TABLE [core].[DonHangOnline] (
    [id] nvarchar(50) NOT NULL,
    [hoaDonId] nvarchar(50) NOT NULL,
    [khachHangId] nvarchar(50) NOT NULL,
    [kenhDat] nvarchar(20) NOT NULL,
    [ngayDat] date NOT NULL,
    [tongTien] decimal(18,2) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__DonHangO__3213E83FAA6F60FF] PRIMARY KEY ([id]),
    CONSTRAINT [FK_DHO_HD] FOREIGN KEY ([hoaDonId]) REFERENCES [core].[HoaDon] ([id]),
    CONSTRAINT [FK_DHO_KH] FOREIGN KEY ([khachHangId]) REFERENCES [core].[KhachHang] ([id])
);

CREATE TABLE [core].[GiaoDichThanhToan] (
    [id] nvarchar(50) NOT NULL,
    [hoaDonId] nvarchar(50) NOT NULL,
    [soTien] decimal(18,2) NOT NULL,
    [ngayGD] date NOT NULL,
    [kenhThanhToanId] nvarchar(50) NOT NULL,
    [moTa] nvarchar(500) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__GiaoDich__3213E83F603FDFFA] PRIMARY KEY ([id]),
    CONSTRAINT [FK_GDTT_HD] FOREIGN KEY ([hoaDonId]) REFERENCES [core].[HoaDon] ([id]),
    CONSTRAINT [FK_GDTT_KTT] FOREIGN KEY ([kenhThanhToanId]) REFERENCES [core].[KenhThanhToan] ([id])
);

CREATE TABLE [core].[LichSuMuaHang] (
    [khachHangId] nvarchar(50) NOT NULL,
    [hoaDonId] nvarchar(50) NOT NULL,
    [tongTien] decimal(18,2) NOT NULL,
    [ngayMua] date NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_LSMH] PRIMARY KEY ([khachHangId], [hoaDonId]),
    CONSTRAINT [FK_LSMH_HD] FOREIGN KEY ([hoaDonId]) REFERENCES [core].[HoaDon] ([id]),
    CONSTRAINT [FK_LSMH_KH] FOREIGN KEY ([khachHangId]) REFERENCES [core].[KhachHang] ([id])
);

CREATE TABLE [core].[BaoCaoTonKho] (
    [id] nvarchar(50) NOT NULL,
    [baoCaoId] nvarchar(50) NOT NULL,
    [isDelete] bit NOT NULL,
    [sanPhamDonViId] nvarchar(50) NOT NULL,
    [tonDauKy] int NOT NULL,
    [nhapTrongKy] int NOT NULL,
    [xuatTrongKy] int NOT NULL,
    [tonCuoiKy] int NOT NULL,
    CONSTRAINT [PK__BaoCaoTo__3213E83F284EB6E8] PRIMARY KEY ([id]),
    CONSTRAINT [FK_BCTK_BC] FOREIGN KEY ([baoCaoId]) REFERENCES [core].[BaoCao] ([id]),
    CONSTRAINT [FK_BCTK_SPDV] FOREIGN KEY ([sanPhamDonViId]) REFERENCES [core].[SanPhamDonVi] ([id])
);

CREATE TABLE [core].[ChiTietGiaoDichNCC] (
    [giaoDichId] nvarchar(50) NOT NULL,
    [sanPhamDonViId] nvarchar(50) NOT NULL,
    [soLuong] int NOT NULL,
    [donGia] decimal(18,2) NOT NULL,
    [thanhTien] decimal(18,2) NULL DEFAULT 0.0,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_CTGD_NCC] PRIMARY KEY ([giaoDichId], [sanPhamDonViId]),
    CONSTRAINT [FK_CTGD_LSGD] FOREIGN KEY ([giaoDichId]) REFERENCES [core].[LichSuGiaoDich] ([id]),
    CONSTRAINT [FK_CTGD_SPDV] FOREIGN KEY ([sanPhamDonViId]) REFERENCES [core].[SanPhamDonVi] ([id])
);

CREATE TABLE [core].[ChiTietHoaDon] (
    [hoaDonId] nvarchar(50) NOT NULL,
    [sanPhamDonViId] nvarchar(50) NOT NULL,
    [soLuong] int NOT NULL,
    [donGia] decimal(18,2) NOT NULL,
    [giamGia] decimal(18,2) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_ChiTietHoaDon] PRIMARY KEY ([hoaDonId], [sanPhamDonViId]),
    CONSTRAINT [FK_CTHD_HoaDon] FOREIGN KEY ([hoaDonId]) REFERENCES [core].[HoaDon] ([id]),
    CONSTRAINT [FK_CTHD_SPDV] FOREIGN KEY ([sanPhamDonViId]) REFERENCES [core].[SanPhamDonVi] ([id])
);

CREATE TABLE [core].[ChiTietHoaDonKhuyenMai] (
    [hoaDonId] nvarchar(50) NOT NULL,
    [sanPhamDonViId] nvarchar(50) NOT NULL,
    [maKhuyenMaiId] nvarchar(50) NOT NULL,
    [giaTriApDung] decimal(18,2) NOT NULL,
    [id] nvarchar(50) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_CTHDKM] PRIMARY KEY ([hoaDonId], [sanPhamDonViId], [maKhuyenMaiId]),
    CONSTRAINT [FK_CTHDKM_HD] FOREIGN KEY ([hoaDonId]) REFERENCES [core].[HoaDon] ([id]),
    CONSTRAINT [FK_CTHDKM_MKM] FOREIGN KEY ([maKhuyenMaiId]) REFERENCES [core].[MaKhuyenMai] ([id]),
    CONSTRAINT [FK_CTHDKM_SPDV] FOREIGN KEY ([sanPhamDonViId]) REFERENCES [core].[SanPhamDonVi] ([id])
);

CREATE TABLE [core].[ChiTietPhieuNhap] (
    [phieuNhapId] nvarchar(50) NOT NULL,
    [sanPhamDonViId] nvarchar(50) NOT NULL,
    [soLuong] int NOT NULL,
    [donGia] decimal(18,2) NOT NULL,
    [hanSuDung] date NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_CTPN] PRIMARY KEY ([phieuNhapId], [sanPhamDonViId]),
    CONSTRAINT [FK_CTPN_PN] FOREIGN KEY ([phieuNhapId]) REFERENCES [core].[PhieuNhap] ([id]),
    CONSTRAINT [FK_CTPN_SPDV] FOREIGN KEY ([sanPhamDonViId]) REFERENCES [core].[SanPhamDonVi] ([id])
);

CREATE TABLE [core].[ChiTietPhieuXuat] (
    [phieuXuatId] nvarchar(50) NOT NULL,
    [sanPhamDonViId] nvarchar(50) NOT NULL,
    [soLuong] int NOT NULL,
    [donGia] decimal(18,2) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_CTPX] PRIMARY KEY ([phieuXuatId], [sanPhamDonViId]),
    CONSTRAINT [FK_CTPX_PX] FOREIGN KEY ([phieuXuatId]) REFERENCES [core].[PhieuXuat] ([id]),
    CONSTRAINT [FK_CTPX_SPDV] FOREIGN KEY ([sanPhamDonViId]) REFERENCES [core].[SanPhamDonVi] ([id])
);

CREATE TABLE [core].[GioHang] (
    [taiKhoanId] nvarchar(50) NOT NULL,
    [sanPhamDonViId] nvarchar(50) NOT NULL,
    [soLuong] int NOT NULL,
    [isDelete] bit NOT NULL,
    [createdAt] datetime NOT NULL DEFAULT ((getdate())),
    [updatedAt] datetime NOT NULL DEFAULT ((getdate())),
    CONSTRAINT [PK_GioHang] PRIMARY KEY ([taiKhoanId], [sanPhamDonViId]),
    CONSTRAINT [FK_GioHang_SanPhamDonVi] FOREIGN KEY ([sanPhamDonViId]) REFERENCES [core].[SanPhamDonVi] ([id]),
    CONSTRAINT [FK_GioHang_TaiKhoan] FOREIGN KEY ([taiKhoanId]) REFERENCES [core].[TaiKhoan] ([id])
);

CREATE TABLE [core].[KiemKe] (
    [id] nvarchar(50) NOT NULL,
    [ngayKiemKe] date NOT NULL,
    [ketQua] nvarchar(500) NOT NULL,
    [nhanVienId] nvarchar(50) NOT NULL,
    [isDelete] bit NOT NULL,
    [sanPhamDonViID] nvarchar(50) NULL,
    CONSTRAINT [PK__KiemKe__3213E83F4355DB52] PRIMARY KEY ([id]),
    CONSTRAINT [FK_KiemKe_NV] FOREIGN KEY ([nhanVienId]) REFERENCES [core].[NhanVien] ([id]),
    CONSTRAINT [FK_KiemKe_SP] FOREIGN KEY ([sanPhamDonViID]) REFERENCES [core].[SanPhamDonVi] ([id])
);

CREATE TABLE [core].[MaDinhDanhSanPham] (
    [id] nvarchar(50) NOT NULL,
    [sanPhamDonViId] nvarchar(50) NOT NULL,
    [loaiMa] nvarchar(10) NOT NULL,
    [maCode] nvarchar(200) NOT NULL,
    [duongDan] nvarchar(500) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__MaDinhDa__3213E83FB8B9410D] PRIMARY KEY ([id]),
    CONSTRAINT [FK_MDD_SPDV] FOREIGN KEY ([sanPhamDonViId]) REFERENCES [core].[SanPhamDonVi] ([id])
);

CREATE TABLE [core].[PhieuDoiTra] (
    [id] nvarchar(50) NOT NULL,
    [hoaDonId] nvarchar(50) NOT NULL,
    [sanPhamDonViId] nvarchar(50) NOT NULL,
    [ngayDoiTra] date NOT NULL,
    [lyDo] nvarchar(500) NOT NULL,
    [soTienHoan] decimal(18,2) NOT NULL,
    [chinhSachId] nvarchar(50) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__PhieuDoi__3213E83FF7A1E82D] PRIMARY KEY ([id]),
    CONSTRAINT [FK_PDT_CS] FOREIGN KEY ([chinhSachId]) REFERENCES [core].[ChinhSachHoanTra] ([id]),
    CONSTRAINT [FK_PDT_HD] FOREIGN KEY ([hoaDonId]) REFERENCES [core].[HoaDon] ([id]),
    CONSTRAINT [FK_PDT_SPDV] FOREIGN KEY ([sanPhamDonViId]) REFERENCES [core].[SanPhamDonVi] ([id])
);

CREATE TABLE [core].[SanPhamViTri] (
    [sanPhamDonViId] nvarchar(50) NOT NULL,
    [viTriId] nvarchar(50) NOT NULL,
    [soLuong] int NOT NULL,
    [id] nvarchar(50) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_SPVT] PRIMARY KEY ([sanPhamDonViId], [viTriId]),
    CONSTRAINT [FK_SPVT_SPDV] FOREIGN KEY ([sanPhamDonViId]) REFERENCES [core].[SanPhamDonVi] ([id]),
    CONSTRAINT [FK_SPVT_ViTri] FOREIGN KEY ([viTriId]) REFERENCES [core].[ViTri] ([id])
);

CREATE TABLE [core].[TonKho] (
    [id] nvarchar(50) NOT NULL,
    [sanPhamDonViId] nvarchar(50) NOT NULL,
    [soLuongTon] int NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__TonKho__3213E83FE16F2840] PRIMARY KEY ([id]),
    CONSTRAINT [FK_TonKho_SPDV] FOREIGN KEY ([sanPhamDonViId]) REFERENCES [core].[SanPhamDonVi] ([id])
);

CREATE TABLE [core].[TrangThaiGiaoHang] (
    [id] nvarchar(50) NOT NULL,
    [donGiaoHangId] nvarchar(50) NOT NULL,
    [trangThai] nvarchar(20) NOT NULL,
    [ngayCapNhat] datetime2(0) NOT NULL,
    [ghiChu] nvarchar(500) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__TrangTha__3213E83F4A6FED2F] PRIMARY KEY ([id]),
    CONSTRAINT [FK_TTGH_DGH] FOREIGN KEY ([donGiaoHangId]) REFERENCES [core].[DonGiaoHang] ([id])
);

CREATE TABLE [core].[ChiTietDonOnline] (
    [donHangId] nvarchar(50) NOT NULL,
    [sanPhamDonViId] nvarchar(50) NOT NULL,
    [soLuong] int NOT NULL,
    [donGia] decimal(18,2) NOT NULL,
    [id] nvarchar(50) NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK_CTDON] PRIMARY KEY ([donHangId], [sanPhamDonViId]),
    CONSTRAINT [FK_CTDON_DH] FOREIGN KEY ([donHangId]) REFERENCES [core].[DonHangOnline] ([id]),
    CONSTRAINT [FK_CTDON_SPDV] FOREIGN KEY ([sanPhamDonViId]) REFERENCES [core].[SanPhamDonVi] ([id])
);

CREATE TABLE [core].[TrangThaiXuLy] (
    [id] nvarchar(50) NOT NULL,
    [donHangId] nvarchar(50) NOT NULL,
    [trangThai] nvarchar(20) NOT NULL,
    [ngayCapNhat] datetime2(0) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__TrangTha__3213E83FF8C7651F] PRIMARY KEY ([id]),
    CONSTRAINT [FK_TTXL_DH] FOREIGN KEY ([donHangId]) REFERENCES [core].[DonHangOnline] ([id])
);

CREATE TABLE [core].[Barcode] (
    [id] nvarchar(50) NOT NULL,
    [maDinhDanhId] nvarchar(50) NOT NULL,
    [barcodeImage] nvarchar(500) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__Barcode__3213E83F1D9006F9] PRIMARY KEY ([id]),
    CONSTRAINT [FK_Bar_MDD] FOREIGN KEY ([maDinhDanhId]) REFERENCES [core].[MaDinhDanhSanPham] ([id])
);

CREATE TABLE [core].[QRCode] (
    [id] nvarchar(50) NOT NULL,
    [maDinhDanhId] nvarchar(50) NOT NULL,
    [qrCodeImage] nvarchar(500) NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__QRCode__3213E83FF13BADA6] PRIMARY KEY ([id]),
    CONSTRAINT [FK_QR_MDD] FOREIGN KEY ([maDinhDanhId]) REFERENCES [core].[MaDinhDanhSanPham] ([id])
);

CREATE TABLE [core].[TemNhan] (
    [id] nvarchar(50) NOT NULL,
    [maDinhDanhId] nvarchar(50) NOT NULL,
    [noiDungTem] nvarchar(max) NOT NULL,
    [ngayIn] date NOT NULL,
    [isDelete] bit NOT NULL,
    CONSTRAINT [PK__TemNhan__3213E83F9EB50D82] PRIMARY KEY ([id]),
    CONSTRAINT [FK_Tem_MDD] FOREIGN KEY ([maDinhDanhId]) REFERENCES [core].[MaDinhDanhSanPham] ([id])
);

CREATE INDEX [IX_BaoCaoBanChay_sanPhamId] ON [core].[BaoCaoBanChay] ([sanPhamId]);

CREATE UNIQUE INDEX [UQ__BaoCaoBa__3213E83E93C476A5] ON [core].[BaoCaoBanChay] ([id]) WHERE ([id] IS NOT NULL);

CREATE INDEX [IX_BaoCaoDoanhThu_baoCaoId] ON [core].[BaoCaoDoanhThu] ([baoCaoId]);

CREATE INDEX [IX_BaoCaoTonKho_baoCaoId] ON [core].[BaoCaoTonKho] ([baoCaoId]);

CREATE INDEX [IX_BaoCaoTonKho_sanPhamDonViId] ON [core].[BaoCaoTonKho] ([sanPhamDonViId]);

CREATE INDEX [IX_Barcode_maDinhDanhId] ON [core].[Barcode] ([maDinhDanhId]);

CREATE INDEX [IX_ChamCong_nhanVienId] ON [core].[ChamCong] ([nhanVienId]);

CREATE INDEX [IX_ChinhSachHoanTra_DanhMuc_danhMucId] ON [core].[ChinhSachHoanTra_DanhMuc] ([danhMucId]);

CREATE UNIQUE INDEX [UQ__ChinhSac__3213E83ED81B46B2] ON [core].[ChinhSachHoanTra_DanhMuc] ([id]) WHERE ([id] IS NOT NULL);

CREATE INDEX [IX_ChiTietDonOnline_sanPhamDonViId] ON [core].[ChiTietDonOnline] ([sanPhamDonViId]);

CREATE UNIQUE INDEX [UQ__ChiTietD__3213E83EC2F0D741] ON [core].[ChiTietDonOnline] ([id]) WHERE ([id] IS NOT NULL);

CREATE INDEX [IX_ChiTietGiaoDichNCC_sanPhamDonViId] ON [core].[ChiTietGiaoDichNCC] ([sanPhamDonViId]);

CREATE INDEX [IX_ChiTietHoaDon_sanPhamDonViId] ON [core].[ChiTietHoaDon] ([sanPhamDonViId]);

CREATE INDEX [IX_CTHD_HD] ON [core].[ChiTietHoaDon] ([hoaDonId]);

CREATE INDEX [IX_ChiTietHoaDonKhuyenMai_maKhuyenMaiId] ON [core].[ChiTietHoaDonKhuyenMai] ([maKhuyenMaiId]);

CREATE INDEX [IX_ChiTietHoaDonKhuyenMai_sanPhamDonViId] ON [core].[ChiTietHoaDonKhuyenMai] ([sanPhamDonViId]);

CREATE UNIQUE INDEX [UQ__ChiTietH__3213E83E64CDB278] ON [core].[ChiTietHoaDonKhuyenMai] ([id]) WHERE ([id] IS NOT NULL);

CREATE INDEX [IX_ChiTietPhieuNhap_sanPhamDonViId] ON [core].[ChiTietPhieuNhap] ([sanPhamDonViId]);

CREATE INDEX [IX_ChiTietPhieuXuat_sanPhamDonViId] ON [core].[ChiTietPhieuXuat] ([sanPhamDonViId]);

CREATE INDEX [IX_DieuKienApDung_chuongTrinhId] ON [core].[DieuKienApDung] ([chuongTrinhId]);

CREATE INDEX [IX_DieuKienApDungDanhMuc_danhMucId] ON [core].[DieuKienApDungDanhMuc] ([danhMucId]);

CREATE INDEX [IX_DieuKienApDungDanhMuc_dieuKienId] ON [core].[DieuKienApDungDanhMuc] ([dieuKienId]);

CREATE INDEX [IX_DieuKienApDungSanPham_dieuKienId] ON [core].[DieuKienApDungSanPham] ([dieuKienId]);

CREATE INDEX [IX_DieuKienApDungSanPham_sanPhamId] ON [core].[DieuKienApDungSanPham] ([sanPhamId]);

CREATE INDEX [IX_DieuKienApDungToanBo_dieuKienId] ON [core].[DieuKienApDungToanBo] ([dieuKienId]);

CREATE INDEX [IX_DonGiaoHang_hoaDonId] ON [core].[DonGiaoHang] ([hoaDonId]);

CREATE INDEX [IX_DonGiaoHang_phiVanChuyenId] ON [core].[DonGiaoHang] ([phiVanChuyenId]);

CREATE INDEX [IX_DonGiaoHang_shipperId] ON [core].[DonGiaoHang] ([shipperId]);

CREATE UNIQUE INDEX [UQ__tmp_ms_x__3213E83EEE107A4B] ON [core].[DonGiaoHang] ([id]);

CREATE INDEX [IX_DHO_HD] ON [core].[DonHangOnline] ([hoaDonId]);

CREATE INDEX [IX_DonHangOnline_khachHangId] ON [core].[DonHangOnline] ([khachHangId]);

CREATE UNIQUE INDEX [UQ__DonHangO__4643563BBFAC1AA5] ON [core].[DonHangOnline] ([hoaDonId]);

CREATE INDEX [IX_GiaoDichThanhToan_hoaDonId] ON [core].[GiaoDichThanhToan] ([hoaDonId]);

CREATE INDEX [IX_GiaoDichThanhToan_kenhThanhToanId] ON [core].[GiaoDichThanhToan] ([kenhThanhToanId]);

CREATE INDEX [IX_GioHang_sanPhamDonViId] ON [core].[GioHang] ([sanPhamDonViId]);

CREATE INDEX [IX_HD_khach] ON [core].[HoaDon] ([khachHangId]);

CREATE INDEX [IX_HoaDon_nhanVienId] ON [core].[HoaDon] ([nhanVienId]);

CREATE UNIQUE INDEX [Index_KhachHang_1] ON [core].[KhachHang] ([soDienThoai]);

CREATE UNIQUE INDEX [Index_KhachHang_2] ON [core].[KhachHang] ([email]) WHERE [email] IS NOT NULL;

CREATE INDEX [IX_KiemKe_nhanVienId] ON [core].[KiemKe] ([nhanVienId]);

CREATE INDEX [IX_KiemKe_sanPhamDonViID] ON [core].[KiemKe] ([sanPhamDonViID]);

CREATE INDEX [IX_LichSuGiaBan_donViId] ON [core].[LichSuGiaBan] ([donViId]);

CREATE INDEX [IX_LichSuGiaBan_sanPhamId] ON [core].[LichSuGiaBan] ([sanPhamId]);

CREATE INDEX [IX_LichSuGiaoDich_nhaCungCapId] ON [core].[LichSuGiaoDich] ([nhaCungCapId]);

CREATE INDEX [IX_LichSuMuaHang_hoaDonId] ON [core].[LichSuMuaHang] ([hoaDonId]);

CREATE INDEX [IX_MaDinhDanhSanPham_sanPhamDonViId] ON [core].[MaDinhDanhSanPham] ([sanPhamDonViId]);

CREATE INDEX [IX_MaKhuyenMai_chuongTrinhId] ON [core].[MaKhuyenMai] ([chuongTrinhId]);

CREATE UNIQUE INDEX [UQ__MaKhuyen__357D4CF9340F2F1C] ON [core].[MaKhuyenMai] ([code]);

CREATE UNIQUE INDEX [Index_NhaCungCap_1] ON [core].[NhaCungCap] ([soDienThoai]);

CREATE UNIQUE INDEX [Index_NhaCungCap_2] ON [core].[NhaCungCap] ([email]) WHERE [email] IS NOT NULL;

CREATE INDEX [IX_NhatKyHoatDong_taiKhoanId] ON [core].[NhatKyHoatDong] ([taiKhoanId]);

CREATE UNIQUE INDEX [UQ__Permissi__357D4CF9AB825447] ON [core].[Permission] ([code]);

CREATE INDEX [IX_PhanCongCaLamViec_caLamViecId] ON [core].[PhanCongCaLamViec] ([caLamViecId]);

CREATE UNIQUE INDEX [UQ__PhanCong__3213E83E23C543A0] ON [core].[PhanCongCaLamViec] ([id]) WHERE ([id] IS NOT NULL);

CREATE INDEX [IX_PhieuDoiTra_chinhSachId] ON [core].[PhieuDoiTra] ([chinhSachId]);

CREATE INDEX [IX_PhieuDoiTra_hoaDonId] ON [core].[PhieuDoiTra] ([hoaDonId]);

CREATE INDEX [IX_PhieuDoiTra_sanPhamDonViId] ON [core].[PhieuDoiTra] ([sanPhamDonViId]);

CREATE INDEX [IX_PhieuNhap_nhaCungCapId] ON [core].[PhieuNhap] ([nhaCungCapId]);

CREATE INDEX [IX_PhieuNhap_nhanVienId] ON [core].[PhieuNhap] ([nhanVienId]);

CREATE INDEX [IX_PhieuXuat_khachHangId] ON [core].[PhieuXuat] ([khachHangId]);

CREATE INDEX [IX_PhieuXuat_nhanVienId] ON [core].[PhieuXuat] ([nhanVienId]);

CREATE INDEX [IX_QRCode_maDinhDanhId] ON [core].[QRCode] ([maDinhDanhId]);

CREATE UNIQUE INDEX [UQ__Role__357D4CF9DD99A6B0] ON [core].[Role] ([code]);

CREATE INDEX [IX_RolePermission_permissionId] ON [core].[RolePermission] ([permissionId]);

CREATE UNIQUE INDEX [UQ__RolePerm__3213E83EEEFE6DC5] ON [core].[RolePermission] ([id]) WHERE ([id] IS NOT NULL);

CREATE INDEX [IX_SP_nhanHieu] ON [core].[SanPham] ([nhanHieuId]);

CREATE INDEX [IX_SanPhamDanhMuc_danhMucId] ON [core].[SanPhamDanhMuc] ([danhMucId]);

CREATE UNIQUE INDEX [UQ__SanPhamD__3213E83E62FEA25D] ON [core].[SanPhamDanhMuc] ([id]) WHERE ([id] IS NOT NULL);

CREATE INDEX [IX_SanPhamDonVi_donViId] ON [core].[SanPhamDonVi] ([donViId]);

CREATE UNIQUE INDEX [UQ__SanPhamD__3213E83ECC46A234] ON [core].[SanPhamDonVi] ([id]);

CREATE INDEX [IX_SanPhamViTri_viTriId] ON [core].[SanPhamViTri] ([viTriId]);

CREATE UNIQUE INDEX [UQ__SanPhamV__3213E83EB9656589] ON [core].[SanPhamViTri] ([id]) WHERE ([id] IS NOT NULL);

CREATE UNIQUE INDEX [Index_Shipper_1] ON [core].[Shipper] ([soDienThoai]);

CREATE UNIQUE INDEX [UQ__tmp_ms_x__701B5126D1F8EDF3] ON [core].[TaiKhoanKhachHang] ([taiKhoanid]);

CREATE UNIQUE INDEX [UQ__tmp_ms_x__701A5D1E31FA1692] ON [core].[TaiKhoanNhanVien] ([taiKhoanId]);

CREATE INDEX [IX_TemNhan_maDinhDanhId] ON [core].[TemNhan] ([maDinhDanhId]);

CREATE INDEX [IX_TheThanhVien_khachHangId] ON [core].[TheThanhVien] ([khachHangId]);

CREATE INDEX [IX_TonKho_sanPhamDonViId] ON [core].[TonKho] ([sanPhamDonViId]);

CREATE INDEX [IX_TrangThaiGiaoHang_donGiaoHangId] ON [core].[TrangThaiGiaoHang] ([donGiaoHangId]);

CREATE INDEX [IX_TrangThaiXuLy_donHangId] ON [core].[TrangThaiXuLy] ([donHangId]);

CREATE INDEX [IX_UserRole_roleId] ON [core].[UserRole] ([roleId]);

CREATE UNIQUE INDEX [UQ__UserRole__3213E83E44209D3B] ON [core].[UserRole] ([id]) WHERE ([id] IS NOT NULL);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251110111650_create_database', N'9.0.10');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251110140134_v4', N'9.0.10');

COMMIT;
GO


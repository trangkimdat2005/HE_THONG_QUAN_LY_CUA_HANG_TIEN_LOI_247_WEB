using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Migrations
{
    /// <inheritdoc />
    public partial class v0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "core");

            migrationBuilder.CreateTable(
                name: "BaoCao",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    loaiBaoCao = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ngayLap = table.Column<DateOnly>(type: "date", nullable: false),
                    fileBaoCao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BaoCao__3213E83FE22F5722", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CaLamViec",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    tenCa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    thoiGianBatDau = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false),
                    thoiGianKetThuc = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CaLamVie__3213E83FCF27A89E", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ChinhSachHoanTra",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    tenChinhSach = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    thoiHan = table.Column<int>(type: "int", nullable: false),
                    dieuKien = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    apDungToanBo = table.Column<bool>(type: "bit", nullable: false),
                    apDungTuNgay = table.Column<DateOnly>(type: "date", nullable: false),
                    apDungDenNgay = table.Column<DateOnly>(type: "date", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChinhSac__3213E83FD509BE56", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ChuongTrinhKhuyenMai",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ten = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    loai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ngayBatDau = table.Column<DateOnly>(type: "date", nullable: false),
                    ngayKetThuc = table.Column<DateOnly>(type: "date", nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChuongTr__3213E83F918CF90D", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DanhMuc",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ten = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DanhMuc__3213E83FECBD45A0", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DonViDoLuong",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ten = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    kyHieu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonViDoL__3213E83F05CA6401", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "HinhAnh",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenAnh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Anh = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnh", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KenhThanhToan",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    tenKenh = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    loaiKenh = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    phiGiaoDich = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active"),
                    cauHinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    updatedAt = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KenhThan__3213E83F2F0444F7", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hoTen = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    soDienThoai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    diaChi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ngayDangKy = table.Column<DateOnly>(type: "date", nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false),
                    gioiTinh = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KhachHan__3213E83F168CE425", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCap",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ten = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    soDienThoai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    diaChi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    maSoThue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhaCungC__3213E83F2D28D041", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "NhanHieu",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ten = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhanHieu__3213E83FB4538AA9", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hoTen = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    chucVu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    luongCoBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    soDienThoai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    diaChi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ngayVaoLam = table.Column<DateOnly>(type: "date", nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false),
                    gioiTinh = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhanVien__3213E83F4D14CBFC", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ten = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Permissi__3213E83FA402C283", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PhiVanChuyen",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    phuongThucTinh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhiVanCh__3213E83F88433FDE", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ten = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active"),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__3213E83F6D7FA3BA", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Shipper",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hoTen = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    soDienThoai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shipper__3213E83FC4207263", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    tenDangNhap = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    matKhauHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TaiKhoan__3213E83F8B2017A0", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ViTri",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    maViTri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    loaiViTri = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ViTri__3213E83FC6CB7FE6", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BaoCaoDoanhThu",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    baoCaoId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    tongDoanhThu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    kyBaoCao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BaoCaoDo__3213E83F0D878F66", x => x.id);
                    table.ForeignKey(
                        name: "FK_BCDT_BC",
                        column: x => x.baoCaoId,
                        principalSchema: "core",
                        principalTable: "BaoCao",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "BaoCaoTonKho",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    baoCaoId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    tongSoLuongTon = table.Column<int>(type: "int", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BaoCaoTo__3213E83F284EB6E8", x => x.id);
                    table.ForeignKey(
                        name: "FK_BCTK_BC",
                        column: x => x.baoCaoId,
                        principalSchema: "core",
                        principalTable: "BaoCao",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DieuKienApDung",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    chuongTrinhId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dieuKien = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    giaTriToiThieu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    giamTheo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    giaTriToiDa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DieuKien__3213E83F9398724C", x => x.id);
                    table.ForeignKey(
                        name: "FK_DKAD_CTKM",
                        column: x => x.chuongTrinhId,
                        principalSchema: "core",
                        principalTable: "ChuongTrinhKhuyenMai",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "MaKhuyenMai",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    chuongTrinhId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    giaTri = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    soLanSuDung = table.Column<int>(type: "int", nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ngayBatDau = table.Column<DateOnly>(type: "date", nullable: false),
                    ngayKetThuc = table.Column<DateOnly>(type: "date", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MaKhuyen__3213E83FAAB31469", x => x.id);
                    table.ForeignKey(
                        name: "FK_MKM_CTKM",
                        column: x => x.chuongTrinhId,
                        principalSchema: "core",
                        principalTable: "ChuongTrinhKhuyenMai",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ChinhSachHoanTra_DanhMuc",
                schema: "core",
                columns: table => new
                {
                    chinhSachId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    danhMucId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSHTDM", x => new { x.chinhSachId, x.danhMucId });
                    table.ForeignKey(
                        name: "FK_CSHTDM_CS",
                        column: x => x.chinhSachId,
                        principalSchema: "core",
                        principalTable: "ChinhSachHoanTra",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CSHTDM_DM",
                        column: x => x.danhMucId,
                        principalSchema: "core",
                        principalTable: "DanhMuc",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TheThanhVien",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    khachHangId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    diemTichLuy = table.Column<int>(type: "int", nullable: false),
                    ngayCap = table.Column<DateOnly>(type: "date", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TheThanh__3213E83FCD40305D", x => x.id);
                    table.ForeignKey(
                        name: "FK_TTV_KH",
                        column: x => x.khachHangId,
                        principalSchema: "core",
                        principalTable: "KhachHang",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LichSuGiaoDich",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nhaCungCapId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ngayGD = table.Column<DateOnly>(type: "date", nullable: false),
                    tongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LichSuGi__3213E83FEE6A44AB", x => x.id);
                    table.ForeignKey(
                        name: "FK_LSGD_NCC",
                        column: x => x.nhaCungCapId,
                        principalSchema: "core",
                        principalTable: "NhaCungCap",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ten = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    nhanHieuId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SanPham__3213E83F9FAE08F2", x => x.id);
                    table.ForeignKey(
                        name: "FK_SanPham_NhanHieu",
                        column: x => x.nhanHieuId,
                        principalSchema: "core",
                        principalTable: "NhanHieu",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ChamCong",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nhanVienId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ngay = table.Column<DateOnly>(type: "date", nullable: false),
                    gioVao = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    gioRa = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    ghiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChamCong__3213E83FB805717D", x => x.id);
                    table.ForeignKey(
                        name: "FK_ChamCong_NV",
                        column: x => x.nhanVienId,
                        principalSchema: "core",
                        principalTable: "NhanVien",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ngayLap = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    tongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    nhanVienId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    khachHangId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HoaDon__3213E83F5212F956", x => x.id);
                    table.ForeignKey(
                        name: "FK_HoaDon_KhachHang",
                        column: x => x.khachHangId,
                        principalSchema: "core",
                        principalTable: "KhachHang",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_HoaDon_NhanVien",
                        column: x => x.nhanVienId,
                        principalSchema: "core",
                        principalTable: "NhanVien",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "KiemKe",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ngayKiemKe = table.Column<DateOnly>(type: "date", nullable: false),
                    ketQua = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    nhanVienId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KiemKe__3213E83F4355DB52", x => x.id);
                    table.ForeignKey(
                        name: "FK_KiemKe_NV",
                        column: x => x.nhanVienId,
                        principalSchema: "core",
                        principalTable: "NhanVien",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PhanCongCaLamViec",
                schema: "core",
                columns: table => new
                {
                    nhanVienId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    caLamViecId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ngay = table.Column<DateOnly>(type: "date", nullable: false),
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanCong", x => new { x.nhanVienId, x.caLamViecId, x.ngay });
                    table.ForeignKey(
                        name: "FK_PhanCong_Ca",
                        column: x => x.caLamViecId,
                        principalSchema: "core",
                        principalTable: "CaLamViec",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PhanCong_NV",
                        column: x => x.nhanVienId,
                        principalSchema: "core",
                        principalTable: "NhanVien",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PhieuNhap",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    nhaCungCapId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ngayNhap = table.Column<DateOnly>(type: "date", nullable: false),
                    tongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false),
                    nhanVienId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuNha__3213E83F632478F9", x => x.id);
                    table.ForeignKey(
                        name: "FK_PN_NCC",
                        column: x => x.nhaCungCapId,
                        principalSchema: "core",
                        principalTable: "NhaCungCap",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PN_NV",
                        column: x => x.nhanVienId,
                        principalSchema: "core",
                        principalTable: "NhanVien",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PhieuXuat",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    khachHangId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ngayXuat = table.Column<DateOnly>(type: "date", nullable: false),
                    tongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false),
                    nhanVienId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuXua__3213E83F3C8AF210", x => x.id);
                    table.ForeignKey(
                        name: "FK_PX_KH",
                        column: x => x.khachHangId,
                        principalSchema: "core",
                        principalTable: "KhachHang",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PX_NV",
                        column: x => x.nhanVienId,
                        principalSchema: "core",
                        principalTable: "NhanVien",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "core",
                columns: table => new
                {
                    roleId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    permissionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RP", x => new { x.roleId, x.permissionId });
                    table.ForeignKey(
                        name: "FK_RP_Permission",
                        column: x => x.permissionId,
                        principalSchema: "core",
                        principalTable: "Permission",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_RP_Role",
                        column: x => x.roleId,
                        principalSchema: "core",
                        principalTable: "Role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "NhatKyHoatDong",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    taiKhoanId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    thoiGian = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    hanhDong = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhatKyHo__3213E83F73810801", x => x.id);
                    table.ForeignKey(
                        name: "FK_NKHD_TK",
                        column: x => x.taiKhoanId,
                        principalSchema: "core",
                        principalTable: "TaiKhoan",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoanKhachHang",
                schema: "core",
                columns: table => new
                {
                    khachHangId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    taiKhoanid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TKKH", x => new { x.khachHangId, x.taiKhoanid });
                    table.ForeignKey(
                        name: "FK_TKKH_KH",
                        column: x => x.khachHangId,
                        principalSchema: "core",
                        principalTable: "KhachHang",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TKKH_TK",
                        column: x => x.taiKhoanid,
                        principalSchema: "core",
                        principalTable: "TaiKhoan",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoanNhanVien",
                schema: "core",
                columns: table => new
                {
                    nhanVienId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    taiKhoanId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TKNV", x => new { x.nhanVienId, x.taiKhoanId });
                    table.ForeignKey(
                        name: "FK_TKNV_NV",
                        column: x => x.nhanVienId,
                        principalSchema: "core",
                        principalTable: "NhanVien",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TKNV_TK",
                        column: x => x.taiKhoanId,
                        principalSchema: "core",
                        principalTable: "TaiKhoan",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "core",
                columns: table => new
                {
                    taiKhoanId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    roleId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hieuLucTu = table.Column<DateOnly>(type: "date", nullable: true),
                    hieuLucDen = table.Column<DateOnly>(type: "date", nullable: true),
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UR", x => new { x.taiKhoanId, x.roleId });
                    table.ForeignKey(
                        name: "FK_UR_Role",
                        column: x => x.roleId,
                        principalSchema: "core",
                        principalTable: "Role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_UR_TK",
                        column: x => x.taiKhoanId,
                        principalSchema: "core",
                        principalTable: "TaiKhoan",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DieuKienApDungDanhMuc",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dieuKienId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    danhMucId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DieuKien__3213E83F8C100A0B", x => x.id);
                    table.ForeignKey(
                        name: "FK_DKADDM_DK",
                        column: x => x.dieuKienId,
                        principalSchema: "core",
                        principalTable: "DieuKienApDung",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DKADDM_DM",
                        column: x => x.danhMucId,
                        principalSchema: "core",
                        principalTable: "DanhMuc",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DieuKienApDungToanBo",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dieuKienId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ghiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DieuKien__3213E83F8B829C4F", x => x.id);
                    table.ForeignKey(
                        name: "FK_DKADTB_DK",
                        column: x => x.dieuKienId,
                        principalSchema: "core",
                        principalTable: "DieuKienApDung",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "BaoCaoBanChay",
                schema: "core",
                columns: table => new
                {
                    baoCaoId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sanPhamId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soLuongBan = table.Column<int>(type: "int", nullable: false),
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BCBC", x => new { x.baoCaoId, x.sanPhamId });
                    table.ForeignKey(
                        name: "FK_BCBC_BC",
                        column: x => x.baoCaoId,
                        principalSchema: "core",
                        principalTable: "BaoCao",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_BCBC_SP",
                        column: x => x.sanPhamId,
                        principalSchema: "core",
                        principalTable: "SanPham",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DieuKienApDungSanPham",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dieuKienId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sanPhamId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DieuKien__3213E83F7827BCA6", x => x.id);
                    table.ForeignKey(
                        name: "FK_DKADSP_DK",
                        column: x => x.dieuKienId,
                        principalSchema: "core",
                        principalTable: "DieuKienApDung",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DKADSP_SP",
                        column: x => x.sanPhamId,
                        principalSchema: "core",
                        principalTable: "SanPham",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LichSuGiaBan",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sanPhamId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    donViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    giaBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ngayBatDau = table.Column<DateOnly>(type: "date", nullable: false),
                    ngayKetThuc = table.Column<DateOnly>(type: "date", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LichSuGi__3213E83F5BEC2A0A", x => x.id);
                    table.ForeignKey(
                        name: "FK_LSGB_DonVi",
                        column: x => x.donViId,
                        principalSchema: "core",
                        principalTable: "DonViDoLuong",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_LSGB_SanPham",
                        column: x => x.sanPhamId,
                        principalSchema: "core",
                        principalTable: "SanPham",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SanPhamDanhMuc",
                schema: "core",
                columns: table => new
                {
                    sanPhamId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    danhMucId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhamDanhMuc", x => new { x.sanPhamId, x.danhMucId });
                    table.ForeignKey(
                        name: "FK_SanPhamDanhMuc_DanhMuc",
                        column: x => x.danhMucId,
                        principalSchema: "core",
                        principalTable: "DanhMuc",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_SanPhamDanhMuc_SanPham",
                        column: x => x.sanPhamId,
                        principalSchema: "core",
                        principalTable: "SanPham",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SanPhamDonVi",
                schema: "core",
                columns: table => new
                {
                    sanPhamId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    donViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    heSoQuyDoi = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    giaBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhamDonVi", x => new { x.sanPhamId, x.donViId });
                    table.UniqueConstraint("AK_SanPhamDonVi_id", x => x.id);
                    table.ForeignKey(
                        name: "FK_SanPhamDonVi_DonVi",
                        column: x => x.donViId,
                        principalSchema: "core",
                        principalTable: "DonViDoLuong",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_SanPhamDonVi_SanPham",
                        column: x => x.sanPhamId,
                        principalSchema: "core",
                        principalTable: "SanPham",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DonGiaoHang",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hoaDonId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    shipperId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    phiVanChuyenId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ngayGiao = table.Column<DateOnly>(type: "date", nullable: false),
                    trangThaiHienTai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DGH", x => x.id);
                    table.ForeignKey(
                        name: "FK_DGH_HD",
                        column: x => x.hoaDonId,
                        principalSchema: "core",
                        principalTable: "HoaDon",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DGH_PVC",
                        column: x => x.phiVanChuyenId,
                        principalSchema: "core",
                        principalTable: "PhiVanChuyen",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DGH_SP",
                        column: x => x.shipperId,
                        principalSchema: "core",
                        principalTable: "Shipper",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DonHangOnline",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hoaDonId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    khachHangId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    kenhDat = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ngayDat = table.Column<DateOnly>(type: "date", nullable: false),
                    tongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonHangO__3213E83FAA6F60FF", x => x.id);
                    table.ForeignKey(
                        name: "FK_DHO_HD",
                        column: x => x.hoaDonId,
                        principalSchema: "core",
                        principalTable: "HoaDon",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DHO_KH",
                        column: x => x.khachHangId,
                        principalSchema: "core",
                        principalTable: "KhachHang",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "GiaoDichThanhToan",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hoaDonId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ngayGD = table.Column<DateOnly>(type: "date", nullable: false),
                    kenhThanhToanId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GiaoDich__3213E83F603FDFFA", x => x.id);
                    table.ForeignKey(
                        name: "FK_GDTT_HD",
                        column: x => x.hoaDonId,
                        principalSchema: "core",
                        principalTable: "HoaDon",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_GDTT_KTT",
                        column: x => x.kenhThanhToanId,
                        principalSchema: "core",
                        principalTable: "KenhThanhToan",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LichSuMuaHang",
                schema: "core",
                columns: table => new
                {
                    khachHangId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hoaDonId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    tongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ngayMua = table.Column<DateOnly>(type: "date", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LSMH", x => new { x.khachHangId, x.hoaDonId });
                    table.ForeignKey(
                        name: "FK_LSMH_HD",
                        column: x => x.hoaDonId,
                        principalSchema: "core",
                        principalTable: "HoaDon",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_LSMH_KH",
                        column: x => x.khachHangId,
                        principalSchema: "core",
                        principalTable: "KhachHang",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietGiaoDichNCC",
                schema: "core",
                columns: table => new
                {
                    giaoDichId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sanPhamDonViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soLuong = table.Column<int>(type: "int", nullable: false),
                    donGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    thanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTGD_NCC", x => new { x.giaoDichId, x.sanPhamDonViId });
                    table.ForeignKey(
                        name: "FK_CTGD_LSGD",
                        column: x => x.giaoDichId,
                        principalSchema: "core",
                        principalTable: "LichSuGiaoDich",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CTGD_SPDV",
                        column: x => x.sanPhamDonViId,
                        principalSchema: "core",
                        principalTable: "SanPhamDonVi",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDon",
                schema: "core",
                columns: table => new
                {
                    hoaDonId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sanPhamDonViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soLuong = table.Column<int>(type: "int", nullable: false),
                    donGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    giamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDon", x => new { x.hoaDonId, x.sanPhamDonViId });
                    table.ForeignKey(
                        name: "FK_CTHD_HoaDon",
                        column: x => x.hoaDonId,
                        principalSchema: "core",
                        principalTable: "HoaDon",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CTHD_SPDV",
                        column: x => x.sanPhamDonViId,
                        principalSchema: "core",
                        principalTable: "SanPhamDonVi",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDonKhuyenMai",
                schema: "core",
                columns: table => new
                {
                    hoaDonId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sanPhamDonViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    maKhuyenMaiId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    giaTriApDung = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTHDKM", x => new { x.hoaDonId, x.sanPhamDonViId, x.maKhuyenMaiId });
                    table.ForeignKey(
                        name: "FK_CTHDKM_HD",
                        column: x => x.hoaDonId,
                        principalSchema: "core",
                        principalTable: "HoaDon",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CTHDKM_MKM",
                        column: x => x.maKhuyenMaiId,
                        principalSchema: "core",
                        principalTable: "MaKhuyenMai",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CTHDKM_SPDV",
                        column: x => x.sanPhamDonViId,
                        principalSchema: "core",
                        principalTable: "SanPhamDonVi",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuNhap",
                schema: "core",
                columns: table => new
                {
                    phieuNhapId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sanPhamDonViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soLuong = table.Column<int>(type: "int", nullable: false),
                    donGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    hanSuDung = table.Column<DateOnly>(type: "date", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTPN", x => new { x.phieuNhapId, x.sanPhamDonViId });
                    table.ForeignKey(
                        name: "FK_CTPN_PN",
                        column: x => x.phieuNhapId,
                        principalSchema: "core",
                        principalTable: "PhieuNhap",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CTPN_SPDV",
                        column: x => x.sanPhamDonViId,
                        principalSchema: "core",
                        principalTable: "SanPhamDonVi",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuXuat",
                schema: "core",
                columns: table => new
                {
                    phieuXuatId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sanPhamDonViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soLuong = table.Column<int>(type: "int", nullable: false),
                    donGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTPX", x => new { x.phieuXuatId, x.sanPhamDonViId });
                    table.ForeignKey(
                        name: "FK_CTPX_PX",
                        column: x => x.phieuXuatId,
                        principalSchema: "core",
                        principalTable: "PhieuXuat",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CTPX_SPDV",
                        column: x => x.sanPhamDonViId,
                        principalSchema: "core",
                        principalTable: "SanPhamDonVi",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                schema: "core",
                columns: table => new
                {
                    taiKhoanId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sanPhamDonViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soLuong = table.Column<int>(type: "int", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => new { x.taiKhoanId, x.sanPhamDonViId });
                    table.ForeignKey(
                        name: "FK_GioHang_SanPhamDonVi",
                        column: x => x.sanPhamDonViId,
                        principalSchema: "core",
                        principalTable: "SanPhamDonVi",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_GioHang_TaiKhoan",
                        column: x => x.taiKhoanId,
                        principalSchema: "core",
                        principalTable: "TaiKhoan",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "MaDinhDanhSanPham",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sanPhamDonViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    loaiMa = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    maCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    duongDan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MaDinhDa__3213E83FB8B9410D", x => x.id);
                    table.ForeignKey(
                        name: "FK_MDD_SPDV",
                        column: x => x.sanPhamDonViId,
                        principalSchema: "core",
                        principalTable: "SanPhamDonVi",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PhieuDoiTra",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hoaDonId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sanPhamDonViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ngayDoiTra = table.Column<DateOnly>(type: "date", nullable: false),
                    lyDo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    soTienHoan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    chinhSachId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuDoi__3213E83FF7A1E82D", x => x.id);
                    table.ForeignKey(
                        name: "FK_PDT_CS",
                        column: x => x.chinhSachId,
                        principalSchema: "core",
                        principalTable: "ChinhSachHoanTra",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PDT_HD",
                        column: x => x.hoaDonId,
                        principalSchema: "core",
                        principalTable: "HoaDon",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PDT_SPDV",
                        column: x => x.sanPhamDonViId,
                        principalSchema: "core",
                        principalTable: "SanPhamDonVi",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SanPhamViTri",
                schema: "core",
                columns: table => new
                {
                    sanPhamDonViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    viTriId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soLuong = table.Column<int>(type: "int", nullable: false),
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPVT", x => new { x.sanPhamDonViId, x.viTriId });
                    table.ForeignKey(
                        name: "FK_SPVT_SPDV",
                        column: x => x.sanPhamDonViId,
                        principalSchema: "core",
                        principalTable: "SanPhamDonVi",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_SPVT_ViTri",
                        column: x => x.viTriId,
                        principalSchema: "core",
                        principalTable: "ViTri",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TonKho",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sanPhamDonViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soLuongTon = table.Column<int>(type: "int", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TonKho__3213E83FE16F2840", x => x.id);
                    table.ForeignKey(
                        name: "FK_TonKho_SPDV",
                        column: x => x.sanPhamDonViId,
                        principalSchema: "core",
                        principalTable: "SanPhamDonVi",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TrangThaiGiaoHang",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    donGiaoHangId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ngayCapNhat = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    ghiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TrangTha__3213E83F4A6FED2F", x => x.id);
                    table.ForeignKey(
                        name: "FK_TTGH_DGH",
                        column: x => x.donGiaoHangId,
                        principalSchema: "core",
                        principalTable: "DonGiaoHang",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonOnline",
                schema: "core",
                columns: table => new
                {
                    donHangId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sanPhamDonViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soLuong = table.Column<int>(type: "int", nullable: false),
                    donGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTDON", x => new { x.donHangId, x.sanPhamDonViId });
                    table.ForeignKey(
                        name: "FK_CTDON_DH",
                        column: x => x.donHangId,
                        principalSchema: "core",
                        principalTable: "DonHangOnline",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CTDON_SPDV",
                        column: x => x.sanPhamDonViId,
                        principalSchema: "core",
                        principalTable: "SanPhamDonVi",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TrangThaiXuLy",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    donHangId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ngayCapNhat = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TrangTha__3213E83FF8C7651F", x => x.id);
                    table.ForeignKey(
                        name: "FK_TTXL_DH",
                        column: x => x.donHangId,
                        principalSchema: "core",
                        principalTable: "DonHangOnline",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Barcode",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    maDinhDanhId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    barcodeImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Barcode__3213E83F1D9006F9", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bar_MDD",
                        column: x => x.maDinhDanhId,
                        principalSchema: "core",
                        principalTable: "MaDinhDanhSanPham",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "QRCode",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    maDinhDanhId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    qrCodeImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__QRCode__3213E83FF13BADA6", x => x.id);
                    table.ForeignKey(
                        name: "FK_QR_MDD",
                        column: x => x.maDinhDanhId,
                        principalSchema: "core",
                        principalTable: "MaDinhDanhSanPham",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TemNhan",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    maDinhDanhId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    noiDungTem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngayIn = table.Column<DateOnly>(type: "date", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TemNhan__3213E83F9EB50D82", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tem_MDD",
                        column: x => x.maDinhDanhId,
                        principalSchema: "core",
                        principalTable: "MaDinhDanhSanPham",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoBanChay_sanPhamId",
                schema: "core",
                table: "BaoCaoBanChay",
                column: "sanPhamId");

            migrationBuilder.CreateIndex(
                name: "UQ__BaoCaoBa__3213E83E93C476A5",
                schema: "core",
                table: "BaoCaoBanChay",
                column: "id",
                unique: true,
                filter: "([id] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoDoanhThu_baoCaoId",
                schema: "core",
                table: "BaoCaoDoanhThu",
                column: "baoCaoId");

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoTonKho_baoCaoId",
                schema: "core",
                table: "BaoCaoTonKho",
                column: "baoCaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Barcode_maDinhDanhId",
                schema: "core",
                table: "Barcode",
                column: "maDinhDanhId");

            migrationBuilder.CreateIndex(
                name: "IX_ChamCong_nhanVienId",
                schema: "core",
                table: "ChamCong",
                column: "nhanVienId");

            migrationBuilder.CreateIndex(
                name: "IX_ChinhSachHoanTra_DanhMuc_danhMucId",
                schema: "core",
                table: "ChinhSachHoanTra_DanhMuc",
                column: "danhMucId");

            migrationBuilder.CreateIndex(
                name: "UQ__ChinhSac__3213E83ED81B46B2",
                schema: "core",
                table: "ChinhSachHoanTra_DanhMuc",
                column: "id",
                unique: true,
                filter: "([id] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonOnline_sanPhamDonViId",
                schema: "core",
                table: "ChiTietDonOnline",
                column: "sanPhamDonViId");

            migrationBuilder.CreateIndex(
                name: "UQ__ChiTietD__3213E83EC2F0D741",
                schema: "core",
                table: "ChiTietDonOnline",
                column: "id",
                unique: true,
                filter: "([id] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGiaoDichNCC_sanPhamDonViId",
                schema: "core",
                table: "ChiTietGiaoDichNCC",
                column: "sanPhamDonViId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_sanPhamDonViId",
                schema: "core",
                table: "ChiTietHoaDon",
                column: "sanPhamDonViId");

            migrationBuilder.CreateIndex(
                name: "IX_CTHD_HD",
                schema: "core",
                table: "ChiTietHoaDon",
                column: "hoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonKhuyenMai_maKhuyenMaiId",
                schema: "core",
                table: "ChiTietHoaDonKhuyenMai",
                column: "maKhuyenMaiId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDonKhuyenMai_sanPhamDonViId",
                schema: "core",
                table: "ChiTietHoaDonKhuyenMai",
                column: "sanPhamDonViId");

            migrationBuilder.CreateIndex(
                name: "UQ__ChiTietH__3213E83E64CDB278",
                schema: "core",
                table: "ChiTietHoaDonKhuyenMai",
                column: "id",
                unique: true,
                filter: "([id] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuNhap_sanPhamDonViId",
                schema: "core",
                table: "ChiTietPhieuNhap",
                column: "sanPhamDonViId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuXuat_sanPhamDonViId",
                schema: "core",
                table: "ChiTietPhieuXuat",
                column: "sanPhamDonViId");

            migrationBuilder.CreateIndex(
                name: "IX_DieuKienApDung_chuongTrinhId",
                schema: "core",
                table: "DieuKienApDung",
                column: "chuongTrinhId");

            migrationBuilder.CreateIndex(
                name: "IX_DieuKienApDungDanhMuc_danhMucId",
                schema: "core",
                table: "DieuKienApDungDanhMuc",
                column: "danhMucId");

            migrationBuilder.CreateIndex(
                name: "IX_DieuKienApDungDanhMuc_dieuKienId",
                schema: "core",
                table: "DieuKienApDungDanhMuc",
                column: "dieuKienId");

            migrationBuilder.CreateIndex(
                name: "IX_DieuKienApDungSanPham_dieuKienId",
                schema: "core",
                table: "DieuKienApDungSanPham",
                column: "dieuKienId");

            migrationBuilder.CreateIndex(
                name: "IX_DieuKienApDungSanPham_sanPhamId",
                schema: "core",
                table: "DieuKienApDungSanPham",
                column: "sanPhamId");

            migrationBuilder.CreateIndex(
                name: "IX_DieuKienApDungToanBo_dieuKienId",
                schema: "core",
                table: "DieuKienApDungToanBo",
                column: "dieuKienId");

            migrationBuilder.CreateIndex(
                name: "IX_DonGiaoHang_hoaDonId",
                schema: "core",
                table: "DonGiaoHang",
                column: "hoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_DonGiaoHang_phiVanChuyenId",
                schema: "core",
                table: "DonGiaoHang",
                column: "phiVanChuyenId");

            migrationBuilder.CreateIndex(
                name: "IX_DonGiaoHang_shipperId",
                schema: "core",
                table: "DonGiaoHang",
                column: "shipperId");

            migrationBuilder.CreateIndex(
                name: "UQ__tmp_ms_x__3213E83EEE107A4B",
                schema: "core",
                table: "DonGiaoHang",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DHO_HD",
                schema: "core",
                table: "DonHangOnline",
                column: "hoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangOnline_khachHangId",
                schema: "core",
                table: "DonHangOnline",
                column: "khachHangId");

            migrationBuilder.CreateIndex(
                name: "UQ__DonHangO__4643563BBFAC1AA5",
                schema: "core",
                table: "DonHangOnline",
                column: "hoaDonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiaoDichThanhToan_hoaDonId",
                schema: "core",
                table: "GiaoDichThanhToan",
                column: "hoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_GiaoDichThanhToan_kenhThanhToanId",
                schema: "core",
                table: "GiaoDichThanhToan",
                column: "kenhThanhToanId");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_sanPhamDonViId",
                schema: "core",
                table: "GioHang",
                column: "sanPhamDonViId");

            migrationBuilder.CreateIndex(
                name: "IX_HD_khach",
                schema: "core",
                table: "HoaDon",
                column: "khachHangId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_nhanVienId",
                schema: "core",
                table: "HoaDon",
                column: "nhanVienId");

            migrationBuilder.CreateIndex(
                name: "IX_KiemKe_nhanVienId",
                schema: "core",
                table: "KiemKe",
                column: "nhanVienId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuGiaBan_donViId",
                schema: "core",
                table: "LichSuGiaBan",
                column: "donViId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuGiaBan_sanPhamId",
                schema: "core",
                table: "LichSuGiaBan",
                column: "sanPhamId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuGiaoDich_nhaCungCapId",
                schema: "core",
                table: "LichSuGiaoDich",
                column: "nhaCungCapId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuMuaHang_hoaDonId",
                schema: "core",
                table: "LichSuMuaHang",
                column: "hoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_MaDinhDanhSanPham_sanPhamDonViId",
                schema: "core",
                table: "MaDinhDanhSanPham",
                column: "sanPhamDonViId");

            migrationBuilder.CreateIndex(
                name: "IX_MaKhuyenMai_chuongTrinhId",
                schema: "core",
                table: "MaKhuyenMai",
                column: "chuongTrinhId");

            migrationBuilder.CreateIndex(
                name: "UQ__MaKhuyen__357D4CF9340F2F1C",
                schema: "core",
                table: "MaKhuyenMai",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NhatKyHoatDong_taiKhoanId",
                schema: "core",
                table: "NhatKyHoatDong",
                column: "taiKhoanId");

            migrationBuilder.CreateIndex(
                name: "UQ__Permissi__357D4CF9AB825447",
                schema: "core",
                table: "Permission",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhanCongCaLamViec_caLamViecId",
                schema: "core",
                table: "PhanCongCaLamViec",
                column: "caLamViecId");

            migrationBuilder.CreateIndex(
                name: "UQ__PhanCong__3213E83E23C543A0",
                schema: "core",
                table: "PhanCongCaLamViec",
                column: "id",
                unique: true,
                filter: "([id] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDoiTra_chinhSachId",
                schema: "core",
                table: "PhieuDoiTra",
                column: "chinhSachId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDoiTra_hoaDonId",
                schema: "core",
                table: "PhieuDoiTra",
                column: "hoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDoiTra_sanPhamDonViId",
                schema: "core",
                table: "PhieuDoiTra",
                column: "sanPhamDonViId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhap_nhaCungCapId",
                schema: "core",
                table: "PhieuNhap",
                column: "nhaCungCapId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhap_nhanVienId",
                schema: "core",
                table: "PhieuNhap",
                column: "nhanVienId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuXuat_khachHangId",
                schema: "core",
                table: "PhieuXuat",
                column: "khachHangId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuXuat_nhanVienId",
                schema: "core",
                table: "PhieuXuat",
                column: "nhanVienId");

            migrationBuilder.CreateIndex(
                name: "IX_QRCode_maDinhDanhId",
                schema: "core",
                table: "QRCode",
                column: "maDinhDanhId");

            migrationBuilder.CreateIndex(
                name: "UQ__Role__357D4CF9DD99A6B0",
                schema: "core",
                table: "Role",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_permissionId",
                schema: "core",
                table: "RolePermission",
                column: "permissionId");

            migrationBuilder.CreateIndex(
                name: "UQ__RolePerm__3213E83EEEFE6DC5",
                schema: "core",
                table: "RolePermission",
                column: "id",
                unique: true,
                filter: "([id] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_SP_nhanHieu",
                schema: "core",
                table: "SanPham",
                column: "nhanHieuId");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamDanhMuc_danhMucId",
                schema: "core",
                table: "SanPhamDanhMuc",
                column: "danhMucId");

            migrationBuilder.CreateIndex(
                name: "UQ__SanPhamD__3213E83E62FEA25D",
                schema: "core",
                table: "SanPhamDanhMuc",
                column: "id",
                unique: true,
                filter: "([id] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "AK_SanPhamDonVi_id",
                schema: "core",
                table: "SanPhamDonVi",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamDonVi_donViId",
                schema: "core",
                table: "SanPhamDonVi",
                column: "donViId");

            migrationBuilder.CreateIndex(
                name: "IX_SPDV_id",
                schema: "core",
                table: "SanPhamDonVi",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "UQ__SanPhamD__3213E83ECC46A234",
                schema: "core",
                table: "SanPhamDonVi",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamViTri_viTriId",
                schema: "core",
                table: "SanPhamViTri",
                column: "viTriId");

            migrationBuilder.CreateIndex(
                name: "UQ__SanPhamV__3213E83EB9656589",
                schema: "core",
                table: "SanPhamViTri",
                column: "id",
                unique: true,
                filter: "([id] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "UQ__tmp_ms_x__701B5126D1F8EDF3",
                schema: "core",
                table: "TaiKhoanKhachHang",
                column: "taiKhoanid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__tmp_ms_x__701A5D1E31FA1692",
                schema: "core",
                table: "TaiKhoanNhanVien",
                column: "taiKhoanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemNhan_maDinhDanhId",
                schema: "core",
                table: "TemNhan",
                column: "maDinhDanhId");

            migrationBuilder.CreateIndex(
                name: "IX_TheThanhVien_khachHangId",
                schema: "core",
                table: "TheThanhVien",
                column: "khachHangId");

            migrationBuilder.CreateIndex(
                name: "IX_TonKho_sanPhamDonViId",
                schema: "core",
                table: "TonKho",
                column: "sanPhamDonViId");

            migrationBuilder.CreateIndex(
                name: "IX_TrangThaiGiaoHang_donGiaoHangId",
                schema: "core",
                table: "TrangThaiGiaoHang",
                column: "donGiaoHangId");

            migrationBuilder.CreateIndex(
                name: "IX_TrangThaiXuLy_donHangId",
                schema: "core",
                table: "TrangThaiXuLy",
                column: "donHangId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_roleId",
                schema: "core",
                table: "UserRole",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "UQ__UserRole__3213E83E44209D3B",
                schema: "core",
                table: "UserRole",
                column: "id",
                unique: true,
                filter: "([id] IS NOT NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaoCaoBanChay",
                schema: "core");

            migrationBuilder.DropTable(
                name: "BaoCaoDoanhThu",
                schema: "core");

            migrationBuilder.DropTable(
                name: "BaoCaoTonKho",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Barcode",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ChamCong",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ChinhSachHoanTra_DanhMuc",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ChiTietDonOnline",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ChiTietGiaoDichNCC",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDon",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDonKhuyenMai",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuNhap",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuXuat",
                schema: "core");

            migrationBuilder.DropTable(
                name: "DieuKienApDungDanhMuc",
                schema: "core");

            migrationBuilder.DropTable(
                name: "DieuKienApDungSanPham",
                schema: "core");

            migrationBuilder.DropTable(
                name: "DieuKienApDungToanBo",
                schema: "core");

            migrationBuilder.DropTable(
                name: "GiaoDichThanhToan",
                schema: "core");

            migrationBuilder.DropTable(
                name: "GioHang",
                schema: "core");

            migrationBuilder.DropTable(
                name: "HinhAnh");

            migrationBuilder.DropTable(
                name: "KiemKe",
                schema: "core");

            migrationBuilder.DropTable(
                name: "LichSuGiaBan",
                schema: "core");

            migrationBuilder.DropTable(
                name: "LichSuMuaHang",
                schema: "core");

            migrationBuilder.DropTable(
                name: "NhatKyHoatDong",
                schema: "core");

            migrationBuilder.DropTable(
                name: "PhanCongCaLamViec",
                schema: "core");

            migrationBuilder.DropTable(
                name: "PhieuDoiTra",
                schema: "core");

            migrationBuilder.DropTable(
                name: "QRCode",
                schema: "core");

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "core");

            migrationBuilder.DropTable(
                name: "SanPhamDanhMuc",
                schema: "core");

            migrationBuilder.DropTable(
                name: "SanPhamViTri",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TaiKhoanKhachHang",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TaiKhoanNhanVien",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TemNhan",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TheThanhVien",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TonKho",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TrangThaiGiaoHang",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TrangThaiXuLy",
                schema: "core");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "core");

            migrationBuilder.DropTable(
                name: "BaoCao",
                schema: "core");

            migrationBuilder.DropTable(
                name: "LichSuGiaoDich",
                schema: "core");

            migrationBuilder.DropTable(
                name: "MaKhuyenMai",
                schema: "core");

            migrationBuilder.DropTable(
                name: "PhieuNhap",
                schema: "core");

            migrationBuilder.DropTable(
                name: "PhieuXuat",
                schema: "core");

            migrationBuilder.DropTable(
                name: "DieuKienApDung",
                schema: "core");

            migrationBuilder.DropTable(
                name: "KenhThanhToan",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CaLamViec",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ChinhSachHoanTra",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "core");

            migrationBuilder.DropTable(
                name: "DanhMuc",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ViTri",
                schema: "core");

            migrationBuilder.DropTable(
                name: "MaDinhDanhSanPham",
                schema: "core");

            migrationBuilder.DropTable(
                name: "DonGiaoHang",
                schema: "core");

            migrationBuilder.DropTable(
                name: "DonHangOnline",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "core");

            migrationBuilder.DropTable(
                name: "TaiKhoan",
                schema: "core");

            migrationBuilder.DropTable(
                name: "NhaCungCap",
                schema: "core");

            migrationBuilder.DropTable(
                name: "ChuongTrinhKhuyenMai",
                schema: "core");

            migrationBuilder.DropTable(
                name: "SanPhamDonVi",
                schema: "core");

            migrationBuilder.DropTable(
                name: "PhiVanChuyen",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Shipper",
                schema: "core");

            migrationBuilder.DropTable(
                name: "HoaDon",
                schema: "core");

            migrationBuilder.DropTable(
                name: "DonViDoLuong",
                schema: "core");

            migrationBuilder.DropTable(
                name: "SanPham",
                schema: "core");

            migrationBuilder.DropTable(
                name: "KhachHang",
                schema: "core");

            migrationBuilder.DropTable(
                name: "NhanVien",
                schema: "core");

            migrationBuilder.DropTable(
                name: "NhanHieu",
                schema: "core");
        }
    }
}

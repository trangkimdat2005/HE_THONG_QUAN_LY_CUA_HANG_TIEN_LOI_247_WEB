using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "qrCodeImage",
                schema: "core",
                table: "QRCode");

            migrationBuilder.AddColumn<string>(
                name: "anhId",
                schema: "core",
                table: "TemNhan",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "anhId",
                schema: "core",
                table: "QRCode",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "anhId",
                schema: "core",
                table: "NhanVien",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "anhId",
                schema: "core",
                table: "KhachHang",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Anh_SanPhamDonVi",
                schema: "core",
                columns: table => new
                {
                    sanPhamDonViId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    anhId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anh_SanPhamDonVi", x => new { x.anhId, x.sanPhamDonViId });
                    table.ForeignKey(
                        name: "FK_Anh_SanPhamDonVi_Anh_PK1",
                        column: x => x.anhId,
                        principalTable: "HinhAnh",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Anh_SanPhamDonVi_SanPhamDonVi",
                        column: x => x.sanPhamDonViId,
                        principalSchema: "core",
                        principalTable: "SanPhamDonVi",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "HinhAnh",
                schema: "core",
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

            migrationBuilder.CreateIndex(
                name: "IX_TemNhan_anhId",
                schema: "core",
                table: "TemNhan",
                column: "anhId");

            migrationBuilder.CreateIndex(
                name: "AK_SanPhamDonVi_id",
                schema: "core",
                table: "SanPhamDonVi",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SPDV_id",
                schema: "core",
                table: "SanPhamDonVi",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_QRCode_anhId",
                schema: "core",
                table: "QRCode",
                column: "anhId");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_anhId",
                schema: "core",
                table: "NhanVien",
                column: "anhId");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_anhId",
                schema: "core",
                table: "KhachHang",
                column: "anhId");

            migrationBuilder.CreateIndex(
                name: "IX_Anh_SanPhamDonVi_sanPhamDonViId",
                schema: "core",
                table: "Anh_SanPhamDonVi",
                column: "sanPhamDonViId");

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHang_Anh",
                schema: "core",
                table: "KhachHang",
                column: "anhId",
                principalTable: "HinhAnh",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NhanVien_Anh",
                schema: "core",
                table: "NhanVien",
                column: "anhId",
                principalTable: "HinhAnh",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QRCode_Anh",
                schema: "core",
                table: "QRCode",
                column: "anhId",
                principalTable: "HinhAnh",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemNhan_Anh",
                schema: "core",
                table: "TemNhan",
                column: "anhId",
                principalTable: "HinhAnh",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KhachHang_Anh",
                schema: "core",
                table: "KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_NhanVien_Anh",
                schema: "core",
                table: "NhanVien");

            migrationBuilder.DropForeignKey(
                name: "FK_QRCode_Anh",
                schema: "core",
                table: "QRCode");

            migrationBuilder.DropForeignKey(
                name: "FK_TemNhan_Anh",
                schema: "core",
                table: "TemNhan");

            migrationBuilder.DropTable(
                name: "Anh_SanPhamDonVi",
                schema: "core");

            migrationBuilder.DropTable(
                name: "HinhAnh",
                schema: "core");

            migrationBuilder.DropIndex(
                name: "IX_TemNhan_anhId",
                schema: "core",
                table: "TemNhan");

            migrationBuilder.DropIndex(
                name: "AK_SanPhamDonVi_id",
                schema: "core",
                table: "SanPhamDonVi");

            migrationBuilder.DropIndex(
                name: "IX_SPDV_id",
                schema: "core",
                table: "SanPhamDonVi");

            migrationBuilder.DropIndex(
                name: "IX_QRCode_anhId",
                schema: "core",
                table: "QRCode");

            migrationBuilder.DropIndex(
                name: "IX_NhanVien_anhId",
                schema: "core",
                table: "NhanVien");

            migrationBuilder.DropIndex(
                name: "IX_KhachHang_anhId",
                schema: "core",
                table: "KhachHang");

            migrationBuilder.DropColumn(
                name: "anhId",
                schema: "core",
                table: "TemNhan");

            migrationBuilder.DropColumn(
                name: "anhId",
                schema: "core",
                table: "QRCode");

            migrationBuilder.DropColumn(
                name: "anhId",
                schema: "core",
                table: "NhanVien");

            migrationBuilder.DropColumn(
                name: "anhId",
                schema: "core",
                table: "KhachHang");

            migrationBuilder.AddColumn<string>(
                name: "qrCodeImage",
                schema: "core",
                table: "QRCode",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}

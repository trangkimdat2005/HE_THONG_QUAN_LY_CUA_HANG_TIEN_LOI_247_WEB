using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Migrations
{
    /// <inheritdoc />
    public partial class cap_nhat_file_barcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "barcodeImage",
                schema: "core",
                table: "Barcode");

            migrationBuilder.AddColumn<string>(
                name: "anhId",
                schema: "core",
                table: "Barcode",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Barcode_anhId",
                schema: "core",
                table: "Barcode",
                column: "anhId");

            migrationBuilder.AddForeignKey(
                name: "FK_BarCode_Anh",
                schema: "core",
                table: "Barcode",
                column: "anhId",
                principalSchema: "core",
                principalTable: "HinhAnh",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BarCode_Anh",
                schema: "core",
                table: "Barcode");

            migrationBuilder.DropIndex(
                name: "IX_Barcode_anhId",
                schema: "core",
                table: "Barcode");

            migrationBuilder.DropColumn(
                name: "anhId",
                schema: "core",
                table: "Barcode");

            migrationBuilder.AddColumn<string>(
                name: "barcodeImage",
                schema: "core",
                table: "Barcode",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}

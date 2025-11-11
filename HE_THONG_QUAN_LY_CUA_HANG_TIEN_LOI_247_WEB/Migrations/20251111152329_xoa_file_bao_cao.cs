using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Migrations
{
    /// <inheritdoc />
    public partial class xoa_file_bao_cao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fileBaoCao",
                schema: "core",
                table: "BaoCao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fileBaoCao",
                schema: "core",
                table: "BaoCao",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyThuVien.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable_CTPhieuMuonDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TinhTrang",
                table: "CTPhieuMuon");

            migrationBuilder.AddColumn<int>(
                name: "TrangThai",
                table: "CTPhieuMuon",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "CTPhieuMuon");

            migrationBuilder.AddColumn<string>(
                name: "TinhTrang",
                table: "CTPhieuMuon",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

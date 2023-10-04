using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyThuVien.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable_PhieuMuon_CTPhieuMuon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GhiChu",
                table: "CTPhieuMuon",
                newName: "GhiChuTra");

            migrationBuilder.AddColumn<string>(
                name: "GhiChuMuon",
                table: "PhieuMuons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayTra",
                table: "CTPhieuMuon",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GhiChuMuon",
                table: "PhieuMuons");

            migrationBuilder.RenameColumn(
                name: "GhiChuTra",
                table: "CTPhieuMuon",
                newName: "GhiChu");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayTra",
                table: "CTPhieuMuon",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}

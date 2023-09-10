using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyThuVien.Migrations
{
    /// <inheritdoc />
    public partial class AddTablesToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TacGias",
                columns: table => new
                {
                    ID_TacGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTacGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuocGia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TacGias", x => x.ID_TacGia);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoans",
                columns: table => new
                {
                    ID_TaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoans", x => x.ID_TaiKhoan);
                });

            migrationBuilder.CreateTable(
                name: "TheLoais",
                columns: table => new
                {
                    ID_TheLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTheLoai = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheLoais", x => x.ID_TheLoai);
                });

            migrationBuilder.CreateTable(
                name: "TheThuViens",
                columns: table => new
                {
                    ID_The = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayBD = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheThuViens", x => x.ID_The);
                });

            migrationBuilder.CreateTable(
                name: "ThuThus",
                columns: table => new
                {
                    ID_ThuThu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenThuThu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ID_TaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuThus", x => x.ID_ThuThu);
                    table.ForeignKey(
                        name: "FK_ThuThus_TaiKhoans_ID_TaiKhoan",
                        column: x => x.ID_TaiKhoan,
                        principalTable: "TaiKhoans",
                        principalColumn: "ID_TaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Saches",
                columns: table => new
                {
                    ID_Sach = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSach = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GiaTien = table.Column<double>(type: "float", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    UrlImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID_TheLoai = table.Column<int>(type: "int", nullable: false),
                    ID_TacGia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Saches", x => x.ID_Sach);
                    table.ForeignKey(
                        name: "FK_Saches_TacGias_ID_TacGia",
                        column: x => x.ID_TacGia,
                        principalTable: "TacGias",
                        principalColumn: "ID_TacGia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Saches_TheLoais_ID_TheLoai",
                        column: x => x.ID_TheLoai,
                        principalTable: "TheLoais",
                        principalColumn: "ID_TheLoai",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocGias",
                columns: table => new
                {
                    ID_DocGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDocGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoCCCD = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ID_The = table.Column<int>(type: "int", nullable: false),
                    ID_TaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocGias", x => x.ID_DocGia);
                    table.ForeignKey(
                        name: "FK_DocGias_TaiKhoans_ID_TaiKhoan",
                        column: x => x.ID_TaiKhoan,
                        principalTable: "TaiKhoans",
                        principalColumn: "ID_TaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocGias_TheThuViens_ID_The",
                        column: x => x.ID_The,
                        principalTable: "TheThuViens",
                        principalColumn: "ID_The",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhieuMuons",
                columns: table => new
                {
                    ID_PhieuMuon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayTaoPhieu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayHenTra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ID_ThuThu = table.Column<int>(type: "int", nullable: false),
                    ID_The = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuMuons", x => x.ID_PhieuMuon);
                    table.ForeignKey(
                        name: "FK_PhieuMuons_TheThuViens_ID_The",
                        column: x => x.ID_The,
                        principalTable: "TheThuViens",
                        principalColumn: "ID_The",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhieuMuons_ThuThus_ID_ThuThu",
                        column: x => x.ID_ThuThu,
                        principalTable: "ThuThus",
                        principalColumn: "ID_ThuThu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CTPhieuMuon",
                columns: table => new
                {
                    ID_PhieuMuon = table.Column<int>(type: "int", nullable: false),
                    ID_Sach = table.Column<int>(type: "int", nullable: false),
                    NgayTra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTPhieuMuon", x => new { x.ID_PhieuMuon, x.ID_Sach });
                    table.ForeignKey(
                        name: "FK_CTPhieuMuon_PhieuMuons_ID_PhieuMuon",
                        column: x => x.ID_PhieuMuon,
                        principalTable: "PhieuMuons",
                        principalColumn: "ID_PhieuMuon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CTPhieuMuon_Saches_ID_Sach",
                        column: x => x.ID_Sach,
                        principalTable: "Saches",
                        principalColumn: "ID_Sach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CTPhieuMuon_ID_Sach",
                table: "CTPhieuMuon",
                column: "ID_Sach");

            migrationBuilder.CreateIndex(
                name: "IX_DocGias_Email_SoCCCD",
                table: "DocGias",
                columns: new[] { "Email", "SoCCCD" });

            migrationBuilder.CreateIndex(
                name: "IX_DocGias_ID_TaiKhoan",
                table: "DocGias",
                column: "ID_TaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_DocGias_ID_The",
                table: "DocGias",
                column: "ID_The");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuMuons_ID_The",
                table: "PhieuMuons",
                column: "ID_The");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuMuons_ID_ThuThu",
                table: "PhieuMuons",
                column: "ID_ThuThu");

            migrationBuilder.CreateIndex(
                name: "IX_Saches_ID_TacGia",
                table: "Saches",
                column: "ID_TacGia");

            migrationBuilder.CreateIndex(
                name: "IX_Saches_ID_TheLoai",
                table: "Saches",
                column: "ID_TheLoai");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoans_TenDangNhap",
                table: "TaiKhoans",
                column: "TenDangNhap",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TheLoais_TenTheLoai",
                table: "TheLoais",
                column: "TenTheLoai",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThuThus_Email",
                table: "ThuThus",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThuThus_ID_TaiKhoan",
                table: "ThuThus",
                column: "ID_TaiKhoan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CTPhieuMuon");

            migrationBuilder.DropTable(
                name: "DocGias");

            migrationBuilder.DropTable(
                name: "PhieuMuons");

            migrationBuilder.DropTable(
                name: "Saches");

            migrationBuilder.DropTable(
                name: "TheThuViens");

            migrationBuilder.DropTable(
                name: "ThuThus");

            migrationBuilder.DropTable(
                name: "TacGias");

            migrationBuilder.DropTable(
                name: "TheLoais");

            migrationBuilder.DropTable(
                name: "TaiKhoans");
        }
    }
}

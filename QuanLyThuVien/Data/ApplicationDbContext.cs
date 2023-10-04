using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        // Create table SQL Server
        public DbSet<TheLoai> TheLoais { get; set; }
        public DbSet<TacGia> TacGias { get; set; }
        public DbSet<Sach> Saches { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<ThuThu> ThuThus { get; set; }
        public DbSet<TheThuVien> TheThuViens { get; set; }
        public DbSet<DocGia> DocGias { get; set; }
        public DbSet<PhieuMuon> PhieuMuons { get; set; }
        public DbSet<CTPhieuMuon> CTPhieuMuon { get; set; }

        // Create Unique
        protected override void OnModelCreating(ModelBuilder builders)
        {
            // Unique table TaiKhoan
            builders.Entity<TaiKhoan>(entity =>
            {
                entity.HasIndex(e => e.TenDangNhap).IsUnique();
            });
            // Unique table TheLoai
            builders.Entity<TheLoai>(entity =>
            {
                entity.HasIndex(e => e.TenTheLoai).IsUnique();
            });
            // Unique Table ThuThu
            builders.Entity<ThuThu>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });
            // Unique Table DocGia
            builders.Entity<DocGia>(entity =>
            {
                entity.HasIndex(e => new {e.Email, e.SoCCCD});
            });

            // Set multipe key table CTPhieuMuon
            builders.Entity<CTPhieuMuon>().HasKey(o => new { o.ID_PhieuMuon, o.ID_Sach });
        }
    }
}

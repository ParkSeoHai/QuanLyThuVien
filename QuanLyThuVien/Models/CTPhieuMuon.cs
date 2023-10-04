using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class CTPhieuMuon
    {
        private int _ID_PhieuMuon;
        private int _ID_Sach;
        private int _SoLuongMuon;
        private int _TrangThai;
        private DateTime? _NgayTra;
        private string _GhiChuTra;

        private PhieuMuon _PhieuMuon;
        private Sach _Sach;

        public int ID_PhieuMuon { get => _ID_PhieuMuon; set => _ID_PhieuMuon = value; }
        public int ID_Sach { get => _ID_Sach; set => _ID_Sach = value; }

        [ForeignKey("ID_PhieuMuon")]
        public PhieuMuon PhieuMuon { get => _PhieuMuon; set => _PhieuMuon = value; }
        [ForeignKey("ID_Sach")]
        public Sach Sach { get => _Sach; set => _Sach = value; }
        public int TrangThai { get => _TrangThai; set => _TrangThai = value; }
        public int SoLuongMuon { get => _SoLuongMuon; set => _SoLuongMuon = value; }
        public DateTime? NgayTra { get => _NgayTra; set => _NgayTra = value; }
        public string GhiChuTra { get => _GhiChuTra; set => _GhiChuTra = value; }
    }
}

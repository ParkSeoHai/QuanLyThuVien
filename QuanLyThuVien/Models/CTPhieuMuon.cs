using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class CTPhieuMuon
    {
        private int _ID_PhieuMuon;
        private int _ID_Sach;
        private DateTime _NgayTra;
        private string _GhiChu;
        private string _TinhTrang;

        private PhieuMuon _PhieuMuon;
        private Sach _Sach;

        public int ID_PhieuMuon { get => _ID_PhieuMuon; set => _ID_PhieuMuon = value; }
        public int ID_Sach { get => _ID_Sach; set => _ID_Sach = value; }
        public DateTime NgayTra { get => _NgayTra; set => _NgayTra = value; }
        public string GhiChu { get => _GhiChu; set => _GhiChu = value; }
        public string TinhTrang { get => _TinhTrang; set => _TinhTrang = value; }

        [ForeignKey("ID_PhieuMuon")]
        public PhieuMuon PhieuMuon { get => _PhieuMuon; set => _PhieuMuon = value; }
        [ForeignKey("ID_Sach")]
        public Sach Sach { get => _Sach; set => _Sach = value; }
    }
}

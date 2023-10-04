using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class PhieuMuon
    {
        private int _ID_PhieuMuon;
        private DateTime _NgayTaoPhieu;
        private DateTime _NgayHenTra;
        private string _GhiChuMuon;

        private int _ID_ThuThu;
        private ThuThu _ThuThu;

        private int _ID_The;
        private TheThuVien _TheThuVien;

        private ICollection<CTPhieuMuon> _CTPhieuMuon;

        [Key]
        public int ID_PhieuMuon { get => _ID_PhieuMuon; set => _ID_PhieuMuon = value; }
        public DateTime NgayTaoPhieu { get => _NgayTaoPhieu; set => _NgayTaoPhieu = DateTime.Now; }
        public DateTime NgayHenTra { get => _NgayHenTra; set => _NgayHenTra = value; }

        public int ID_ThuThu { get => _ID_ThuThu; set => _ID_ThuThu = value; }
        [ForeignKey("ID_ThuThu")]
        public ThuThu ThuThu { get => _ThuThu; set => _ThuThu = value; }

        public int ID_The { get => _ID_The; set => _ID_The = value; }
        [ForeignKey("ID_The")]
        public TheThuVien TheThuVien { get => _TheThuVien; set => _TheThuVien = value; }
        public ICollection<CTPhieuMuon> CTPhieuMuon { get => _CTPhieuMuon; set => _CTPhieuMuon = value; }
        public string GhiChuMuon { get => _GhiChuMuon; set => _GhiChuMuon = value; }
    }
}

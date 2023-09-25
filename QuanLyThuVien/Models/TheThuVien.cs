using System.ComponentModel.DataAnnotations;

namespace QuanLyThuVien.Models
{
    public class TheThuVien
    {
        private int _ID_The;
        private DateTime _NgayBD;
        private DateTime _NgayHetHan;
        private string _GhiChu;

        private ICollection<DocGia> _DocGias;
        private ICollection<PhieuMuon> _PhieuMuon;

        [Key]
        public int ID_The { get => _ID_The; set => _ID_The = value; }
        [Required]
        public DateTime NgayBD { get => _NgayBD; set => _NgayBD = DateTime.Now; }
        [Required]
        public DateTime NgayHetHan { get => _NgayHetHan; set => _NgayHetHan = value; }
        public string GhiChu { get => _GhiChu; set => _GhiChu = value; }

        public ICollection<DocGia> DocGias { get => _DocGias; set => _DocGias = value; }
        public ICollection<PhieuMuon> PhieuMuon { get => _PhieuMuon; set => _PhieuMuon = value; }
    }
}

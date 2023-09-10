using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class ThuThu
    {
        private int _ID_ThuThu;
        private string _TenThuThu;
        private string _GioiTinh;
        private DateTime _NgaySinh;
        private string _SDT;
        private string _Email;
        private int _ID_TaiKhoan;
        private TaiKhoan _TaiKhoan;
        private ICollection<PhieuMuon> _PhieuMuon;

        [Key]
        public int ID_ThuThu { get => _ID_ThuThu; set => _ID_ThuThu = value; }
        public string TenThuThu { get => _TenThuThu; set => _TenThuThu = value; }
        public string GioiTinh { get => _GioiTinh; set => _GioiTinh = value; }
        public DateTime NgaySinh { get => _NgaySinh; set => _NgaySinh = value; }
        public string SDT { get => _SDT; set => _SDT = value; }
        public string Email { get => _Email; set => _Email = value; }

        public int ID_TaiKhoan { get => _ID_TaiKhoan; set => _ID_TaiKhoan = value; }
        [ForeignKey("ID_TaiKhoan")]
        public TaiKhoan TaiKhoan { get => _TaiKhoan; set => _TaiKhoan = value; }

        public ICollection<PhieuMuon> PhieuMuon { get => _PhieuMuon; set => _PhieuMuon = value; }
    }
}

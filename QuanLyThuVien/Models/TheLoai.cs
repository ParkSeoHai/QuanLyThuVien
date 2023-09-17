using System.ComponentModel.DataAnnotations;

namespace QuanLyThuVien.Models
{
    public class TheLoai
    {
        private int _ID_TheLoai;
        private string _TenTheLoai;
        private ICollection<Sach> _Sachs;

        [Key]
        public int ID_TheLoai { get => _ID_TheLoai; set => _ID_TheLoai = value; }
        [Required]
        public string TenTheLoai { get => _TenTheLoai; set => _TenTheLoai = value; }
        public ICollection<Sach> Sachs { get => _Sachs; set => _Sachs = value; }

        public TheLoai() { }
        public TheLoai(int iD_TheLoai, string tenTheLoai)
        {
            ID_TheLoai = iD_TheLoai;
            TenTheLoai = tenTheLoai;
        }
    }
}

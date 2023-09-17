using System.ComponentModel.DataAnnotations;

namespace QuanLyThuVien.Models
{
    public class TacGia
    {
        private int _ID_TacGia;
        private string _TenTacGia;
        private string _QuocGia;
        private ICollection<Sach> _Sachs;

        [Key]
        public int ID_TacGia { get => _ID_TacGia; set => _ID_TacGia = value; }
        [Required]
        public string TenTacGia { get => _TenTacGia; set => _TenTacGia = value; }
        [Required]
        public string QuocGia { get => _QuocGia; set => _QuocGia = value; }
        public ICollection<Sach> Sachs { get => _Sachs; set => _Sachs = value; }

        public TacGia() { }

        public TacGia(int iD_TacGia, string tenTacGia, string quocGia)
        {
            ID_TacGia = iD_TacGia;
            TenTacGia = tenTacGia;
            QuocGia = quocGia;
        }
    }
}

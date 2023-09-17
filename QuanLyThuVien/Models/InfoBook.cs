using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLyThuVien.Models
{
    public class InfoBook : Sach
    {
        private string? _TenTacGia;
        private string? _TenTheLoai;
        private Sach _Sach;

        // Danh sách lưu dữ liệu thể loại sách
        private List<SelectListItem> _TheLoaiSelectList;

        public string? TenTacGia { get => _TenTacGia; set => _TenTacGia = value; }
        public string? TenTheLoai { get => _TenTheLoai; set => _TenTheLoai = value; }
        public Sach Sach { get => _Sach; set => _Sach = value; }
        public List<SelectListItem> TheLoaiSelectList { get => _TheLoaiSelectList; set => _TheLoaiSelectList = value; }

        public InfoBook()
        {

        }

        public InfoBook(string tenSach, string tenTacGia, string tenTheLoai)
        {
            base.TenSach = tenSach;
            this.TenTacGia = tenTacGia;
            this.TenTheLoai = tenTheLoai;
        }

        public InfoBook(Sach sach, TacGia tgia, TheLoai tloai)
        {
            this.Sach = sach;
            base.TacGia = tgia;
            base.TheLoai = tloai;
        }
    }
}

namespace QuanLyThuVien.Models
{
    public class DashboardBook
    {
        private string? _TenSach;
        private string? _TenTacGia;
        private string? _TenTheLoai;

        public string TenSach { get => _TenSach; set => _TenSach = value; }
        public string TenTacGia { get => _TenTacGia; set => _TenTacGia = value; }
        public string TenTheLoai { get => _TenTheLoai; set => _TenTheLoai = value; }

        public DashboardBook()
        {

        }

        public DashboardBook(string tenSach, string tenTacGia, string tenTheLoai)
        {
            this.TenSach = tenSach;
            this.TenTacGia = tenTacGia;
            this.TenTheLoai = tenTheLoai;
        }
    }
}

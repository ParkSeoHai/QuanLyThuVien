namespace QuanLyThuVien.Models
{
    public class ThongKeSachMuonNhieu
    {
        private int idSach;
        private string tenSach;
        private string tenTacGia;
        private string tenTheLoai;
        private DateTime ngayNhap;
        private double giaBan;
        private int soLuongMuon;

        public string TenSach { get => tenSach; set => tenSach = value; }
        public string TenTacGia { get => tenTacGia; set => tenTacGia = value; }
        public string TenTheLoai { get => tenTheLoai; set => tenTheLoai = value; }
        public DateTime NgayNhap { get => ngayNhap; set => ngayNhap = value; }
        public double GiaBan { get => giaBan; set => giaBan = value; }
        public int SoLuongMuon { get => soLuongMuon; set => soLuongMuon = value; }
        public int IdSach { get => idSach; set => idSach = value; }

        public ThongKeSachMuonNhieu() { }

        public ThongKeSachMuonNhieu(int id, string tenSach, string tenTacGia, string tenTheLoai, DateTime ngayNhap, double giaBan, int soLuongMuon)
        {
            IdSach = id;
            TenSach = tenSach;
            TenTacGia = tenTacGia;
            TenTheLoai = tenTheLoai;
            NgayNhap = ngayNhap;
            GiaBan = giaBan;
            SoLuongMuon = soLuongMuon;
        }
    }
}

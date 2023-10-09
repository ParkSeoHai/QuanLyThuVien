namespace QuanLyThuVien.Models
{
    public class SachTra
    {
        private int _Id_PhieuMuon;
        private int _Id_Sach;
        private string _TenSach;
        private int _Id_MaThe;
        private string _TenDocGia;
        private int _SlMuon;
        private DateTime? _NgayTra;
        private string _GhiChuTra;

        public int Id_PhieuMuon { get => _Id_PhieuMuon; set => _Id_PhieuMuon = value; }
        public int Id_Sach { get => _Id_Sach; set => _Id_Sach = value; }
        public string TenSach { get => _TenSach; set => _TenSach = value; }
        public int Id_MaThe { get => _Id_MaThe; set => _Id_MaThe = value; }
        public string TenDocGia { get => _TenDocGia; set => _TenDocGia = value; }
        public int SlMuon { get => _SlMuon; set => _SlMuon = value; }
        public DateTime? NgayTra { get => _NgayTra; set => _NgayTra = value; }
        public string GhiChuTra { get => _GhiChuTra; set => _GhiChuTra = value; }

        public SachTra() { }

        public SachTra(int id_PhieuMuon, int id_Sach, string tenSach, int id_MaThe, string tenDocGia, int slMuon, DateTime? ngayTra, string ghiChuTra)
        {
            Id_PhieuMuon = id_PhieuMuon;
            Id_Sach = id_Sach;
            TenSach = tenSach;
            Id_MaThe = id_MaThe;
            TenDocGia = tenDocGia;
            SlMuon = slMuon;
            NgayTra = ngayTra;
            GhiChuTra = ghiChuTra;
        }
    }
}

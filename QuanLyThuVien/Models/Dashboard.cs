namespace QuanLyThuVien.Models
{
    public class Dashboard
    {
        private string? _TenTaiKhoan;
        private string? _VaiTro;
        private int? _TongDocGia;
        private int? _TongSach;
        private int? _TongTaiKhoan;
        private int? _TongMuonSach;
        private LinkedList<InfoBook>? _LstDbBooks = new LinkedList<InfoBook>();
        private LinkedList<TaiKhoan>? _ListTaiKhoan = new LinkedList<TaiKhoan>();

        public string? TenTaiKhoan { get => _TenTaiKhoan; set => _TenTaiKhoan = value; }
        public string? VaiTro { get => _VaiTro; set => _VaiTro = value; }
        public int? TongDocGia { get => _TongDocGia; set => _TongDocGia = value; }
        public int? TongSach { get => _TongSach; set => _TongSach = value; }
        public int? TongTaiKhoan { get => _TongTaiKhoan; set => _TongTaiKhoan = value; }
        public int? TongMuonSach { get => _TongMuonSach; set => _TongMuonSach = value; }
        public LinkedList<TaiKhoan>? ListTaiKhoan { get => _ListTaiKhoan; set => _ListTaiKhoan = value; }
        public LinkedList<InfoBook>? LstDbBooks { get => _LstDbBooks; set => _LstDbBooks = value; }

        public Dashboard()
        {
            
        }

        public Dashboard(string tenTK, string vaiTro, int tongDG, int tongSach, int tongTK, int tongMuon, LinkedList<InfoBook> lstDbBook, LinkedList<TaiKhoan> lstTK)
        {
            this.TenTaiKhoan = tenTK;
            this.VaiTro = vaiTro;
            this.TongDocGia = tongDG;
            this.TongSach = tongSach;
            this.TongTaiKhoan = tongTK;
            this.TongMuonSach = tongMuon;
            this.LstDbBooks = lstDbBook;
            this.ListTaiKhoan = lstTK;
        }
    }
}

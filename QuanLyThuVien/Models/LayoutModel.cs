namespace QuanLyThuVien.Models
{
    public class LayoutModel
    {
        private string _TenDangNhap;
        private string _VaiTro;

        public string TenDangNhap { get => _TenDangNhap; set => _TenDangNhap = value; }
        public string VaiTro { get => _VaiTro; set => _VaiTro = value; }
        public LayoutModel() { }

        public LayoutModel(string tenDN, string vaiTro)
        {
            TenDangNhap = tenDN;
            VaiTro = vaiTro;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class BaoCaoThongKeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BaoCaoThongKeController(ApplicationDbContext db)
        {
            this._db = db;
        }

        public IActionResult Index()
        {
            List<ThongKeSachMuonNhieu> lstSach = new List<ThongKeSachMuonNhieu>();
            var data = from s in _db.Saches
                       join tl in _db.TheLoais on s.ID_TheLoai equals tl.ID_TheLoai
                       join tg in _db.TacGias on s.ID_TacGia equals tg.ID_TacGia
                       join ctpm in _db.CTPhieuMuon on s.ID_Sach equals ctpm.ID_Sach
                       join pm in _db.PhieuMuons on ctpm.ID_PhieuMuon equals pm.ID_PhieuMuon
                       where ctpm.TrangThai == 1
                       group new { s, ctpm } by new { s.ID_Sach, s.TenSach, tg.TenTacGia, tl.TenTheLoai, s.NgayNhap, s.GiaTien } into g
                       orderby g.Sum(x => x.ctpm.SoLuongMuon) descending
                       select new
                       {
                           obj = new ThongKeSachMuonNhieu(
                               g.Key.ID_Sach,
                               g.Key.TenSach,
                               g.Key.TenTacGia,
                               g.Key.TenTheLoai,
                               g.Key.NgayNhap,
                               g.Key.GiaTien,
                               g.Sum(x => x.ctpm.SoLuongMuon)
                           )
                       };
            foreach (var item in data)
            {
                lstSach.Add(item.obj);
            }

            return View(lstSach);
        }
    }
}

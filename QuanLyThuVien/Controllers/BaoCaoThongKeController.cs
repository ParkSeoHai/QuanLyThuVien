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
                       from tl in _db.TheLoais
                       from tg in _db.TacGias
                       from pm in _db.PhieuMuons
                       from ctpm in _db.CTPhieuMuon
                       where s.ID_Sach == ctpm.ID_Sach && tl.ID_TheLoai == s.ID_TheLoai
                       && s.ID_TacGia == tg.ID_TacGia && ctpm.ID_PhieuMuon == pm.ID_PhieuMuon
                       && ctpm.TrangThai == 1
                       orderby ctpm.SoLuongMuon descending
                       select new
                       {
                           obj = new ThongKeSachMuonNhieu(s.ID_Sach, s.TenSach, tg.TenTacGia, tl.TenTheLoai, s.NgayNhap, s.GiaTien, ctpm.SoLuongMuon)
                       };
            foreach (var item in data)
            {
                lstSach.Add(item.obj);
            }

            return View(lstSach);
        }
    }
}

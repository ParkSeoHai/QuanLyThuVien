using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;
using System.Diagnostics;

namespace QuanLyThuVien.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db) 
        {
            _db = db;
        }

        private int Id = LoginController.id;
        
        public IActionResult Index()
        {
            bool isId = checkId(Id);
            if(isId)
            {
                string tenTK = getTenTK(Id);
                string vaiTro = getVaiTro(Id);
                int tongDG = getTongDG();
                int tongSach = getTongSach();
                int tongTK = getTongTK();
                int tongMuon = getTongMuon();
                LinkedList<InfoBook> lstDbBook = getLstDbBook();
                LinkedList<TaiKhoan> lstTaiKhoan = getLstTaiKhoan();

                Dashboard dashboard = new Dashboard(tenTK, vaiTro, tongDG, tongSach, tongTK, tongMuon, lstDbBook, lstTaiKhoan);

                return View(dashboard);
            }
            return NotFound();
        }

        // Method kiểm tra id
        public bool checkId(int id)
        {
            if (id == 0) return false;
            return true;
        }

        // Method lấy Tên tài khoản
        public string getTenTK(int id)
        {
            string TenTK = "";
            var value = from tk in _db.TaiKhoans
                           where tk.ID_TaiKhoan == id
                           select tk.TenDangNhap;
            if(value.Count() > 0 )
            {
                foreach(var tk in value ) 
                {
                    TenTK = tk;
                }
            }
            return TenTK;
        }

        // Method lấy vai trò của tài khoản
        public string getVaiTro(int id)
        {
            string VaiTro = "";
            var value = from tk in _db.TaiKhoans
                        where tk.ID_TaiKhoan == id
                        select tk.VaiTro;
            if (value.Count() > 0)
            {
                foreach (var tk in value)
                {
                    VaiTro = tk;
                }
            }
            return VaiTro;
        }

        // Method lấy tổng độc giả
        public int getTongDG()
        {
            var result = from dg in _db.DocGias
                         select dg;
            return result.Count();
        }

        // Method lấy tổng sách
        public int getTongSach()
        {
            var result = from s in _db.Saches
                         select s;
            return result.Count();
        }

        // Method lấy tổng tài khoản
        public int getTongTK()
        {
            var result = from tk in _db.TaiKhoans
                         select tk;
            return result.Count();
        }

        // Method lấy tổng phiếu mượn sách
        public int getTongMuon()
        {
            var result = from pm in _db.PhieuMuons
                         select pm;
            return result.Count();
        }

        // Method lấy danh sách 10 sách mới nhất
        public LinkedList<InfoBook> getLstDbBook()
        {
            LinkedList<InfoBook> lstDbBook = new LinkedList<InfoBook>();
            var obj = from s in _db.Saches
                      from tg in _db.TacGias
                      from tl in _db.TheLoais
                      where (s.ID_TacGia == tg.ID_TacGia && s.ID_TheLoai == tl.ID_TheLoai)
                      orderby s.ID_Sach descending
                      select new
                      {
                          TenSach = s.TenSach,
                          TenTacGia = tg.TenTacGia,
                          TenTheLoai = tl.TenTheLoai
                      };

            int i = 0;
            foreach(var item in obj)
            {
                if (i == 10) return lstDbBook;
                InfoBook data = new InfoBook(item.TenSach, item.TenTacGia, item.TenTheLoai);
                lstDbBook.AddLast(data);
                ++i;
            }

            return lstDbBook;
        }

        // Method lấy danh sách 5 tài khoản mới nhất
        public LinkedList<TaiKhoan> getLstTaiKhoan()
        {
            LinkedList<TaiKhoan> lstTK = new LinkedList<TaiKhoan>();
            var obj = from tk in _db.TaiKhoans
                      where tk.VaiTro == "Độc giả"
                      orderby tk.ID_TaiKhoan descending
                      select tk;
            int i = 0;
            foreach (var tk in obj)
            {
                if (i == 5) return lstTK;
                lstTK.AddLast(tk);
                ++i;
            }
            return lstTK;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
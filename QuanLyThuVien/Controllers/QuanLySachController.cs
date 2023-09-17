using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class QuanLySachController : Controller
    {
        private readonly ApplicationDbContext _db;
        public QuanLySachController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET
        public IActionResult Index()
        {
            LinkedList<InfoBook> lstSach = getDataSach();
            if(lstSach.Count <= 0)
            {
                TempData["message"] = "Dữ liệu trống";
                return View(lstSach);
            }
            return View(lstSach);
        }

        // GET
        public IActionResult Create()
        {
            var theLoais = from tl in _db.TheLoais
                          select tl;

            InfoBook infoBook = new InfoBook();
            infoBook.TheLoaiSelectList = new List<SelectListItem>();

            foreach(var item in theLoais)
            {
                infoBook.TheLoaiSelectList.Add(new SelectListItem { Text = item.TenTheLoai, Value = item.ID_TheLoai.ToString() });
            }

            return View(infoBook);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InfoBook infoBook)
        {
            int idTacGia;

            try
            {
                // Biến = true nếu tác giả đã tồn tại trong csdl
                bool isTg = checkTacGia(infoBook.Sach.TacGia.TenTacGia, infoBook.Sach.TacGia.QuocGia);
                if (isTg)
                {
                    idTacGia = selectIdTacGia(infoBook.Sach.TacGia.TenTacGia, infoBook.Sach.TacGia.QuocGia);
                    Sach sach = new Sach(infoBook.Sach.TenSach, DateTime.Now, infoBook.Sach.GiaTien, infoBook.Sach.SoLuong, infoBook.Sach.UrlImg, idTacGia, infoBook.Sach.TheLoai.ID_TheLoai);
                    if (addSach(sach))
                    {
                        TempData["success"] = "Thêm sách mới thành công";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    bool isAddTacGia = addTacGia(infoBook.Sach.TacGia);
                    if (isAddTacGia)
                    {
                        idTacGia = selectIdTacGia(infoBook.Sach.TacGia.TenTacGia, infoBook.Sach.TacGia.QuocGia);
                        Sach sach = new Sach(infoBook.Sach.TenSach, DateTime.Now, infoBook.Sach.GiaTien, infoBook.Sach.SoLuong, infoBook.Sach.UrlImg, idTacGia, infoBook.Sach.TheLoai.ID_TheLoai);
                        if (addSach(sach))
                        {
                            TempData["success"] = "Thêm sách mới thành công";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                }
            } catch (Exception ex)
            {
                TempData["error"] = ex.InnerException.Message;
                return RedirectToAction("Index");
            }

            return View(infoBook);
        }

        // Method tìm kiêm Id tác giả để thêm sách mới
        public int selectIdTacGia(string tenTacGia, string quocGia)
        {
            var tacGia = from tg in _db.TacGias
                         where tg.TenTacGia == tenTacGia && tg.QuocGia == quocGia
                         select tg.ID_TacGia;
            foreach(var tg in tacGia)
            {
                return tg;
            }
            return 0;
        }

        // Method thêm dữ liệu sách vào csdl
        public bool addSach(Sach obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                _db.Add(obj);
                if (_db.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        // Method kiểm tra tác giả có trong csdl hay chưa
        public bool checkTacGia(string tenTacGia, string quocGia)
        {
            var tacgia = from tg in _db.TacGias
                         where tg.TenTacGia.ToUpper() == tenTacGia.ToUpper() && tg.QuocGia.ToUpper() == quocGia.ToUpper()
                         select tg;
            if(tacgia.Count() > 0)
            {
                return true;
            }
            return false;
        }

        // Method thêm dữ liệu vào bảng tác giả trong trường hợp tác giả chưa tồn tại trong csdl
        public bool addTacGia(TacGia obj)
        {
            if (obj == null)
            {
                return false;
            } else
            {
                _db.Add(obj);
                if(_db.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        // Method get data thông tin sách để hiển thị lên trang Index
        public LinkedList<InfoBook> getDataSach()
        {
            LinkedList<InfoBook> lstSach = new LinkedList<InfoBook>();
            var dataSach = from s in _db.Saches
                           from tg in _db.TacGias
                           from tl in _db.TheLoais
                           where (s.ID_TacGia == tg.ID_TacGia && s.ID_TheLoai == tl.ID_TheLoai)
                           select new
                           {
                               sach = new Sach(s.ID_Sach, s.TenSach, s.GiaTien, s.SoLuong, s.UrlImg, s.NgayNhap),
                               tacGia = new TacGia(tg.ID_TacGia, tg.TenTacGia, tg.QuocGia),
                               theLoai = new TheLoai(tl.ID_TheLoai, tl.TenTheLoai)
                           };

            foreach (var item in dataSach)
            {
                InfoBook infoBook = new InfoBook(item.sach, item.tacGia, item.theLoai);
                lstSach.AddLast(infoBook);
            }
            return lstSach;
        }
    }
}

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

        public List<SelectListItem> TheLoaiSelectList = new List<SelectListItem>();

        // GET View Index
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

        // Post search sách
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchSach(string? searchInput)
        {
            // Danh sách lưu thông tin để hiện thị ra view
            LinkedList<InfoBook> lstSach = new LinkedList<InfoBook>();

            // Truy vấn lấy dữ liệu thông tin sách
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
                // Nếu khớp với từ tìm kiếm thì thêm vào danh sách lstSach
                var isSearch = item.sach.ID_Sach.ToString() == searchInput
                                || item.sach.TenSach.ToUpper().Contains(searchInput.ToUpper());
                if(isSearch)
                {
                    InfoBook info = new InfoBook(item.sach, item.tacGia, item.theLoai);
                    lstSach.AddLast(info);
                }
            }

            if (lstSach.Count <= 0)
            {
                TempData["message"] = "Không tìm thấy thông tin sách";
                return View("Index", lstSach);
            }

            TempData["message"] = $"Tìm thấy {lstSach.Count()} kết quả";
            return View("Index", lstSach);
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

        // GET View Create
        public IActionResult Create()
        {
            var theLoais = from tl in _db.TheLoais
                          select tl;

            foreach(var item in theLoais)
            {
                TheLoaiSelectList.Add(new SelectListItem { Text = item.TenTheLoai, Value = item.ID_TheLoai.ToString() });
            }

            InfoBook infoBook = new InfoBook();
            infoBook.TheLoaiSelectList = TheLoaiSelectList;

            return View(infoBook);
        }

        // POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InfoBook infoBook)
        {
            int idTacGia;
            // Kiểm tra tác giả tồn tại trong csld chưa
            // True thì thêm sách mới
            // False thì thêm tác giả mới vào csdl sau đó thêm thông tin sách
            bool isTg = checkTacGia(infoBook.Sach.TacGia.TenTacGia, infoBook.Sach.TacGia.QuocGia);
            if (isTg)
            {
                idTacGia = selectIdTacGia(infoBook.Sach.TacGia.TenTacGia, infoBook.Sach.TacGia.QuocGia);
                Sach sach = new Sach();
                sach.TenSach = infoBook.Sach.TenSach.ToUpper();
                sach.GiaTien = infoBook.Sach.GiaTien;
                sach.SoLuong = infoBook.Sach.SoLuong;
                sach.UrlImg = infoBook.Sach.UrlImg;
                sach.NgayNhap = DateTime.Now;
                sach.ID_TacGia = idTacGia;
                sach.ID_TheLoai = infoBook.Sach.TheLoai.ID_TheLoai;
                // Thêm sách mới
                try
                {
                    await _db.Saches.AddAsync(sach);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Thêm sách mới thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.InnerException.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                bool isAddTacGia = addTacGia(infoBook.Sach.TacGia);
                if (isAddTacGia)
                {
                    idTacGia = selectIdTacGia(infoBook.Sach.TacGia.TenTacGia, infoBook.Sach.TacGia.QuocGia);
                    Sach sach = new Sach();
                    sach.TenSach = infoBook.Sach.TenSach.ToUpper();
                    sach.GiaTien = infoBook.Sach.GiaTien;
                    sach.SoLuong = infoBook.Sach.SoLuong;
                    sach.UrlImg = infoBook.Sach.UrlImg;
                    sach.NgayNhap = DateTime.Now;
                    sach.ID_TacGia = idTacGia;
                    sach.ID_TheLoai = infoBook.Sach.TheLoai.ID_TheLoai;
                    // Thêm sách mới
                    try
                    {
                        await _db.Saches.AddAsync(sach);
                        await _db.SaveChangesAsync();
                        TempData["success"] = "Thêm sách mới thành công";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = ex.InnerException.Message;
                        return RedirectToAction("Index");
                    }
                }
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
        
        // Get View edit
        public IActionResult ViewEdit(int? id)
        {
            var dataSach = from s in _db.Saches
                           from tg in _db.TacGias
                           from tl in _db.TheLoais
                           where (s.ID_TacGia == tg.ID_TacGia && s.ID_TheLoai == tl.ID_TheLoai && s.ID_Sach == id)
                           select new
                           {
                               sach = new Sach(s.ID_Sach, s.TenSach, s.GiaTien, s.SoLuong, s.UrlImg, s.NgayNhap),
                               tacGia = new TacGia(tg.ID_TacGia, tg.TenTacGia, tg.QuocGia),
                               theLoai = new TheLoai(tl.ID_TheLoai, tl.TenTheLoai)
                           };

            var theLoais = from tl in _db.TheLoais
                           select tl;
            foreach (var item in theLoais)
            {
                TheLoaiSelectList.Add(new SelectListItem { Text = item.TenTheLoai, Value = item.ID_TheLoai.ToString() });
            }

            InfoBook info = new InfoBook();
            info.TheLoaiSelectList = TheLoaiSelectList;

            foreach (var item in dataSach)
            {
                info.Sach = item.sach;
                info.TacGia = item.tacGia;
                info.TheLoai = item.theLoai;
            }

            return View(info);
        }
        
        // Post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(InfoBook info) 
        {
            try
            {
                int idTacGia;
                // Kiểm tra tác giả tồn tại trong csld chưa
                // True thì sửa thông tin sách
                // False thì thêm tác giả mới vào csdl sau đó sửa thông tin sách
                bool isTacGia = checkTacGia(info.TacGia.TenTacGia, info.TacGia.QuocGia);
                if (isTacGia)
                {
                    // Lấy id của tác giả trong csdl
                    idTacGia = selectIdTacGia(info.TacGia.TenTacGia, info.TacGia.QuocGia);
                    Sach sach = info.Sach;
                    sach.TenSach = info.Sach.TenSach.ToUpper();
                    sach.UrlImg = info.Sach.UrlImg;
                    sach.ID_TacGia = idTacGia;
                    sach.ID_TheLoai = info.TheLoai.ID_TheLoai;
                    // Cập nhật csdl
                    try
                    {
                        _db.Saches.Update(sach);
                        await _db.SaveChangesAsync();
                        TempData["success"] = "Cập nhật thông tin sách thành công";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = ex.InnerException.Message;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    bool isAddTacGia = addTacGia(info.TacGia);
                    if (isAddTacGia)
                    {
                        idTacGia = selectIdTacGia(info.TacGia.TenTacGia, info.TacGia.QuocGia);
                        Sach sach = info.Sach;
                        sach.TenSach = info.Sach.TenSach.ToUpper();
                        sach.UrlImg = info.Sach.UrlImg;
                        sach.ID_TacGia = idTacGia;
                        sach.ID_TheLoai = info.TheLoai.ID_TheLoai;
                        // Sửa thông tin sách
                        try
                        {
                            _db.Saches.Update(sach);
                            await _db.SaveChangesAsync();
                            TempData["success"] = "Cập nhật thông tin sách thành công";
                            return RedirectToAction("Index");
                        }
                        catch (Exception ex)
                        {
                            TempData["error"] = ex.InnerException.Message;
                            return RedirectToAction("Index");
                        }
                    }
                }
            } catch(Exception ex)
            {
                TempData["error"] = ex.InnerException.Message;
                return RedirectToAction("Index");
            }
            return BadRequest();
        }

        // Get View Delete
        public IActionResult Delete(int? id)
        {
            var dataSach = from s in _db.Saches
                           from tg in _db.TacGias
                           from tl in _db.TheLoais
                           where (s.ID_TacGia == tg.ID_TacGia && s.ID_TheLoai == tl.ID_TheLoai && s.ID_Sach == id)
                           select new
                           {
                               sach = new Sach(s.ID_Sach, s.TenSach, s.GiaTien, s.SoLuong, s.UrlImg, s.NgayNhap),
                               tacGia = new TacGia(tg.ID_TacGia, tg.TenTacGia, tg.QuocGia),
                               theLoai = new TheLoai(tl.ID_TheLoai, tl.TenTheLoai)
                           };
            InfoBook info = new InfoBook();
             
            foreach (var item in dataSach)
            {
                info.Sach = item.sach;
                info.TacGia = item.tacGia;
                info.TheLoai = item.theLoai;
            }

            return View(info);
        }

        // Post Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            try
            {
                var obj = _db.Saches.Find(id);
                if (obj == null) return NotFound();
                _db.Saches.Remove(obj);
                await _db.SaveChangesAsync();
                TempData["success"] = "Xóa sách thành công";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.InnerException.Message;
                return RedirectToAction("Index");
            }
        }

        // GET View Details
        public IActionResult Details(int? id)
        {
            var obj = _db.Saches.Find(id);
            if(obj == null) return NotFound();

            var tenTacGia = from tg in _db.TacGias
                            where tg.ID_TacGia == obj.ID_TacGia
                            select tg.TenTacGia;
            var tenTheLoai = from tl in _db.TheLoais
                             where tl.ID_TheLoai == obj.ID_TheLoai
                             select tl.TenTheLoai;

            InfoBook info = new InfoBook();
            info.Sach = obj;

            foreach(var tg in tenTacGia)
            {
                info.TenTacGia = tg.ToString();
            }

            foreach(var tl in tenTheLoai)
            {
                info.TenTheLoai = tl.ToString();
            }

            return View(info);
        }
    }
}

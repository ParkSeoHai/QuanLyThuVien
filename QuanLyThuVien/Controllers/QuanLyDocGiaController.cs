using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class QuanLyDocGiaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public QuanLyDocGiaController(ApplicationDbContext db)
        {
            _db = db;
        }


        // GET View Index
        public IActionResult Index()
        {
            IQueryable<DocGia> lstDG = getDataDocGia();
            if(lstDG.Count() <= 0)
            {
                TempData["message"] = "Dữ liệu trống";
            }
            return View(lstDG);
        }

        // Phương thức get data DocGia
        public IQueryable<DocGia> getDataDocGia()
        {
            var obj = from dg in _db.DocGias
                      select dg;
            return obj;
        }

        // GET view Create
        public IActionResult Create()
        {
            return View();
        }

        // POST create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(DocGia docGia)
        {
            try
            {
                if (docGia == null) { return BadRequest(); }
                bool isAddTaiKhoan = addTaiKhoanDocGia(docGia.Email);
                bool isAddTheThuVien = addTheThuVien(docGia.SoCCCD);

                if (isAddTaiKhoan && isAddTheThuVien)
                {
                    try
                    {
                        int maTK = getMaTK(docGia.Email);
                        int maThe = getMaThe(docGia.SoCCCD);
                        if (maTK != 0 && maThe != 0)
                        {
                            DocGia dg = docGia;
                            dg.ID_TaiKhoan = maTK;
                            dg.ID_The = maThe;

                            await _db.DocGias.AddAsync(dg);
                            await _db.SaveChangesAsync();

                            TempData["success"] = "Thêm mới độc giả thành công";
                            return RedirectToAction("Index");
                        }
                    } catch(Exception ex)
                    {
                        removeTaiKhoan(docGia.Email);
                        removeTheThuVien(docGia.SoCCCD);

                        TempData["error"] = ex.InnerException.Message;
                        return RedirectToAction("Index");
                    }
                }
            } catch(Exception ex)
            {
                TempData["error"] = ex.InnerException.Message;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET view Edit
        public IActionResult Edit(int? id)
        {
            var obj = from dg in _db.DocGias
                      where dg.ID_DocGia == id
                      select dg;

            foreach (var docgia in obj)
            {
                return View(docgia);
            }

            return NotFound();
        }

        // Post Edit
        public async Task<IActionResult> EditPost(DocGia obj)
        {
            try
            {
                _db.DocGias.Update(obj);
                await _db.SaveChangesAsync();
                TempData["success"] = "Sửa thông tin độc giả thành công";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.InnerException.Message;
                return RedirectToAction("Index");
            }
        }

        // Get view delete
        public IActionResult Delete(int? id)
        {
            var obj = _db.DocGias.Find(id);
            if (obj == null) return NotFound();
            return View(obj);
        }

        // Post Delete
        public async Task<IActionResult> DeletePost(int? idDocGia)
        {
            var obj = _db.DocGias.Find(idDocGia);
            if (obj == null) return NotFound();
            try
            {
                _db.DocGias.Remove(obj);
                await _db.SaveChangesAsync();

                /*
                    Khi xóa độc giả thì sẽ xóa luôn cả thông tin tài khoản 
                    và thẻ thư viện của độc giả đó.
                 */
                removeTaiKhoan(obj.Email);
                removeTheThuVien(obj.SoCCCD);

                TempData["success"] = "Xóa độc giả thành công";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.InnerException.Message;
                return RedirectToAction("Index");
            }
        }

        /*
            Phương thức tạo tài khoản khi thêm mới độc giả
            Tên tài khoản = email
            Mật khẩu = 123456;
         */
        public bool addTaiKhoanDocGia(string email)
        {
            if(email == null) { return false; }
            TaiKhoan tk = new TaiKhoan();
            tk.TenDangNhap = email;
            tk.MatKhau = "123456";
            tk.VaiTro = "Độc giả";

            _db.TaiKhoans.AddAsync(tk);
            if(_db.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        // Phương thức lấy mã tài khoản độc giả
        public int getMaTK(string tentk)
        {
            var maTK = from tk in _db.TaiKhoans
                       where tk.TenDangNhap == tentk
                       select tk.ID_TaiKhoan;
            foreach(var item in maTK)
            {
                return item;
            }

            return 0;
        }

        /*
            Phương thức tạo thẻ thư viện khi thêm mới độc giả
            NgayDB = DateTime.Now;
            Thời hạn của 1 thẻ là 3 năm kể từ ngày tạo
            Lưu thông tin ghi chú = số cccd
         */
        public bool addTheThuVien(string SoCCCD)
        {
            TheThuVien the = new TheThuVien();
            the.NgayBD = DateTime.Now;
            the.NgayHetHan = new DateTime(the.NgayBD.Year + 3, the.NgayBD.Month, the.NgayBD.Day);
            the.GhiChu = SoCCCD;

            _db.TheThuViens.AddAsync(the);
            if (_db.SaveChanges() > 0) return true;
            return false;
        }

        // Phương thức lấy mã thẻ thư viện
        public int getMaThe(string soCCCD)
        {
            var maThes = from th in _db.TheThuViens
                         where th.GhiChu == soCCCD
                         select th.ID_The;
            foreach(var item in maThes)
            {
                return item;
            }

            return 0;
        }

        // Phương thức xóa thông tin tài khoản nếu thêm độc giả xảy ra lỗi
        public void removeTaiKhoan(string email)
        {
            var obj = from tk in _db.TaiKhoans
                      where tk.TenDangNhap == email
                      select tk;
            TaiKhoan taiKhoan = new TaiKhoan();
            foreach(var i in obj)
            {
                taiKhoan = i;
            }
            _db.Remove(taiKhoan);
            _db.SaveChanges();
        }

        // Phương thức xóa thông tin thẻ thư viện nếu thêm độc giả xảy ra lỗi
        public void removeTheThuVien(string soCCCD)
        {
            var obj = from t in _db.TheThuViens
                      where t.GhiChu == soCCCD
                      select t;

            TheThuVien the = new TheThuVien();
            foreach (var i in obj)
            {
                the = i;
            }
            _db.Remove(the);
            _db.SaveChanges();
        }
    }
}

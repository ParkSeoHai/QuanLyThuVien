using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class QuanTriHeThongController : Controller
    {
        private readonly ApplicationDbContext _db;
        private string vaiTro = LoginController.layout.VaiTro;
        public QuanTriHeThongController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET
        public IActionResult Index()
        {
            if(vaiTro == "Admin")
            {
                IQueryable<TaiKhoan> dataTaiKhoan = getDataTaiKhoan();
                if (dataTaiKhoan.Count() <= 0)
                {
                    TempData["message"] = "Dữ liệu trống";
                }
                return View(dataTaiKhoan);
            }
            TempData["message"] = "Bạn không có quyền truy cập vào chức năng này!!!";
            return RedirectToAction("Index", "Home");
        }

        // GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaiKhoan obj, string confirmPassword)
        {
            string pass = obj.MatKhau;
            string confirm = confirmPassword;

            if(pass != confirm)
            {
                ModelState.AddModelError("MatKhau", "Mật khẩu xác nhận không đúng");
            } else
            {
                try
                {
                    _db.TaiKhoans.Add(obj);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Thêm tài khoản thành công";
                    return RedirectToAction("Index");
                } catch (Exception ex)
                {
                    TempData["error"] = ex.InnerException.Message;
                    return View(obj);
                }
            }

            return View(obj);
        }

        // GET
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var taiKhoanFromDb = _db.TaiKhoans.Find(id);
            if(taiKhoanFromDb == null) { return NotFound(); }
            return View(taiKhoanFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaiKhoan obj, string confirmPassword)
        {
            string pass = obj.MatKhau;
            string confirm = confirmPassword;
            if (pass != confirm)
            {
                ModelState.AddModelError("MatKhau", "Mật khẩu xác nhận không đúng");
            }
            else
            {
                try
                {
                    _db.TaiKhoans.UpdateRange(obj);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Sửa thông tin tài khoản thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.InnerException.Message;
                    return View(obj);
                }
            }

            return View(obj);
        }

        // GET
        public IActionResult Delete(int? id) 
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var taiKhoanFromDb = _db.TaiKhoans.Find(id);
            if (taiKhoanFromDb == null) { return NotFound(); }
            return View(taiKhoanFromDb);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id_taikhoan)
        {
            var obj = _db.TaiKhoans.Find(id_taikhoan);
            try
            {
                if (obj == null) { return NotFound(); }
                _db.TaiKhoans.Remove(obj);
                await _db.SaveChangesAsync();
                TempData["success"] = "Xóa thông tin tài khoản thành công";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.InnerException.Message;
                return View(obj);
            }
        }


        // Method get data table TaiKhoan
        public IQueryable<TaiKhoan> getDataTaiKhoan()
        {
            var obj = from tk in _db.TaiKhoans
                      where tk.VaiTro != "Admin"
                      select tk;
            return obj;
        }
    }
}

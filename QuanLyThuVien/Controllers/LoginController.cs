﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LoginController(ApplicationDbContext db)
        {
            _db = db;
        }

        public static int id;
        // Model lưu thông tin tên đăng nhập, vai trò dùng chuyển dữ liệu qua _Layout.cshtml
        public static LayoutModel layout;

        public IActionResult Index()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string taikhoan, string matkhau)
        {
            string username = taikhoan;
            string password = matkhau;
            // Truy van linq kiem tra tai khoan dang nhap
            var obj = from tk in _db.TaiKhoans
                      where (tk.TenDangNhap == username && tk.MatKhau == password)
                      select tk;
            foreach (var taiKhoan in obj)
            {
                if (taiKhoan.VaiTro == "Admin" || taiKhoan.VaiTro == "Thủ thư")
                {
                    id = taiKhoan.ID_TaiKhoan;

                    layout = new LayoutModel(taiKhoan.TenDangNhap, taiKhoan.VaiTro);
                    return RedirectToAction("Index", "Home");

                    /*return RedirectToRoute(new
                    {
                        Controller = "Home",
                        Action = "Index"
                    });*/

                    /*return View("Views/Home/Index.cshtml", id);*/
                } else
                {
                    TempData["message"] = "Bạn không có quyền truy cập vào hệ thống!!!";
                    return View("Index");
                }
            }
            TempData["error"] = "Tài khoản hoặc mật khẩu không chính xác!!!";
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            return View("Index");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class QuanTriHeThongController : Controller
    {
        private readonly ApplicationDbContext _db;
        public QuanTriHeThongController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

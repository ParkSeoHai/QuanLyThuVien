using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    public class QuanLyMuonTraSachController : Controller
    {
        private readonly ApplicationDbContext _db;
        public QuanLyMuonTraSachController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Mã tài khoản đăng nhập
        private int idDN = LoginController.id;

        // Get view sách đang mượn
        public IActionResult ViewSachMuon()
        {
            LinkedList<SachMuon> lstPhieuMuon = new LinkedList<SachMuon>();
            var dataPhieuMuons = from tt in _db.ThuThus
                                from pm in _db.PhieuMuons
                                from ctpm in _db.CTPhieuMuon
                                from dg in _db.DocGias
                                from s in _db.Saches
                                from ttv in _db.TheThuViens
                                where (pm.ID_PhieuMuon == ctpm.ID_PhieuMuon && pm.ID_The == ttv.ID_The
                                      && pm.ID_ThuThu == tt.ID_ThuThu && ttv.ID_The == dg.ID_The 
                                      && ctpm.ID_Sach == s.ID_Sach && ctpm.TrangThai == 0)
                                select new
                                {
                                    MaPM = pm.ID_PhieuMuon,
                                    TenNguoiTao = tt.TenThuThu,
                                    MaTheTV = ttv.ID_The,
                                    TenDg = dg.TenDocGia,
                                    MaSach = s.ID_Sach,
                                    TenSach = s.TenSach,
                                    SoLuongMuon = ctpm.SoLuongMuon,
                                    NgayTaoPM = pm.NgayTaoPhieu,
                                    NgayHenTra = pm.NgayHenTra,
                                    GhiChuMuon = pm.GhiChuMuon
                                };

            foreach(var data in dataPhieuMuons)
            {
                SachMuon obj = new SachMuon(data.MaPM, data.TenNguoiTao, data.MaTheTV, data.TenDg, data.MaSach, 
                    data.TenSach, data.SoLuongMuon, data.NgayTaoPM, data.NgayHenTra, data.GhiChuMuon);
                lstPhieuMuon.AddLast(obj);
            }

            return View(lstPhieuMuon);
        }

        // Get view lập phiếu mượn
        public IActionResult CreatePhieuMuon()
        {
            return View();
        }

        // Post lập phiếu mượn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SachMuon model)
        {
            // Kiểm tra mã thẻ thư viện, mã sách có tồn tại không
            var theThuVien = _db.TheThuViens.Find(model.MaTheThuVien);
            var sach = _db.Saches.Find(model.MaSach);
            // True nếu số lượng sách mượn lớn hơn số lượng sách tồn kho
            bool isSoLuongSach = sach.SoLuong - model.SoLuongMuon < 0 ? true : false;

            if (theThuVien == null)
                ModelState.AddModelError("MaTheThuVien", "Mã thẻ thư viện không tồn tại!");
            if (sach == null)
                ModelState.AddModelError("MaSach", "Mã sách không tồn tại!");
            if (model.SoLuongMuon <= 0)
                ModelState.AddModelError("SoLuongMuon", "Số lượng mượn phải lớn hơn 0");
             if (isSoLuongSach)
                ModelState.AddModelError("SoLuongMuon", "Số lượng sách trong kho không đủ!");

            if (theThuVien != null && sach != null && !isSoLuongSach && model.SoLuongMuon > 0)
            {
                try
                {
                    // Lấy mã thủ thư
                    var thuthu = from tt in _db.ThuThus
                                 where tt.ID_TaiKhoan == idDN
                                 select tt;
                    int maThuThu = 0;
                    foreach (var tt in thuthu)
                    {
                        maThuThu = tt.ID_ThuThu;
                    }

                    // Thêm dữ liệu vào bảng phiếu mượn
                    PhieuMuon pm = new PhieuMuon();
                    pm.NgayTaoPhieu = model.NgayTaoPhieu;
                    pm.NgayHenTra = model.NgayHenTra;
                    pm.ID_ThuThu = maThuThu;
                    pm.ID_The = model.MaTheThuVien;
                    if (string.IsNullOrEmpty(model.GhiChuMuon))
                        pm.GhiChuMuon = "Trống";
                    else
                        pm.GhiChuMuon = model.GhiChuMuon;

                    await _db.PhieuMuons.AddAsync(pm);
                    await _db.SaveChangesAsync();

                    // Thêm dữ liệu vào bảng chi tiết phiếu mượn
                    CTPhieuMuon ctpm = new CTPhieuMuon();
                    // Lấy mã phiếu mượn vừa thêm vào cơ sở dữ liệu
                    int maPM = _db.PhieuMuons.Max(id => id.ID_PhieuMuon);
                    ctpm.ID_PhieuMuon = maPM;
                    ctpm.ID_Sach = model.MaSach;
                    ctpm.GhiChuTra = "Trống";
                    // Trạng thái: 0 -> đang mượn, 1 -> đã trả
                    ctpm.TrangThai = 0;
                    ctpm.SoLuongMuon = model.SoLuongMuon;
                    await _db.CTPhieuMuon.AddAsync(ctpm);
                    await _db.SaveChangesAsync();

                    // Sửa lại số lượng sách khi lập phiếu mượn
                    UpdateSach(model.MaSach, model.SoLuongMuon);

                    TempData["success"] = "Lập phiếu mượn thành công";
                    return RedirectToAction("ViewSachMuon");
                } catch(Exception ex)
                {
                    TempData["error"] = ex.InnerException.Message;
                    return View("CreatePhieuMuon", model);
                }

            }
            return View("CreatePhieuMuon", model);
        }

        // Phương thức cập nhật số lượng sách khi tạo phiếu mượn
        public async Task UpdateSach(int maSach, int soLuongMuon)
        {
            var sachObj = _db.Saches.Find(maSach);
            int soLuongNew = sachObj.SoLuong - soLuongMuon;
            if(sachObj != null)
            {
                sachObj.SoLuong = soLuongNew;
                _db.Saches.Update(sachObj);
                await _db.SaveChangesAsync();
            }
        }

        // Phương thức cập nhật số lượng sách khi sửa phiếu mượn
        public async Task UpdateSach(int maSach, int soLuongOld, int soLuongNew)
        {
            var sachObj = _db.Saches.Find(maSach);
            int soLuong = sachObj.SoLuong - soLuongNew + soLuongOld;
            if (sachObj != null)
            {
                sachObj.SoLuong = soLuong;
                _db.Saches.Update(sachObj);
                await _db.SaveChangesAsync();
            }
        }

        // Phương thức cập nhật số lượng sách khi xóa phiếu mượn
        public async Task UpdateSachWhenDelete(int maSach, int soLuongMuon)
        {
            Sach? s = _db.Saches.Find(maSach);
            int soLuongNew = s.SoLuong + soLuongMuon;
            if (s != null)
            {
                s.SoLuong = soLuongNew;
                _db.Saches.Update(s);
                await _db.SaveChangesAsync();
            }
        }

        // Get view edit 
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var data = from tt in _db.ThuThus
                      from pm in _db.PhieuMuons
                      from ctpm in _db.CTPhieuMuon
                      from dg in _db.DocGias
                      from s in _db.Saches
                      from ttv in _db.TheThuViens
                      where (pm.ID_PhieuMuon == ctpm.ID_PhieuMuon && pm.ID_The == ttv.ID_The
                            && pm.ID_ThuThu == tt.ID_ThuThu && ttv.ID_The == dg.ID_The
                            && ctpm.ID_Sach == s.ID_Sach && pm.ID_PhieuMuon == id)
                      select new
                      {
                          MaPM = pm.ID_PhieuMuon,
                          TenNguoiTao = tt.TenThuThu,
                          MaTheTV = ttv.ID_The,
                          TenDg = dg.TenDocGia,
                          MaSach = s.ID_Sach,
                          TenSach = s.TenSach,
                          SoLuongMuon = ctpm.SoLuongMuon,
                          NgayTaoPM = pm.NgayTaoPhieu,
                          NgayHenTra = pm.NgayHenTra,
                          GhiChuMuon = pm.GhiChuMuon
                      };

            SachMuon? obj = new SachMuon();
            foreach(var i in data)
            {
                obj = new SachMuon(i.MaPM, i.TenNguoiTao, i.MaTheTV, i.TenDg, i.MaSach,
                    i.TenSach, i.SoLuongMuon, i.NgayTaoPM, i.NgayHenTra, i.GhiChuMuon);
            }

            if (obj == null)
                return NotFound();

            return View(obj);
        }

        // Post edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SachMuon model)
        {
            // Lấy số sách mượn của bảng CTPhieuMuon khi chưa sửa chi tiết phiếu mượn
            var CTPhieuMuon = from ctpm in _db.CTPhieuMuon
                             where ctpm.ID_PhieuMuon == model.MaPhieuMuon
                             select ctpm.SoLuongMuon;
            int soLuongOld = 0;
            foreach(var data in CTPhieuMuon)
            {
                soLuongOld = data;
                break;
            }

            // Kiểm tra mã thẻ thư viện, mã sách có tồn tại không
            var theThuVien = _db.TheThuViens.Find(model.MaTheThuVien);
            var sach = _db.Saches.Find(model.MaSach);
            // True nếu số lượng sách mượn lớn hơn số lượng sách tồn kho
            bool isSoLuongSach = sach.SoLuong - model.SoLuongMuon + soLuongOld < 0 ? true : false;

            if (theThuVien == null)
                ModelState.AddModelError("MaTheThuVien", "Mã thẻ thư viện không tồn tại!");
            if (sach == null)
                ModelState.AddModelError("MaSach", "Mã sách không tồn tại!");
            if (model.SoLuongMuon <= 0)
                ModelState.AddModelError("SoLuongMuon", "Số lượng mượn phải lớn hơn 0");
            if (isSoLuongSach)
                ModelState.AddModelError("SoLuongMuon", "Số lượng sách trong kho không đủ!");

            if (theThuVien != null && sach != null && !isSoLuongSach && model.SoLuongMuon > 0)
            {
                try
                {
                    // Lấy mã thủ thư
                    var thuthu = from tt in _db.ThuThus
                                 where tt.ID_TaiKhoan == idDN
                                 select tt;
                    int maThuThu = 0;
                    foreach (var tt in thuthu)
                    {
                        maThuThu = tt.ID_ThuThu;
                    }

                    // Sửa dữ liệu bảng phiếu mượn
                    PhieuMuon pm = new PhieuMuon();
                    pm.NgayTaoPhieu = model.NgayTaoPhieu;
                    pm.NgayHenTra = model.NgayHenTra;
                    pm.ID_ThuThu = maThuThu;
                    pm.ID_The = model.MaTheThuVien;
                    if (string.IsNullOrEmpty(model.GhiChuMuon))
                        pm.GhiChuMuon = "Trống";
                    else
                        pm.GhiChuMuon = model.GhiChuMuon;

                    _db.PhieuMuons.Update(pm);
                    await _db.SaveChangesAsync();

                    // Sửa dữ liệu bảng chi tiết phiếu mượn
                    CTPhieuMuon ctpm = new CTPhieuMuon();
                    ctpm.ID_PhieuMuon = model.MaPhieuMuon;
                    ctpm.ID_Sach = model.MaSach;
                    ctpm.GhiChuTra = "Trống";
                    // Trạng thái: 0 -> đang mượn, 1 -> đã trả
                    ctpm.TrangThai = 0;
                    ctpm.SoLuongMuon = model.SoLuongMuon;
                    _db.CTPhieuMuon.Update(ctpm);
                    await _db.SaveChangesAsync();

                    // Sửa lại số lượng sách khi sửa phiếu mượn
                    UpdateSach(model.MaSach, soLuongOld, model.SoLuongMuon);

                    TempData["success"] = "Sửa phiếu mượn thành công";
                    return RedirectToAction("ViewSachMuon");
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.InnerException.Message;
                    return View("CreatePhieuMuon", model);
                }

            }
            return View(model);
        }

        // Get view Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var data = from tt in _db.ThuThus
                       from pm in _db.PhieuMuons
                       from ctpm in _db.CTPhieuMuon
                       from dg in _db.DocGias
                       from s in _db.Saches
                       from ttv in _db.TheThuViens
                       where (pm.ID_PhieuMuon == ctpm.ID_PhieuMuon && pm.ID_The == ttv.ID_The
                             && pm.ID_ThuThu == tt.ID_ThuThu && ttv.ID_The == dg.ID_The
                             && ctpm.ID_Sach == s.ID_Sach && pm.ID_PhieuMuon == id)
                       select new
                       {
                           MaPM = pm.ID_PhieuMuon,
                           TenNguoiTao = tt.TenThuThu,
                           MaTheTV = ttv.ID_The,
                           TenDg = dg.TenDocGia,
                           MaSach = s.ID_Sach,
                           TenSach = s.TenSach,
                           SoLuongMuon = ctpm.SoLuongMuon,
                           NgayTaoPM = pm.NgayTaoPhieu,
                           NgayHenTra = pm.NgayHenTra,
                           GhiChuMuon = pm.GhiChuMuon
                       };

            SachMuon? obj = new SachMuon();
            foreach (var i in data)
            {
                obj = new SachMuon(i.MaPM, i.TenNguoiTao, i.MaTheTV, i.TenDg, i.MaSach,
                    i.TenSach, i.SoLuongMuon, i.NgayTaoPM, i.NgayHenTra, i.GhiChuMuon);
            }

            if (obj == null)
                return NotFound();
            return View(obj);
        }

        // Post delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? maPhieuMuon)
        {
            if(maPhieuMuon != null)
            {
                try
                {
                    // Select dữ liệu bảng chi tiết phiếu mượn qua mã phiếu mượn
                    CTPhieuMuon? ctpm = _db.CTPhieuMuon.First(e => e.ID_PhieuMuon == maPhieuMuon);

                    // Select dữ liệu bảng phiếu mượn qua mã phiếu mượn
                    PhieuMuon? pm = _db.PhieuMuons.Find(maPhieuMuon);

                    if (ctpm != null && pm != null)
                    {
                        _db.Remove(ctpm);
                        _db.PhieuMuons.Remove(pm);
                        await _db.SaveChangesAsync();

                        await UpdateSachWhenDelete(ctpm.ID_Sach, ctpm.SoLuongMuon);

                        TempData["success"] = "Xóa phiếu mượn thành công";
                        return RedirectToAction("ViewSachMuon");
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.InnerException.Message;
                    return View("Delete", maPhieuMuon);
                }
            }
            return NotFound();
        }

        // Post trả sách
        public async Task<IActionResult> TraSach(int? id)
        {
            if (id == null) return NotFound();
            // Select dữ liệu bảng chi tiết phiếu mượn qua mã phiếu mượn
            CTPhieuMuon? ctpm = _db.CTPhieuMuon.First(e => e.ID_PhieuMuon == id);
            // 1 -> đã trả
            ctpm.TrangThai = 1;
            ctpm.NgayTra = DateTime.Now;

            _db.CTPhieuMuon.Update(ctpm);
            await _db.SaveChangesAsync();

            // Đợi cập nhật thông tin sách
            int maSach = _db.CTPhieuMuon.First(e => e.ID_PhieuMuon == id).ID_Sach;
            await UpdateSlSachKhiTra(maSach, ctpm.SoLuongMuon);

            TempData["success"] = "Xác nhận trả sách thành công";
            return RedirectToAction("ViewSachMuon");
        }

        // Phương thức thay đổi số lượng sách khi xác nhận trả sách
        public async Task UpdateSlSachKhiTra(int maSach, int slTra)
        {
            Sach? s = _db.Saches.Find(maSach);
            if(s != null)
            {
                s.SoLuong += slTra;
                _db.Saches.Update(s);
                await _db.SaveChangesAsync();
            }
        }

        // Get view sách đã trả
        public IActionResult ViewSachTra()
        {
            var obj = from s in _db.Saches
                      from dg in _db.DocGias
                      from ctpm in _db.CTPhieuMuon
                      from ttv in _db.TheThuViens
                      from pm in _db.PhieuMuons
                      where s.ID_Sach == ctpm.ID_Sach && dg.ID_The == ttv.ID_The && ctpm.ID_PhieuMuon == pm.ID_PhieuMuon && ttv.ID_The == pm.ID_The && ctpm.TrangThai == 1
                      select new
                      {
                          sachTra = new SachTra(ctpm.ID_PhieuMuon, s.ID_Sach, s.TenSach, ttv.ID_The, dg.TenDocGia, ctpm.SoLuongMuon, ctpm.NgayTra, ctpm.GhiChuTra)
                      };

            LinkedList<SachTra> lstSachTra = new LinkedList<SachTra>();
            foreach(var item in obj)
            {
                lstSachTra.AddLast(item.sachTra);
            }

            return View(lstSachTra);
        }
    }
}

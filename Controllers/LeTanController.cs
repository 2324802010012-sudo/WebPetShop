using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;

namespace WebPetShop.Controllers
{

    public class LeTanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeTanController(ApplicationDbContext context)
        {
            _context = context;
        }
        private void LoadThongBao()
        {
            int soDonCho = _context.DonHangs.Count(d => d.TrangThai == "Chờ xác nhận" || d.TrangThai == "Chờ duyệt");
            ViewBag.SoDonCho = soDonCho;
        }

        // ===============================
        //  XEM CHI TIẾT KÝ GỬI (LỄ TÂN)
        // ===============================
        public async Task<IActionResult> ChiTiet(int id)
        {
            var data = await _context.KyGuiThuCungs
                .Include(k => k.MaKhNavigation)       // khách hàng
                .Include(k => k.ChiTietChamSocs)      // nhật ký chăm sóc
                .FirstOrDefaultAsync(k => k.MaKyGui == id);

            if (data == null)
                return NotFound();

            return View("ChiTiet", data);
        }


        // =================== TRANG CHÍNH ===================
        public IActionResult Index()
        {


            if (HttpContext.Session.GetString("Role") != "LeTan")
                return RedirectToAction("Login", "Auth");

            // ✅ Đếm số lượng cần xử lý
            ViewBag.DonHangMoi = _context.DonHangs.Count(d => d.TrangThai == "Chờ duyệt");
            ViewBag.KyGuiMoi = _context.KyGuiThuCungs.Count(k => k.TrangThai == "Chờ xác nhận");
            ViewBag.BaiNhanNuoiMoi = _context.BaiDangNhanNuois.Count(b => b.TrangThai == "Chờ duyệt");
            ViewBag.HoTroMoi = _context.LichSuHeThongs.Count(h => h.HanhDong.Contains("Yêu cầu hỗ trợ"));
            LoadThongBao();
            return View();
        }

        // =================== ĐƠN HÀNG ===================
     
        public IActionResult DonHang()
        {
            LoadThongBao();
            // ✅ Lấy danh sách đơn hàng
            var donHangs = _context.DonHangs
                .Include(d => d.MaNguoiDungNavigation)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

            // ✅ Đếm số đơn chờ xác nhận
            int soDonCho = donHangs.Count(d => d.TrangThai == "Chờ xác nhận" || d.TrangThai == "Chờ duyệt");
            ViewBag.SoDonCho = soDonCho;

            return View(donHangs);
        }


        // 🧾 Xem chi tiết
        public IActionResult ChiTietDon(int id)
        {
            var donHang = _context.DonHangs
                .Include(d => d.MaNguoiDungNavigation)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(d => d.MaDh == id);

            if (donHang == null)
                return NotFound();

            return View(donHang);
        }

        // ✅ Lễ tân xác nhận đơn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XacNhanDon(int id)
        {
            var don = _context.DonHangs
                .FirstOrDefault(d => d.MaDh == id);

            if (don == null)
                return Json(new { success = false, message = "Không tìm thấy đơn!" });

            // Cập nhật trạng thái
            don.TrangThai = "Đã xác nhận";

            _context.Update(don);
            _context.SaveChanges();   // ⬅️ Lưu vào database thật

            return Json(new { success = true });
        }


        // =================== KÝ GỬI ===================

        public IActionResult KyGui()
        {
            LoadThongBao();
            var kyGuiList = _context.KyGuiThuCungs
                .Include(k => k.MaKhNavigation)
                .OrderByDescending(k => k.MaKyGui)
                .ToList();

            ViewBag.KyGuiMoi = kyGuiList.Count(k => k.TrangThaiDon == "ChoXacNhan");
            return View(kyGuiList);
        }
        [HttpGet]
        public IActionResult GetCountGiaHanMoi()
        {
            int count = _context.KyGuiThuCungs
                .Count(k => k.TrangThai == "Gia hạn mới");

            return Json(count);
        }
        // 2. LỄ TÂN XÁC NHẬN TIẾP NHẬN THÚ + BẮT ĐẦU KÝ GỬI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhanKyGui(int id)
        {
            var kg = await _context.KyGuiThuCungs.FindAsync(id);
            if (kg == null)
            {
                TempData["Error"] = "Không tìm thấy đơn ký gửi!";
                return RedirectToAction("KyGui");
            }

            kg.TrangThaiDon = "DangChamSoc";         // → Đang ký gửi / Đang chăm sóc
            kg.TrangThaiThanhToan = "DaThanhToan";   // đánh dấu đã thu tiền
            // kg.NgayBatDauChamSoc = DateTime.Now;  // nếu bạn có cột này thì thêm

            _context.Update(kg);
            await _context.SaveChangesAsync();

            // Ghi log hành động (tùy chọn)
            _context.LichSuHeThongs.Add(new LichSuHeThong
            {
                MaNguoiDung = int.Parse(HttpContext.Session.GetString("UserId") ?? "0"),
                HanhDong = $"Lễ tân xác nhận ký gửi thú cưng '{kg.TenThuCung}' - Mã #{kg.MaKyGui}",
                NgayThucHien = DateTime.Now
            });
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã tiếp nhận thú cưng '{kg.TenThuCung}' – Bắt đầu ký gửi thành công!";
            return RedirectToAction("KyGui");
        }

        // 3. LỄ TÂN TRẢ THÚ CHO KHÁCH (khi khách đến nhận)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TraThuKyGui(int id)
        {
            var kg = await _context.KyGuiThuCungs.FindAsync(id);

            if (kg == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn ký gửi!" });
            }

            // Chỉ cho trả thú khi đang chăm sóc
            if (kg.TrangThaiDon != "DangChamSoc" && kg.TrangThaiDon != "GiaHanMoi")
            {
                return Json(new { success = false, message = "Đơn không ở trạng thái chăm sóc, không thể trả thú!" });
            }

            // ===== CẬP NHẬT TRẠNG THÁI =====
            kg.TrangThaiDon = "HoanThanh";

            // Nếu bảng có cột NgayTraThucTe thì dùng:
            // kg.NgayTraThucTe = DateOnly.FromDateTime(DateTime.Today);

            _context.Update(kg);

            // ===== GHI LOG =====
            int maNguoiDung = int.TryParse(HttpContext.Session.GetString("UserId"), out var tmp)
                                ? tmp : 0;

            _context.LichSuHeThongs.Add(new LichSuHeThong
            {
                MaNguoiDung = maNguoiDung,
                HanhDong = $"Trả thú cưng '{kg.TenThuCung}' - Mã ký gửi #{kg.MaKyGui}",
                NgayThucHien = DateTime.Now
            });

            // Lưu 1 lần là đủ
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Trả thú thành công!" });
        }

        // =================== NHẬN NUÔI ===================
        public IActionResult BaiNhanNuoi()
        {
            ViewBag.LichSuNhanNuoi = _context.BaiDangNhanNuois.Count(b => b.TrangThai == "Chờ duyệt");
            return View(_context.BaiDangNhanNuois.Include(b => b.MaNguoiTaoNavigation).ToList());
        }

        // =================== HỖ TRỢ KHÁCH HÀNG ===================
        public IActionResult HoTroKhachHang()
        {
            ViewBag.HoTroMoi = _context.LichSuHeThongs.Count(h => h.HanhDong.Contains("Yêu cầu hỗ trợ"));
            return View(_context.LichSuHeThongs.Include(h => h.MaNguoiDungNavigation).ToList());
        }
        // =================== LỄ TÂN: QUẢN LÝ YÊU CẦU NHẬN NUÔI ===================
        //public async Task<IActionResult> LichSuNhanNuoi()
        //{
        //    if (HttpContext.Session.GetString("Role") != "LeTan")
        //        return RedirectToAction("Login", "Auth");

        //    var data = await _context.YeuCauNhanNuois
        //        // Thêm dòng này để luôn lấy dữ liệu mới nhất
        //        .Include(y => y.BaiDangNavigation!)
        //            .ThenInclude(b => b.MaThuCungNhanNuoiNavigation!)
        //            .ThenInclude(t => t.MaKyGuiNavigation)
        //        .Include(y => y.NguoiDungNavigation)
        //        .OrderByDescending(y => y.NgayYeuCau)
        //        .ToListAsync();

        //    return View(data);
        //}
        public async Task<IActionResult> LichSuNhanNuoi()
        {
            if (HttpContext.Session.GetString("Role") != "LeTan")
                return RedirectToAction("Login", "Auth");

            var data = await _context.YeuCauNhanNuois
                .Include(y => y.BaiDangNavigation!)
                    .ThenInclude(b => b.MaThuCungNhanNuoiNavigation!)
                    .ThenInclude(t => t.MaKyGuiNavigation)
                .Include(y => y.NguoiDungNavigation)
                .OrderByDescending(y => y.NgayYeuCau)
                .ToListAsync();

            return View("~/Views/LeTan/LichSuNhanNuoi.cshtml", data);
        }

        [HttpPost]
public async Task<JsonResult> XacNhanNhanNuoi(int maYeuCau)
{
    var yc = await _context.YeuCauNhanNuois
        .Include(y => y.BaiDangNavigation)!
        .ThenInclude(b => b.MaThuCungNhanNuoiNavigation!)
        .ThenInclude(t => t.MaKyGuiNavigation)
        .Include(y => y.NguoiDungNavigation)
        .FirstOrDefaultAsync(y => y.MaYeuCau == maYeuCau);

    if (yc == null || yc.TrangThai != "Chờ xác nhận")
        return Json(new { success = false, message = "Yêu cầu không hợp lệ hoặc đã xử lý!" });

    // 1️⃣ Bé đã có chủ chưa?
    bool daCoChu = await _context.YeuCauNhanNuois
        .AnyAsync(y => y.MaBaiDang == yc.MaBaiDang && y.TrangThai == "Đã xác nhận");

    if (daCoChu)
        return Json(new { success = false, message = "Bé này đã được nhận nuôi bởi người khác!" });

    // 2️⃣ Duyệt yêu cầu này
    yc.TrangThai = "Đã xác nhận";
    yc.BaiDangNavigation.TrangThai = "Đã có chủ";

    // 3️⃣ Tự động từ chối các yêu cầu còn lại
    var danhSachConLai = await _context.YeuCauNhanNuois
        .Where(y => y.MaBaiDang == yc.MaBaiDang
                 && y.MaYeuCau != yc.MaYeuCau
                 && y.TrangThai == "Chờ xác nhận")
        .ToListAsync();

    foreach (var item in danhSachConLai)
        item.TrangThai = "Đã từ chối";

    // 4️⃣ Ghi log
    _context.LichSuHeThongs.Add(new LichSuHeThong
    {
        MaNguoiDung = int.Parse(HttpContext.Session.GetString("UserId") ?? "0"),
        HanhDong = $"Lễ tân xác nhận bé '{yc.BaiDangNavigation.MaThuCungNhanNuoiNavigation.MaKyGuiNavigation.TenThuCung}' cho khách {yc.NguoiDungNavigation.HoTen}",
        NgayThucHien = DateTime.Now
    });

    await _context.SaveChangesAsync();

    return Json(new { success = true, message = "Xác nhận thành công! Bé đã có chủ." });
}
[HttpPost]
public async Task<JsonResult> TuChoiNhanNuoi(int maYeuCau)
{
    var yc = await _context.YeuCauNhanNuois
        .Include(y => y.BaiDangNavigation!)
            .ThenInclude(b => b.MaThuCungNhanNuoiNavigation!)
            .ThenInclude(t => t.MaKyGuiNavigation)
        .Include(y => y.NguoiDungNavigation)
        .FirstOrDefaultAsync(y => y.MaYeuCau == maYeuCau);

    if (yc == null || yc.TrangThai != "Chờ xác nhận")
        return Json(new { success = false, message = "Yêu cầu không hợp lệ hoặc đã xử lý!" });

    bool daCoChu = await _context.YeuCauNhanNuois
        .AnyAsync(y => y.MaBaiDang == yc.MaBaiDang && y.TrangThai == "Đã xác nhận");

    if (daCoChu)
        return Json(new { success = false, message = "Bé này đã được nhận nuôi bởi người khác!" });

    yc.TrangThai = "Đã từ chối";

    _context.LichSuHeThongs.Add(new LichSuHeThong
    {
        MaNguoiDung = int.Parse(HttpContext.Session.GetString("UserId") ?? "0"),
        HanhDong = $"Lễ tân từ chối yêu cầu nhận nuôi bé '{yc.BaiDangNavigation.MaThuCungNhanNuoiNavigation.MaKyGuiNavigation.TenThuCung}' từ khách {yc.NguoiDungNavigation?.HoTen}",
        NgayThucHien = DateTime.Now
    });

    await _context.SaveChangesAsync();

    return Json(new { success = true, message = "Đã từ chối yêu cầu thành công!" });
}
}
}
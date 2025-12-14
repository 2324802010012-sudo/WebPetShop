using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;

namespace WebPetShop.Controllers
{
    public class KyGuiThuCungController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KyGuiThuCungController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===============================
        //  Danh sách tất cả (Admin)
        // ===============================
        public async Task<IActionResult> Index()
        {
            var data = await _context.KyGuiThuCungs
                .Include(k => k.MaKhNavigation)
                .Include(k => k.ChiTietChamSocs)
                .OrderByDescending(k => k.NgayGui)
                .ToListAsync();
            return View(data);
        }

        // ===============================
        //  Danh sách đang ký gửi
        // ===============================
        public async Task<IActionResult> DanhSach()
        {
            var data = await _context.KyGuiThuCungs
                .Include(k => k.MaKhNavigation)
                .Where(k => k.TrangThai == "Đang ký gửi")
                .OrderByDescending(k => k.NgayGui)
                .ToListAsync();
            return View(data);
        }

        // ===============================
        //  Chi tiết – Nhật ký ký gửi
        // ===============================
        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.KyGuiThuCungs
                .Include(k => k.MaKhNavigation)
                .Include(k => k.ChiTietChamSocs)
                .FirstOrDefaultAsync(k => k.MaKyGui == id);

            if (item == null)
                return NotFound();

            // 🔥 Quan trọng: trả về view ChiTiet.cshtml
            return View("/Views/KyGui/ChiTiet.cshtml", item);
        }
        public async Task<IActionResult> ChiTietKhach(int id)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Auth");

            int userId = int.Parse(userIdStr);

            var item = await _context.KyGuiThuCungs
                .Include(k => k.ChiTietChamSocs)
                .Include(k => k.MaKhNavigation)
                .FirstOrDefaultAsync(k => k.MaKyGui == id && k.MaKh == userId);

            if (item == null)
                return NotFound();

            return View("/Views/KyGui/ChiTietKhach.cshtml", item);
        }

        // ===============================
        //  LỊCH SỬ KÝ GỬI CỦA KHÁCH
        // ===============================
        public async Task<IActionResult> LichSuKyGui()
        {
            // Lấy UserId từ Session
            var userIdStr = HttpContext.Session.GetString("UserId");

            // Nếu chưa đăng nhập → quay về Login
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Auth");

            int userId = int.Parse(userIdStr);

            // Lấy danh sách ký gửi của khách
            var data = await _context.KyGuiThuCungs
                .Include(k => k.ChiTietChamSocs)
                .Where(k => k.MaKh == userId)
                .OrderByDescending(k => k.NgayGui)
                .ToListAsync();

            return View("/Views/KyGui/LichSuKyGui.cshtml", data);
        }

        [HttpPost]
        public async Task<IActionResult> GiaHan(int MaKyGui, DateTime NgayHetHanMoi)
        {
            var item = await _context.KyGuiThuCungs
                .FirstOrDefaultAsync(x => x.MaKyGui == MaKyGui);

            if (item == null)
                return NotFound();

            if (item.NgayHetHan.HasValue &&
                NgayHetHanMoi.Date <= item.NgayHetHan.Value.ToDateTime(TimeOnly.MinValue))
            {
                TempData["Error"] = "Ngày nhận lại mới phải sau ngày hiện tại!";
                return RedirectToAction("ChiTietKhach", new { id = MaKyGui });
            }

            // Số ngày gia hạn
            int soNgayThem = (NgayHetHanMoi.Date -
                              item.NgayHetHan.Value.ToDateTime(TimeOnly.MinValue).Date).Days;

            decimal donGia = 500000; // tuỳ bạn
            decimal thanhTien = soNgayThem * donGia;

            // Cập nhật
            item.NgayHetHan = DateOnly.FromDateTime(NgayHetHanMoi);
            item.PhiKyGui = (item.PhiKyGui ?? 0) + thanhTien;
            item.TrangThaiDon = "GiaHanMoi";
            // Lưu hóa đơn
            var hd = new HoaDon
            {
                MaKyGui = MaKyGui,
                MaKeToan = 2,
                NgayLap = DateTime.Now,
                SoTien = thanhTien,
                HinhThuc = "Tiền mặt",
                GhiChu = $"Gia hạn thêm {soNgayThem} ngày"
            };

            _context.HoaDons.Add(hd);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Gia hạn thành công thêm {soNgayThem} ngày!";
            return RedirectToAction("ChiTietKhach", new { id = MaKyGui });
        }


        // ===============================
        //  Form thêm nhật ký
        // ===============================
        [HttpGet]
        public IActionResult ThemNhatKy(int id)
        {
            ViewBag.MaKyGui = id;
            return View();
        }


        // ===============================
        //  Nhận dữ liệu nhật ký
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemNhatKy(int MaKyGui, string GhiChu)
        {
            if (!string.IsNullOrWhiteSpace(GhiChu))
            {
                _context.ChiTietChamSocs.Add(new ChiTietChamSoc
                {
                    MaKyGui = MaKyGui,
                    GhiChu = GhiChu.Trim(),
                    NgayCapNhat = DateTime.Now
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = MaKyGui });
        }

    }
}

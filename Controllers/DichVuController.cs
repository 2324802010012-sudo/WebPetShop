using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;

namespace WebPetShop.Controllers
{
    public class DichVuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DichVuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🏠 Trang giới thiệu dịch vụ
        public IActionResult Index()
        {
            return View();
        }

        // 📋 Trang giới thiệu riêng cho dịch vụ ký gửi
        [HttpGet]
        public IActionResult DatKyGui()
        {
            return View(); // <-- nếu là View() thì đang gọi DatKyGui.cshtml
        }
        [HttpGet]
        public async Task<IActionResult> NhanNuoi()
        {
            var danhSachThuCung = await _context.DanhSachThuCungNhanNuoi
                .OrderByDescending(x => x.NgayDang)
                .ToListAsync();

            return View(danhSachThuCung); // View: Views/DichVu/NhanNuoi.cshtml
        }

        // 🐾 POST: Xử lý form đặt ký gửi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DatKyGui(KyGuiThuCung model)
        {
            // ✅ Lấy userId từ session (dạng string)
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["Error"] = "⚠️ Bạn cần đăng nhập trước khi đặt dịch vụ!";
                return RedirectToAction("Login", "Auth");
            }

            int userId = int.Parse(userIdStr);

            // 🕓 Kiểm tra hợp lệ ngày
            if (model.NgayGui == null || model.NgayHetHan == null)
            {
                TempData["Error"] = "❌ Vui lòng chọn đầy đủ ngày gửi và ngày nhận!";
                return View(model);
            }

            if (model.NgayHetHan < model.NgayGui)
            {
                TempData["Error"] = "❌ Ngày nhận phải sau ngày gửi!";
                return View(model);
            }

            // ✅ Gán thông tin trước khi lưu DB
            model.MaKh = userId;
            model.TrangThai = "Đang ký gửi";
            model.PhiKyGui ??= 0;

            _context.KyGuiThuCungs.Add(model);
            _context.SaveChanges();

            // 🟢 Thông báo thành công và điều hướng đến trang lịch sử ký gửi
            TempData["Success"] = "✅ Đăng ký ký gửi thành công!";
            return RedirectToAction("LichSuKyGui", "DichVu");
        }


        // 🕓 Xem lịch sử ký gửi của khách
        public IActionResult LichSuKyGui()
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var danhSach = _context.KyGuiThuCungs
                .Where(k => k.MaKh == userId)
                .OrderByDescending(k => k.NgayGui)
                .ToList();

            return View(danhSach);
        }

        // 🔍 Xem chi tiết từng đơn ký gửi
        public IActionResult ChiTiet(int id)
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var kyGui = _context.KyGuiThuCungs
                .Include(k => k.MaKhNavigation)
                .Include(k => k.ChiTietChamSocs)
                .FirstOrDefault(k => k.MaKyGui == id && k.MaKh == userId);

            if (kyGui == null)
                return NotFound();

            return View(kyGui);
        }

        // ❌ Hủy đơn ký gửi (chỉ nếu đang xử lý)
        [HttpPost]
        public IActionResult HuyKyGui(int id)
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var kyGui = _context.KyGuiThuCungs
                .FirstOrDefault(k => k.MaKyGui == id && k.MaKh == userId);

            if (kyGui == null)
            {
                TempData["Error"] = "Không tìm thấy dịch vụ ký gửi!";
                return RedirectToAction("LichSuKyGui");
            }

            if (kyGui.TrangThai != "Đang ký gửi")
            {
                TempData["Error"] = "Không thể hủy dịch vụ này!";
                return RedirectToAction("LichSuKyGui");
            }

            kyGui.TrangThai = "Đã hủy";
            _context.SaveChanges();

            TempData["Success"] = "❌ Hủy dịch vụ ký gửi thành công!";
            return RedirectToAction("LichSuKyGui");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;

namespace WebPetShop.Controllers
{
    public class DonHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================
        // 🧩 HÀM LẤY USER ID
        // ============================
        private int? GetUserId()
        {
            string? id = HttpContext.Session.GetString("UserId");
            return string.IsNullOrEmpty(id) ? null : int.Parse(id);
        }

        // ============================
        // 📝 LỊCH SỬ ĐƠN HÀNG
        // ============================
        public IActionResult LichSu()
        {
            int? userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var donHangs = _context.DonHangs
                .Where(d => d.MaNguoiDung == userId)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

            return View(donHangs);
        }

        // ============================
        // 🧾 THANH TOÁN GIỎ HÀNG
        // ============================
        [HttpPost]
        public IActionResult ThanhToan(int MaHTTT)
        {
            int? userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var gioHang = _context.GioHangs.FirstOrDefault(x => x.MaNguoiDung == userId);
            if (gioHang == null)
                return RedirectToAction("Index", "GioHang");

            var items = _context.ChiTietGioHangs
                .Include(x => x.MaSpNavigation)
                .Where(x => x.MaGh == gioHang.MaGh)
                .ToList();

            if (!items.Any())
                return RedirectToAction("Index", "GioHang");

            decimal tongTien = items.Sum(sp => (sp.SoLuong ?? 1) * sp.MaSpNavigation.Gia);

            // ⭐ Tạo đơn hàng
            var donHang = new DonHang
            {
                MaNguoiDung = userId.Value,
                NgayDat = DateTime.Now,
                TrangThai = "Chờ xác nhận",
                TongTien = tongTien,
                MaHTTT = MaHTTT   // ⭐⭐⭐ LƯU HÌNH THỨC THANH TOÁN (TIỀN MẶT / CK)
            };

            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            // ⭐ Thêm chi tiết đơn
            foreach (var sp in items)
            {
                _context.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaDh = donHang.MaDh,
                    MaSp = sp.MaSp,
                    SoLuong = sp.SoLuong ?? 1,
                    DonGia = sp.MaSpNavigation.Gia
                });
            }

            // ⭐ Xóa giỏ hàng
            _context.ChiTietGioHangs.RemoveRange(items);
            _context.SaveChanges();

            TempData["msg"] = "✅ Đặt hàng thành công!";
            return RedirectToAction("LichSu");
        }


        // ============================
        // 🔍 CHI TIẾT ĐƠN HÀNG
        // ============================
        public IActionResult ChiTiet(int id)
        {
            int? userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var donHang = _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(d => d.MaDh == id && d.MaNguoiDung == userId);

            if (donHang == null)
                return NotFound();

            return View(donHang);
        }

        // ============================
        // ❌ HỦY ĐƠN — TRẢ HÀNG VỀ KHO
        // ============================
        public IActionResult HuyDon(int id)
        {
            int? userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var don = _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .FirstOrDefault(x => x.MaDh == id && x.MaNguoiDung == userId);

            // ⭐ Các trạng thái được phép hủy
            var trangThaiChoHuy = new[]
            {
        "Chờ duyệt",
        "Xác nhận",
        "Chuẩn bị hàng",

    };

            // ❌ Nếu không thuộc danh sách cho hủy → báo lỗi
            if (don == null || !trangThaiChoHuy.Contains(don.TrangThai))
            {
                TempData["msg"] = "⚠️ Đơn hàng đã được bàn giao cho đơn vị vận chuyển, không thể hủy!";
                return RedirectToAction("LichSu");
            }

            // ⭐ Hoàn trả kho
            foreach (var ct in don.ChiTietDonHangs)
            {
                var sp = _context.SanPhams.FirstOrDefault(s => s.MaSp == ct.MaSp);
                if (sp != null)
                {
                    sp.SoLuongTon += ct.SoLuong;
                    _context.SanPhams.Update(sp);
                }
            }

            // ⭐ Cập nhật trạng thái
            don.TrangThai = "Đã hủy";
            _context.SaveChanges();

            TempData["msg"] = "❌ Đơn hàng đã được hủy thành công!";
            return RedirectToAction("LichSu");
        }
    }
}

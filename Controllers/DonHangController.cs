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
        public IActionResult LichSu()
        {
            string? userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Auth");
            int maNguoiDung = int.Parse(userIdStr);


            var donHangs = _context.DonHangs
                .Where(d => d.MaNguoiDung == maNguoiDung)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

            return View(donHangs);
        }
        public IActionResult ThanhToan()
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung");
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

            decimal tongTien = 0;
            foreach (var sp in items)
            {
                decimal gia = sp.MaSpNavigation.Gia;
                int soLuong = sp.SoLuong ?? 1;

                tongTien += soLuong * gia;
            }

            var donHang = new DonHang
            {
                MaNguoiDung = userId.Value,
                NgayDat = DateTime.Now,
                TrangThai = "Chờ duyệt",
                TongTien = tongTien
            };

            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            foreach (var sp in items)
            {
                decimal gia = sp.MaSpNavigation.Gia;
                int soLuong = sp.SoLuong ?? 1;

                _context.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaDh = donHang.MaDh,
                    MaSp = sp.MaSp,
                    SoLuong = soLuong,
                    DonGia = gia
                });
            }

            // ✅ Xóa giỏ hàng sau khi thanh toán
            _context.ChiTietGioHangs.RemoveRange(items);
            _context.SaveChanges();

            TempData["msg"] = "✅ Đặt hàng thành công!";
            return RedirectToAction("LichSuDonHang");
        }

        // ✅ Trang xem lịch sử đơn hàng
        public IActionResult LichSuDonHang()
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var donHangs = _context.DonHangs
                .Where(x => x.MaNguoiDung == userId)
                .OrderByDescending(x => x.NgayDat)
                .ToList();

            return View(donHangs);
        }
        // ✅ Chi tiết đơn hàng
        public IActionResult ChiTiet(int id)
        {
            string? userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Auth");

            int maNguoiDung = int.Parse(userIdStr);


            var donHang = _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(d => d.MaDh == id && d.MaNguoiDung == maNguoiDung);

            if (donHang == null)
                return NotFound();

            return View(donHang);
        }

        public IActionResult HuyDon(int id)
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var don = _context.DonHangs.FirstOrDefault(x => x.MaDh == id && x.MaNguoiDung == userId);

            if (don == null || don.TrangThai != "Chờ duyệt")
                return RedirectToAction("LichSuDonHang");

            don.TrangThai = "Đã hủy";
            _context.SaveChanges();

            TempData["msg"] = "❌ Đơn hàng đã được hủy!";
            return RedirectToAction("LichSuDonHang");
        }

    }
}

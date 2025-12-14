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
                                    .ThenInclude(sp => sp.DanhGia)
                .FirstOrDefault(d => d.MaDh == id && d.MaNguoiDung == userId);

            if (donHang == null)
                return NotFound();

            return View(donHang);
        }

        // ============================
        // ❌ HỦY ĐƠN — TRẢ HÀNG VỀ KHO
        // ============================
        [HttpGet]
        public IActionResult HuyDon(int id)
        {
            int? userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var don = _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .FirstOrDefault(x => x.MaDh == id && x.MaNguoiDung == userId);

            var trangThaiChoHuy = new[]
            {
        "Chờ xác nhận",
        "Đã xác nhận",
        "Đang chuẩn bị"
    };

            if (don == null || !trangThaiChoHuy.Contains(don.TrangThai))
            {
                TempData["msg"] = "⚠️ Đơn hàng đã bàn giao cho đơn vị vận chuyển, không thể hủy!";
                return RedirectToAction("LichSu");
            }

            // Trả hàng về kho
            foreach (var ct in don.ChiTietDonHangs)
            {
                var sp = _context.SanPhams.FirstOrDefault(s => s.MaSp == ct.MaSp);
                if (sp != null)
                {
                    sp.SoLuongTon += ct.SoLuong;
                }
            }

            don.TrangThai = "Đã hủy";
            _context.SaveChanges();

            TempData["msg"] = "❌ Đơn hàng đã được hủy thành công!";
            return RedirectToAction("LichSu");
        }

        // HIỂN THỊ FORM ĐÁNH GIÁ
        // GET
        public IActionResult DanhGia(int maSp, int maDh)
        {
            return RedirectToAction("ChiTiet", new { id = maDh });
        }

        [HttpPost]
        public IActionResult LuuDanhGia(int MaSp, int MaDh, int Diem, string NoiDung, IFormFile? AnhDanhGia)
        {
            int? userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            // Kiểm tra đánh giá 1 lần
            bool daDanhGia = _context.DanhGia.Any(x => x.MaNguoiDung == userId && x.MaSp == MaSp);
            if (daDanhGia)
            {
                TempData["Error"] = "⚠️ Bạn đã đánh giá sản phẩm này rồi!";
                return RedirectToAction("ChiTiet", new { id = MaDh });
            }

            string fileName = null;
            if (AnhDanhGia != null)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(AnhDanhGia.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImagesDanhGia", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    AnhDanhGia.CopyTo(stream);
                }
            }

            var dg = new DanhGia
            {
                MaNguoiDung = userId.Value,
                MaSp = MaSp,
                NoiDung = NoiDung,
                Diem = Diem,
                HinhAnh = fileName,
                NgayDanhGia = DateTime.Now
            };

            _context.DanhGia.Add(dg);
            _context.SaveChanges();

            TempData["Success"] = "🎉 Cảm ơn bạn đã đánh giá sản phẩm!";
            return RedirectToAction("ChiTiet", new { id = MaDh });
        }


    }
}

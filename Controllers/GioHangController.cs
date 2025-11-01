using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;

namespace WebPetShop.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GioHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🧾 HIỂN THỊ GIỎ HÀNG
        public IActionResult Index()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "⚠️ Vui lòng đăng nhập để tiếp tục!";
                return RedirectToAction("Login", "Auth");
            }

            int maNguoiDung = int.Parse(userId);

            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
                return View(new List<ChiTietGioHang>());

            return View(gioHang.ChiTietGioHangs);
        }

        // 📝 CẬP NHẬT SỐ LƯỢNG
        [HttpPost]
        public IActionResult Update(int maCtgh, int soLuong)
        {
            var chiTiet = _context.ChiTietGioHangs.Find(maCtgh);
            if (chiTiet != null)
            {
                chiTiet.SoLuong = soLuong;
                _context.Update(chiTiet);
                _context.SaveChanges();
                TempData["Success"] = "✅ Cập nhật số lượng thành công!";
            }
            return RedirectToAction("Index");
        }

        // ➕ THÊM SẢN PHẨM VÀO GIỎ
        [HttpGet]
        public IActionResult Them(int id, int soLuong = 1)
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "⚠️ Vui lòng đăng nhập để thêm sản phẩm!";
                return RedirectToAction("Login", "Auth");
            }

            int maNguoiDung = int.Parse(userId);

            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null)
            {
                gioHang = new GioHang { MaNguoiDung = maNguoiDung, NgayTao = DateTime.Now };
                _context.GioHangs.Add(gioHang);
                _context.SaveChanges();
            }

            var chiTiet = _context.ChiTietGioHangs
                .FirstOrDefault(c => c.MaGh == gioHang.MaGh && c.MaSp == id);

            if (chiTiet == null)
            {
                chiTiet = new ChiTietGioHang
                {
                    MaGh = gioHang.MaGh,
                    MaSp = id,
                    SoLuong = soLuong
                };
                _context.ChiTietGioHangs.Add(chiTiet);
            }
            else
            {
                chiTiet.SoLuong += soLuong;
                _context.ChiTietGioHangs.Update(chiTiet);
            }

            _context.SaveChanges();
            TempData["Success"] = "🛒 Đã thêm sản phẩm vào giỏ hàng!";
            return RedirectToAction("Index");
        }

        // ⚡ MUA NGAY
        [HttpGet]
        public IActionResult MuaNgay(int id, int soLuong = 1)
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "⚠️ Vui lòng đăng nhập để mua hàng!";
                return RedirectToAction("Login", "Auth");
            }

            var sp = _context.SanPhams.Find(id);
            if (sp == null) return NotFound();

            ViewBag.SoLuong = soLuong;
            return View("~/Views/ThanhToan/Index.cshtml", sp);
        }

        // ❌ XÓA SẢN PHẨM
        public IActionResult Remove(int id)
        {
            var chiTiet = _context.ChiTietGioHangs.Find(id);
            if (chiTiet != null)
            {
                _context.ChiTietGioHangs.Remove(chiTiet);
                _context.SaveChanges();
                TempData["Success"] = "🗑️ Đã xóa sản phẩm khỏi giỏ hàng!";
            }
            return RedirectToAction("Index");
        }

        // 💳 THANH TOÁN
        public IActionResult Checkout()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            int maNguoiDung = int.Parse(userId);

            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
            {
                TempData["Error"] = "⚠️ Giỏ hàng trống!";
                return RedirectToAction("Index");
            }

            return View("~/Views/ThanhToan/Index.cshtml", gioHang);
        }
        public IActionResult ThanhToanThanhCong(int maDh)
        {
            var donHang = _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(d => d.MaDh == maDh);

            if (donHang == null)
                return RedirectToAction("Index", "SanPham");

            return View(donHang);
        }

        // ✅ XÁC NHẬN ĐẶT HÀNG
        [HttpPost]
        public IActionResult XacNhan(string HoTen, string SoDienThoai, string DiaChi, string? GhiChu)
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            int maNguoiDung = int.Parse(userId);

            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
            {
                TempData["Error"] = "⚠️ Giỏ hàng trống!";
                return RedirectToAction("Index");
            }

            var donHang = new DonHang
            {
                MaNguoiDung = maNguoiDung,
                NgayDat = DateTime.Now,
                TrangThai = "Chờ xác nhận",
                HoTenNguoiNhan = HoTen,
                SoDienThoai = SoDienThoai,
                DiaChiGiaoHang = DiaChi,
                GhiChu = GhiChu
            };
            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            foreach (var ct in gioHang.ChiTietGioHangs)
            {
                var chiTiet = new ChiTietDonHang
                {
                    MaDh = donHang.MaDh,
                    MaSp = ct.MaSp,
                    SoLuong = ct.SoLuong ?? 1,
                    DonGia = ct.MaSpNavigation.Gia
                };
                _context.ChiTietDonHangs.Add(chiTiet);
            }

            _context.ChiTietGioHangs.RemoveRange(gioHang.ChiTietGioHangs);
            _context.SaveChanges();

            TempData["Success"] = "🎉 Đặt hàng thành công! Cửa hàng sẽ liên hệ xác nhận sớm.";
            return RedirectToAction("Index", "SanPham");
        }
    }
}

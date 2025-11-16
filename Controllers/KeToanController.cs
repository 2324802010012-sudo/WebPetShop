using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;

namespace WebPetShop.Controllers
{
    public class KeToanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KeToanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // 📌 DASHBOARD KẾ TOÁN
        // =====================================================
        public IActionResult Index()
        {
            var now = DateTime.Now;
            int thang = now.Month;
            int nam = now.Year;

            ViewBag.TongHoaDon = _context.HoaDons.Count();

            ViewBag.DoanhThuThang = _context.HoaDons
                .Where(h => h.NgayLap.HasValue &&
                            h.NgayLap.Value.Month == thang &&
                            h.NgayLap.Value.Year == nam)
                .Sum(h => (decimal?)h.SoTien ?? 0);

            ViewBag.DonCODChuaThu = _context.DonHangs
                .Where(d => d.PhuongThucThanhToan == "COD" &&
                            d.TrangThai == "Đã giao")
                .Count();

            ViewBag.TongChiPhiNhap = _context.PhieuNhaps
                .Where(p => p.NgayNhap.HasValue &&
                            p.NgayNhap.Value.Month == thang &&
                            p.NgayNhap.Value.Year == nam)
                .Sum(p => (decimal?)p.TongTien ?? 0);

            return View();
        }

        // =====================================================
        // 📄 DANH SÁCH HÓA ĐƠN
        // =====================================================
        public IActionResult HoaDon()
        {
            var ds = _context.HoaDons
                .Include(h => h.MaDhNavigation)
                .Include(h => h.MaKyGuiNavigation)
                .Include(h => h.MaKeToanNavigation)
                .OrderByDescending(h => h.NgayLap)
                .ToList();

            return View(ds);
        }

        // =====================================================
        // 📄 CHI TIẾT HÓA ĐƠN
        // =====================================================
        public IActionResult ChiTietHoaDon(int id)
        {
            var hd = _context.HoaDons
                .Include(h => h.MaDhNavigation)
                    .ThenInclude(d => d.ChiTietDonHangs)
                        .ThenInclude(ct => ct.MaSpNavigation)
                .Include(h => h.MaKyGuiNavigation)
                .Include(h => h.MaKeToanNavigation)
                .FirstOrDefault(h => h.MaHd == id);

            if (hd == null) return NotFound();

            return View(hd);
        }

        // =====================================================
        // 🐾 LẬP HÓA ĐƠN KÝ GỬI
        // =====================================================
        [HttpGet]
        public IActionResult LapHoaDonKyGui(int maKyGui)
        {
            var kg = _context.KyGuiThuCungs
                .Include(k => k.MaKhNavigation)
                .FirstOrDefault(k => k.MaKyGui == maKyGui);

            if (kg == null) return NotFound();

            return View(kg);
        }

        [HttpPost]
        public IActionResult LapHoaDonKyGui(int MaKyGui, decimal SoTien, string HinhThuc, string? GhiChu)
        {
            var hd = new HoaDon
            {
                MaKyGui = MaKyGui,
                MaKeToan = HttpContext.Session.GetInt32("UserId") ?? 2,
                SoTien = SoTien,
                HinhThuc = HinhThuc,
                GhiChu = GhiChu,
                NgayLap = DateTime.Now
            };

            _context.HoaDons.Add(hd);
            _context.SaveChanges();

            TempData["Success"] = "Đã lập hóa đơn ký gửi thành công!";
            return RedirectToAction("HoaDon");
        }

        // =====================================================
        // 💰 BÁO CÁO DOANH THU – CHI PHÍ – LỢI NHUẬN
        // =====================================================
        public IActionResult BaoCaoTaiChinh()
        {
            var now = DateTime.Now;
            int thang = now.Month;
            int nam = now.Year;

            // ---------------------------
            // TỔNG HỢP THÁNG HIỆN TẠI
            // ---------------------------
            var doanhThu = _context.HoaDons
                .Where(h => h.NgayLap.HasValue &&
                            h.NgayLap.Value.Month == thang &&
                            h.NgayLap.Value.Year == nam)
                .Sum(h => (decimal?)h.SoTien ?? 0);

            var chiPhiNhap = _context.PhieuNhaps
                .Where(p => p.NgayNhap.HasValue &&
                            p.NgayNhap.Value.Month == thang &&
                            p.NgayNhap.Value.Year == nam)
                .Sum(p => (decimal?)p.TongTien ?? 0);

            var loiNhuan = doanhThu - chiPhiNhap;

            ViewBag.DoanhThu = doanhThu;
            ViewBag.ChiPhi = chiPhiNhap;
            ViewBag.LoiNhuan = loiNhuan;

            // ---------------------------
            // BIỂU ĐỒ 12 THÁNG
            // ---------------------------
            var thongKe = new List<dynamic>();

            for (int i = 1; i <= 12; i++)
            {
                var dt = _context.HoaDons
                    .Where(h => h.NgayLap.HasValue &&
                                h.NgayLap.Value.Month == i &&
                                h.NgayLap.Value.Year == nam)
                    .Sum(h => (decimal?)h.SoTien ?? 0);

                var cp = _context.PhieuNhaps
                    .Where(p => p.NgayNhap.HasValue &&
                                p.NgayNhap.Value.Month == i &&
                                p.NgayNhap.Value.Year == nam)
                    .Sum(p => (decimal?)p.TongTien ?? 0);

                thongKe.Add(new
                {
                    Thang = i,
                    DoanhThu = dt,
                    ChiPhi = cp,
                    LoiNhuan = dt - cp
                });
            }

            ViewBag.ThongKe = thongKe;

            return View();
        }
    }
}

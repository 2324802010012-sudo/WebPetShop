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
        public IActionResult XacNhanDon(int id)
        {
            var don = _context.DonHangs.FirstOrDefault(d => d.MaDh == id);
            if (don == null) return NotFound();

            // ⭐ Cập nhật trạng thái đúng
            don.TrangThai = "Đã xác nhận";
            _context.SaveChanges();

            // ⭐ Ghi log
            _context.LichSuHeThongs.Add(new LichSuHeThong
            {
                MaNguoiDung = int.Parse(HttpContext.Session.GetString("UserId")),
                HanhDong = $"Lễ tân xác nhận đơn #{id}, chuyển sang kho chuẩn bị hàng",
                NgayThucHien = DateTime.Now
            });

            _context.SaveChanges();

            TempData["Success"] = $"✅ Đã xác nhận đơn #{id} và gửi sang kho!";
            return RedirectToAction("DonHang");
        }

        // =================== KÝ GỬI ===================
        public IActionResult KyGui()
        {
            ViewBag.KyGuiMoi = _context.KyGuiThuCungs.Count(k => k.TrangThai == "Chờ xác nhận");
            return View(_context.KyGuiThuCungs.Include(k => k.MaKhNavigation).ToList());
        }

        // =================== NHẬN NUÔI ===================
        public IActionResult BaiNhanNuoi()
        {
            ViewBag.BaiNhanNuoiMoi = _context.BaiDangNhanNuois.Count(b => b.TrangThai == "Chờ duyệt");
            return View(_context.BaiDangNhanNuois.Include(b => b.MaNguoiTaoNavigation).ToList());
        }

        // =================== HỖ TRỢ KHÁCH HÀNG ===================
        public IActionResult HoTroKhachHang()
        {
            ViewBag.HoTroMoi = _context.LichSuHeThongs.Count(h => h.HanhDong.Contains("Yêu cầu hỗ trợ"));
            return View(_context.LichSuHeThongs.Include(h => h.MaNguoiDungNavigation).ToList());
        }
    }
}

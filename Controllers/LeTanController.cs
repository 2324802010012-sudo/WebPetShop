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
            var donHang = _context.DonHangs
                .Include(d => d.MaNguoiDungNavigation)
                .FirstOrDefault(d => d.MaDh == id);

            if (donHang == null)
                return NotFound();

            // 🎯 Cập nhật trạng thái
            donHang.TrangThai = "Đã xác nhận"; // hiển thị bên khách hàng
            _context.SaveChanges();

            // 🎯 Gửi thông báo cho kho (tạo 1 bản ghi trong LichSuHeThong hoặc 1 table Notification nếu có)
            var log = new LichSuHeThong
            {
                MaNguoiDung = Convert.ToInt32(HttpContext.Session.GetString("UserId")),
                HanhDong = $"Lễ tân xác nhận đơn #{donHang.MaDh}, chuyển sang kho chuẩn bị hàng",
                NgayThucHien = DateTime.Now
            };
            _context.LichSuHeThongs.Add(log);

            // 🎯 Cập nhật thêm trạng thái bên kho (tùy cách bạn thể hiện)
            // Ví dụ: lưu trạng thái hiển thị riêng cho kho
            var phieuXuat = new PhieuXuat
            {
                MaDh = donHang.MaDh,
                NgayXuat = DateTime.Now,
                TongTien = donHang.TongTien ?? 0,
                MaNhanVien = null,
                MaKhachHang = donHang.MaNguoiDung,
            };
            _context.PhieuXuats.Add(phieuXuat);

            _context.SaveChanges();

            TempData["Success"] = $"✅ Đã xác nhận đơn #{donHang.MaDh} và gửi sang kho chuẩn bị hàng.";
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

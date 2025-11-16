using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;

namespace WebPetShop.Controllers
{
    public class ChamSocController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ChamSocController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // =============== DANH SÁCH THÚ KÝ GỬI ===============
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "ChamSoc")
                return RedirectToAction("Login", "Auth");

            var list = _context.KyGuiThuCungs
                .Include(k => k.MaKhNavigation)
                .Where(k => k.TrangThai.Contains("ký gửi"))
                .OrderByDescending(k => k.NgayGui)
                .ToList();

            return View(list);
        }

        // =============== CHI TIẾT MỘT THÚ KÝ GỬI ===============
        public IActionResult ChiTiet(int id)
        {
            var kg = _context.KyGuiThuCungs
                .Include(k => k.MaKhNavigation)
                .FirstOrDefault(k => k.MaKyGui == id);

            if (kg == null) return NotFound();

            var lichSu = _context.ChiTietChamSocs
                .Where(c => c.MaKyGui == id)
                .OrderByDescending(c => c.NgayCapNhat)
                .ToList();

            ViewBag.LichSu = lichSu;
            return View(kg);
        }

        // =============== THÊM LỊCH SỬ CHĂM SÓC ===============
        [HttpPost]
        public IActionResult ThemChamSoc(int maKyGui, string tinhTrang, string ghiChu, IFormFile? hinhAnh)
        {
            string? fileName = null;

            // Lưu hình
            if (hinhAnh != null)
            {
                string uploadPath = Path.Combine(_env.WebRootPath, "ImagesChamSoc");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                fileName = Guid.NewGuid().ToString() + "_" + hinhAnh.FileName;
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    hinhAnh.CopyTo(stream);
                }
            }

            var cs = new ChiTietChamSoc
            {
                MaKyGui = maKyGui,
                TinhTrang = tinhTrang,
                GhiChu = ghiChu,
                HinhAnh = fileName,
                NgayCapNhat = DateTime.Now
            };

            _context.ChiTietChamSocs.Add(cs);
            _context.SaveChanges();

            TempData["Success"] = "Đã cập nhật chăm sóc cho thú cưng!";
            return RedirectToAction("ChiTiet", new { id = maKyGui });
        }
    }
}

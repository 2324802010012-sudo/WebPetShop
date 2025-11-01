using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;

namespace WebPetShop.Controllers
{
    public class KyGuiController : Controller
    {
        private readonly ApplicationDbContext _context;
        public KyGuiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult DanhSach()
        {
            var data = _context.KyGuiThuCungs
                .Include(x => x.MaKhNavigation)
                .Where(x => x.TrangThai == "Đang ký gửi")
                .OrderByDescending(x => x.MaKyGui)
                .ToList();

            return View(data);
        }
        public IActionResult TheoDoi(int id)
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var thu = _context.KyGuiThuCungs
                    .Include(x => x.ChiTietChamSocs)
                    .FirstOrDefault(x => x.MaKyGui == id && x.MaKh == userId);

            if (thu == null) return NotFound();

            return View(thu);
        }
        [HttpGet]
        public IActionResult NhatKy(int id)
        {
            var model = new ChiTietChamSoc { MaKyGui = id };
            return View(model);
        }

        [HttpPost]
        public IActionResult NhatKy(ChiTietChamSoc log)
        {
            log.NgayCapNhat = DateTime.Now;
            _context.ChiTietChamSocs.Add(log);
            _context.SaveChanges();

            TempData["msg"] = "Đã cập nhật!";
            return RedirectToAction("NhatKy", new { id = log.MaKyGui });
        }
        public IActionResult TraThu(int id)
        {
            var thu = _context.KyGuiThuCungs.Find(id);
            if (thu == null) return NotFound();

            thu.TrangThai = "Đã trả";
            _context.SaveChanges();

            // Tạo hóa đơn
            var hd = new HoaDon
            {
                MaKyGui = id,
                MaKeToan = 1, // tạm cứng
                SoTien = 0,  // sẽ tính sau
                NgayLap = DateTime.Now
            };
            _context.HoaDons.Add(hd);
            _context.SaveChanges();

            TempData["msg"] = "✅ Đã trả thú và tạo hóa đơn";
            return RedirectToAction("DanhSach");
        }

    }
}

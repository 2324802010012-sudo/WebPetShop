using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;

namespace WebPetShop.Controllers
{
    public class AdminDonHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminDonHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ds = _context.DonHangs
                .Include(x => x.MaNguoiDungNavigation)
                .OrderByDescending(x => x.MaDh)
                .ToList();
            return View(ds);
        }

        public IActionResult Duyet(int id)
        {
            var don = _context.DonHangs
                .Include(x => x.ChiTietDonHangs)
                .ThenInclude(x => x.MaSpNavigation)
                .FirstOrDefault(x => x.MaDh == id);

            if (don == null) return NotFound();

            don.TrangThai = "Đã duyệt";

            // ✅ Trừ tồn kho
            foreach (var ct in don.ChiTietDonHangs)
            {
                ct.MaSpNavigation.SoLuongTon -= ct.SoLuong;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Huy(int id)
        {
            var don = _context.DonHangs.Find(id);
            if (don == null) return NotFound();

            don.TrangThai = "Hủy đơn";
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

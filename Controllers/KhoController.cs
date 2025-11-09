using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;

namespace WebPetShop.Controllers
{
    public class KhoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public KhoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🏠 Trang tổng quan
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Kho")
            {
                return RedirectToAction("AccessDenied", "Auth");
            }
            var spSapHet = _context.SanPhams
                .Where(sp => sp.SoLuongTon <= 5)
                .OrderBy(sp => sp.SoLuongTon)
                .ToList();
            ViewBag.TongSP = _context.SanPhams.Count();
            ViewBag.SapHet = spSapHet.Count;
            return View(spSapHet);
        }

        // 📦 Danh sách hàng hóa
        public IActionResult HangHoa()
        {
            var list = _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .OrderBy(s => s.TenSp)
                .ToList();
            return View(list);
        }

        // 📥 Phiếu nhập
        public IActionResult NhapKho() => View();

        // 📤 Phiếu xuất
        public IActionResult XuatKho() => View();

        // 📈 Báo cáo
        public IActionResult BaoCao() => View();
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;

namespace WebPetShop.Controllers
{
    public class UserDonHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserDonHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var ds = _context.DonHangs
                .Where(x => x.MaNguoiDung == userId)
                .OrderByDescending(x => x.MaDh)
                .ToList();

            return View(ds);
        }
    }
}

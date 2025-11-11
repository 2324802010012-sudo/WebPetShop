using Microsoft.AspNetCore.Mvc;
using WebPetShop.Data;
using Microsoft.EntityFrameworkCore;
public class GiaoHangController : Controller
{
    private readonly ApplicationDbContext _context;

    public GiaoHangController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        // ✅ Kiểm tra vai trò
        var role = HttpContext.Session.GetString("Role");
        if (role != "GiaoHang" && role != "Admin")
        {
            return RedirectToAction("Login", "Auth");
        }

        ViewData["Title"] = "Trang Nhân Viên Giao Hàng";
        return View();
    }
    public IActionResult DanhSachGiaoHang()
    {
        var ds = _context.DonHangs
            .Where(d => d.TrangThai == "Đang giao" || d.TrangThai == "Chờ giao")
            .Include(d => d.MaNguoiDungNavigation)
            .ToList();

        return View(ds);
    }

    public IActionResult CapNhatTrangThai(int id)
    {
        var don = _context.DonHangs.Find(id);
        if (don == null) return NotFound();

        return View(don);
    }

    [HttpPost]
    public IActionResult CapNhatTrangThai(int id, string trangThai)
    {
        var don = _context.DonHangs.Find(id);
        if (don != null)
        {
            don.TrangThai = trangThai;
            _context.SaveChanges();
        }
        return RedirectToAction("DanhSachGiaoHang");
    }
}

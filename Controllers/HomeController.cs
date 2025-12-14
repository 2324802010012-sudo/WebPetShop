using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // Lấy 6 sản phẩm mới nhất (hoặc nổi bật)
        var sanPhamNoiBat = _context.SanPhams
            .OrderByDescending(x => x.MaSp)
            .Take(6)
            .ToList();

        // 🔹 Lấy các sản phẩm có khuyến mãi đang active
        var today = DateOnly.FromDateTime(DateTime.Now);

        var khuyenMaiHienTai = _context.KhuyenMais
            .Include(k => k.SanPham)
            .Where(k => k.TrangThai == true &&
                        k.NgayBatDau <= today &&
                        k.NgayKetThuc >= today)
            .ToList();


        ViewBag.KhuyenMai = khuyenMaiHienTai;

        return View(sanPhamNoiBat);
    }
}

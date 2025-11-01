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
        var sanPhamNoiBat = _context.SanPhams
            .OrderByDescending(x => x.MaSp)
            .Take(6)
            .ToList();

        return View(sanPhamNoiBat);
    }

}

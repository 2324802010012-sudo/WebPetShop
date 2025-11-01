using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;

namespace WebPetShop.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SanPhamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Hiển thị sản phẩm + lọc danh mục + tìm kiếm
        public IActionResult Index(int? madm, string? searchString)
        {
            var query = _context.SanPhams
                .Include(x => x.MaDanhMucNavigation)
                .AsQueryable();

            // ✅ Lọc theo danh mục
            if (madm.HasValue)
            {
                query = query.Where(s => s.MaDanhMuc == madm);
            }

            // ✅ Lọc theo từ khóa tìm kiếm
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(s => s.TenSp.Contains(searchString));
            }

            // ✅ Truyền danh mục cho Sidebar Filter
            ViewBag.DanhMuc = _context.DanhMucs.ToList();

            return View(query.ToList());
        }

        // ✅ Trang chi tiết sản phẩm
        public IActionResult Detail(int id)
        {
            var sp = _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .FirstOrDefault(s => s.MaSp == id);

            if (sp == null) return NotFound();

            return View(sp);
        }
        public IActionResult DanhMuc(int id)
        {
            var sanPhams = _context.SanPhams
                .Where(sp => sp.MaDanhMuc == id)
                .ToList();

            var danhMuc = _context.DanhMucs
                .FirstOrDefault(dm => dm.MaDanhMuc == id);

            ViewBag.TenDanhMuc = danhMuc?.TenDanhMuc ?? "Sản phẩm";

            return View("Index", sanPhams); // tái sử dụng view danh sách sản phẩm
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;
using Microsoft.Data.SqlClient;

namespace WebPetShop.Controllers
{
    public class KhoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public KhoController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ==========================================================
        // 🏠 TRANG CHÍNH
        // ==========================================================
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Kho")
                return RedirectToAction("AccessDenied", "Auth");

            var spSapHet = _context.SanPhams
                .Where(sp => sp.SoLuongTon <= 5)
                .OrderBy(sp => sp.SoLuongTon)
                .ToList();

            ViewBag.TongSP = _context.SanPhams.Count();
            ViewBag.SapHet = spSapHet.Count;

            return View(spSapHet);
        }

        // ==========================================================
        // 📦 DANH SÁCH HÀNG HÓA
        // ==========================================================
        public IActionResult HangHoa()
        {
            var list = _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .OrderBy(s => s.TenSp)
                .ToList();

            ViewBag.DanhMucs = _context.DanhMucs.OrderBy(d => d.TenDanhMuc).ToList();
            return View(list);
        }

        // ==========================================================
        public IActionResult NhapKho() => RedirectToAction("PhieuNhap");
        public IActionResult BaoCao() => View();

        // ==========================================================
        // ✏️ CHỈNH SỬA SẢN PHẨM
        // ==========================================================
        [HttpGet]
        public IActionResult ChinhSua(int id)
        {
            var sp = _context.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .FirstOrDefault(s => s.MaSp == id);

            if (sp == null) return NotFound();

            ViewBag.DanhMucs = _context.DanhMucs.ToList();
            return View(sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChinhSua(int MaSp, string TenSp, decimal Gia, int MaDanhMuc, int SoLuongTon)
        {
            var existing = _context.SanPhams.FirstOrDefault(x => x.MaSp == MaSp);
            if (existing == null)
            {
                TempData["Error"] = "❌ Không tìm thấy sản phẩm.";
                return RedirectToAction("HangHoa");
            }

            existing.TenSp = TenSp;
            existing.Gia = Gia;
            existing.MaDanhMuc = MaDanhMuc;
            existing.SoLuongTon = SoLuongTon;

            _context.SaveChanges();

            TempData["Success"] = "✅ Cập nhật sản phẩm thành công!";
            return RedirectToAction("HangHoa");
        }

        // ==========================================================
        // ➕ THÊM SẢN PHẨM MỚI
        // ==========================================================
        [HttpGet]
        public IActionResult ThemSanPham()
        {
            ViewBag.DanhMucs = _context.DanhMucs.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPham(SanPham sp, IFormFile? HinhAnhFile)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sp.TenSp) || sp.Gia <= 0 || sp.MaDanhMuc == 0)
                {
                    ViewBag.DanhMucs = _context.DanhMucs.ToList();
                    TempData["Error"] = "❌ Dữ liệu nhập không hợp lệ!";
                    return View(sp);
                }

                string fileName = "no-image.png";
                if (HinhAnhFile != null && HinhAnhFile.Length > 0)
                {
                    fileName = Guid.NewGuid() + Path.GetExtension(HinhAnhFile.FileName);
                    string folder = Path.Combine(_env.WebRootPath, "ImagesWWeb");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    string filePath = Path.Combine(folder, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    HinhAnhFile.CopyTo(stream);
                }

                _context.Database.ExecuteSqlRaw(
                    "EXEC sp_ThemSanPham @TenSP, @Gia, @MoTa, @HinhAnh, @MaDanhMuc, @SoLuongTon",
                    new SqlParameter("@TenSP", sp.TenSp ?? (object)DBNull.Value),
                    new SqlParameter("@Gia", sp.Gia),
                    new SqlParameter("@MoTa", (object?)sp.MoTa ?? DBNull.Value),
                    new SqlParameter("@HinhAnh", fileName),
                    new SqlParameter("@MaDanhMuc", sp.MaDanhMuc),
                    new SqlParameter("@SoLuongTon", sp.SoLuongTon)
                );

                TempData["Success"] = "✅ Thêm sản phẩm mới thành công!";
                return RedirectToAction("HangHoa");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "❌ Lỗi khi thêm sản phẩm: " + ex.Message;
                ViewBag.DanhMucs = _context.DanhMucs.ToList();
                return View(sp);
            }
        }

        // ==========================================================
        // 🗑️ XÓA SẢN PHẨM
        // ==========================================================
        public IActionResult Xoa(int id)
        {
            var sp = _context.SanPhams.Find(id);
            if (sp != null)
            {
                _context.SanPhams.Remove(sp);
                _context.SaveChanges();
                TempData["Success"] = "🗑️ Đã xóa sản phẩm khỏi danh sách.";
            }
            else
            {
                TempData["Error"] = "❌ Không tìm thấy sản phẩm cần xóa.";
            }
            return RedirectToAction("HangHoa");
        }

        // ==========================================================
        // 📦 PHIẾU NHẬP
        // ==========================================================
        [HttpGet]
        public async Task<IActionResult> PhieuNhap()
        {
            var danhSach = await _context.PhieuNhaps
                .Include(p => p.MaNccNavigation)
                .Include(p => p.MaNhanVienNavigation)
                .OrderByDescending(p => p.NgayNhap)
                .ToListAsync();

            return View(danhSach);
        }

        [HttpGet]
        public IActionResult ThemPhieuNhap()
        {
            ViewBag.NhaCungCap = _context.NhaCungCaps.OrderBy(n => n.TenNcc).ToList();
            ViewBag.SanPham = _context.SanPhams.OrderBy(s => s.TenSp).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemPhieuNhap(PhieuNhap model, List<ChiTietPhieuNhap> chiTiet)
        {
            try
            {
                if (chiTiet == null || !chiTiet.Any())
                {
                    TempData["Error"] = "❌ Vui lòng thêm ít nhất 1 sản phẩm vào phiếu nhập.";
                    return RedirectToAction(nameof(ThemPhieuNhap));
                }

                model.NgayNhap = DateTime.Now;
                model.MaNhanVien = HttpContext.Session.GetInt32("UserId") ?? 1;
                model.TongTien = chiTiet.Sum(c => (c.DonGia ?? 0) * (c.SoLuong ?? 0));

                _context.PhieuNhaps.Add(model);
                await _context.SaveChangesAsync();

                foreach (var c in chiTiet)
                {
                    c.MaPn = model.MaPn;
                    _context.ChiTietPhieuNhaps.Add(c);

                    var sp = await _context.SanPhams.FindAsync(c.MaSp);
                    if (sp != null)
                    {
                        sp.SoLuongTon += c.SoLuong ?? 0;
                        _context.SanPhams.Update(sp);
                    }
                }

                await _context.SaveChangesAsync();

                TempData["Success"] = "✅ Đã thêm phiếu nhập thành công!";
                return RedirectToAction(nameof(PhieuNhap));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "❌ Lỗi khi thêm phiếu nhập: " + ex.Message;
                return RedirectToAction(nameof(ThemPhieuNhap));
            }
        }

        // ==========================================================
        // 📤 PHIẾU XUẤT KHO – phiên bản tối ưu, tránh lỗi trigger
        // ==========================================================
        public IActionResult XuatKho()
        {
            var ds = _context.PhieuXuats
                .Include(x => x.MaNhanVienNavigation)
                .Include(x => x.MaKhachHangNavigation)
                .OrderByDescending(x => x.NgayXuat)
                .ToList();

            return View(ds);
        }

        [HttpGet]
        public IActionResult ThemPhieuXuat()
        {
            ViewBag.SanPham = _context.SanPhams.OrderBy(s => s.TenSp).ToList();
            ViewBag.KhachHang = _context.NguoiDungs
                .Where(u => u.VaiTro == "KhachHang")
                .OrderBy(u => u.HoTen)
                .ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemPhieuXuat(int MaKhachHang, int? MaDH, List<ChiTietPhieuXuat> chiTiet)
        {
            try
            {
                // 🔹 Kiểm tra dữ liệu đầu vào
                if (chiTiet == null || !chiTiet.Any())
                {
                    TempData["Error"] = "❌ Vui lòng thêm ít nhất 1 sản phẩm vào phiếu xuất.";
                    return RedirectToAction(nameof(ThemPhieuXuat));
                }

                var nhanVien = _context.NguoiDungs.FirstOrDefault(u => u.VaiTro == "Kho");
                decimal tongTien = chiTiet.Sum(x => (decimal)x.SoLuong * x.DonGia);

                // 🧾 Tạo phiếu xuất
                var px = new PhieuXuat
                {
                    MaNhanVien = nhanVien?.MaNguoiDung,
                    MaKhachHang = MaKhachHang,
                    MaDh = MaDH,
                    NgayXuat = DateTime.Now,
                    TongTien = tongTien
                };

                _context.PhieuXuats.Add(px);
                _context.SaveChanges(); // 🔸 Lưu trước để có MaPX

                // 💥 Lưu chi tiết phiếu xuất
                foreach (var c in chiTiet)
                {
                    c.MaPx = px.MaPx;
                    _context.Database.ExecuteSqlRaw(
                        @"INSERT INTO ChiTietPhieuXuat (MaPX, MaSP, SoLuong, DonGia)
                  VALUES (@MaPX, @MaSP, @SoLuong, @DonGia)",
                        new SqlParameter("@MaPX", px.MaPx),
                        new SqlParameter("@MaSP", c.MaSp),
                        new SqlParameter("@SoLuong", c.SoLuong),
                        new SqlParameter("@DonGia", c.DonGia)
                    );

                    // 🔹 Cập nhật tồn kho trực tiếp (dự phòng nếu trigger bị tắt)
                    var sp = _context.SanPhams.Find(c.MaSp);
                    if (sp != null)
                    {
                        // Trừ số lượng tồn khi xuất hàng
                        sp.SoLuongTon = Math.Max((sp.SoLuongTon ?? 0) - c.SoLuong, 0);
                        _context.SanPhams.Update(sp);
                    }
                }

                    _context.SaveChanges();

                // 🚚 Nếu phiếu xuất gắn với đơn hàng → tạo phiếu giao hàng
                if (MaDH != null)
                {
                    var giaoHang = new GiaoHang
                    {
                        MaDh = (int)MaDH,
                        MaNhanVienGiao = nhanVien?.MaNguoiDung,
                        DonViGiao = "Nội bộ PetShop",
                        NgayGiao = DateTime.Now,
                        TrangThai = "Đang giao"
                    };

                    _context.GiaoHangs.Add(giaoHang);

                    var donHang = _context.DonHangs.Find(MaDH);
                    if (donHang != null)
                    {
                        donHang.TrangThai = "Đang giao hàng";
                        _context.DonHangs.Update(donHang);
                    }

                    _context.SaveChanges();
                }

                TempData["Success"] = "✅ Đã tạo phiếu xuất và cập nhật tồn kho thành công!";
                return RedirectToAction(nameof(XuatKho));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "❌ Lỗi khi tạo phiếu xuất: " + ex.Message;
                return RedirectToAction(nameof(ThemPhieuXuat));
            }
        }

        public IActionResult ChiTietPhieuXuat(int id)
        {
            var px = _context.PhieuXuats
                .Include(p => p.MaNhanVienNavigation)
                .Include(p => p.MaKhachHangNavigation)
                .Include(p => p.ChiTietPhieuXuats)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(p => p.MaPx == id);

            if (px == null) return NotFound();
            return View(px);
        }

    }
}

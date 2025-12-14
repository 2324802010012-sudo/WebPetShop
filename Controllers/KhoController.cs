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
        public IActionResult ChinhSua(SanPham model)
        {
            ModelState.Remove("KhuyenMais");
            ModelState.Remove("MaDanhMucNavigation");

            if (!ModelState.IsValid)
            {
                // Xem lỗi bind
                var errors = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                TempData["Error"] = "Lỗi nhập liệu: " + errors;

                ViewBag.DanhMucs = _context.DanhMucs.ToList();
                return View(model);
            }

            var sp = _context.SanPhams.FirstOrDefault(s => s.MaSp == model.MaSp);
            if (sp == null)
            {
                TempData["Error"] = "❌ Không tìm thấy sản phẩm!";
                return RedirectToAction("HangHoa");
            }

            sp.TenSp = model.TenSp;
            sp.Gia = model.Gia;
            sp.MaDanhMuc = model.MaDanhMuc;
            sp.SoLuongTon = model.SoLuongTon;

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
        public IActionResult ChiTietPhieuNhap(int id)
        {
            var pn = _context.PhieuNhaps
                .Include(p => p.MaNccNavigation)
                .Include(p => p.MaNhanVienNavigation)
                .Include(p => p.ChiTietPhieuNhaps)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(p => p.MaPn == id);

            if (pn == null)
                return NotFound();

            return View(pn);
        }
        public IActionResult InPhieuNhap(int id)
        {
            var pn = _context.PhieuNhaps
                .Include(p => p.MaNccNavigation)
                .Include(p => p.MaNhanVienNavigation)
                .Include(p => p.ChiTietPhieuNhaps)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(p => p.MaPn == id);

            if (pn == null)
                return NotFound();

            return View(pn);
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

        public IActionResult DonChoXuLy()
        {
            var don = _context.DonHangs
                .Where(d => d.TrangThai == "Đã xác nhận" || d.TrangThai == "Đang chuẩn bị")
                .Include(d => d.MaNguoiDungNavigation)
                .ToList();

            return View(don);
        }
        public IActionResult ChuanBiHang(int id)
        {
            var don = _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .FirstOrDefault(d => d.MaDh == id);

            if (don == null) return NotFound();

            // ✔ Cập nhật trạng thái
            _context.Database.ExecuteSqlRaw(
                "UPDATE DonHang SET TrangThai = N'Đang chuẩn bị' WHERE MaDH = {0}", id);

            // ✔ Nếu chưa có phiếu xuất → tạo bằng SQL Raw để tránh trigger OUTPUT
            if (!_context.PhieuXuats.Any(px => px.MaDh == id))
            {
                int userId = HttpContext.Session.GetInt32("UserId") ?? 1;

                _context.Database.ExecuteSqlRaw(
                    @"INSERT INTO PhieuXuat (MaDH, MaKhachHang, NgayXuat, MaNhanVien, TongTien)
              VALUES ({0}, {1}, GETDATE(), {2}, {3})",
                    don.MaDh, don.MaNguoiDung, userId, don.TongTien ?? 0);

                // 🔎 Lấy mã phiếu xuất mới nhất
                int maPx = _context.PhieuXuats
                    .OrderByDescending(x => x.MaPx)
                    .Select(x => x.MaPx)
                    .FirstOrDefault();

                // ✔ Thêm chi tiết phiếu xuất + trừ tồn kho bằng raw SQL
                foreach (var ct in don.ChiTietDonHangs)
                {
                    _context.Database.ExecuteSqlRaw(
                        @"INSERT INTO ChiTietPhieuXuat (MaPX, MaSP, SoLuong, DonGia)
                  VALUES ({0}, {1}, {2}, {3})",
                        maPx, ct.MaSp, ct.SoLuong, ct.DonGia);

                    _context.Database.ExecuteSqlRaw(
                        @"UPDATE SanPham 
                  SET SoLuongTon = SoLuongTon - {0}
                  WHERE MaSP = {1}",
                        ct.SoLuong, ct.MaSp);
                }
            }

            return RedirectToAction("DonChoXuLy");
        }
        public IActionResult GiaoChoVanChuyen(int id)
        {
            var don = _context.DonHangs.Find(id);
            if (don == null) return NotFound();

            // ✔ Cập nhật trạng thái đơn
            _context.Database.ExecuteSqlRaw(
                "UPDATE DonHang SET TrangThai = N'Chờ giao' WHERE MaDH = {0}", id);

            // ✔ Tạo bản ghi giao hàng bằng RAW SQL
            _context.Database.ExecuteSqlRaw(
                @"INSERT INTO GiaoHang (MaDH, DonViGiao, TrangThai, NgayGiao, MaNhanVienGiao)
          VALUES ({0}, N'Nội bộ PetShop', N'Chờ giao', GETDATE(), NULL)",
                id);

            return RedirectToAction("DonChoXuLy");
        }


        public IActionResult BaoCao()
        {
            // ================================
            // 1️⃣ THỐNG KÊ SẢN PHẨM
            // ================================
            ViewBag.TongSanPham = _context.SanPhams.Count();

            // Sản phẩm sắp hết
            var sapHet = _context.SanPhams
                .Where(sp => sp.SoLuongTon <= 5)
                .Include(sp => sp.MaDanhMucNavigation)
                .ToList();

            ViewBag.SapHet = sapHet.Count;
            ViewBag.SanPhamSapHet = sapHet;

            // Tổng tồn kho
            ViewBag.TongTonKho = _context.SanPhams.Sum(sp => sp.SoLuongTon ?? 0);

            // Nhập – xuất tháng này
            var thang = DateTime.Now.Month;
            var nam = DateTime.Now.Year;

            ViewBag.NhapThang = _context.PhieuNhaps
                .Count(p => p.NgayNhap.HasValue &&
                            p.NgayNhap.Value.Month == thang &&
                            p.NgayNhap.Value.Year == nam);

            ViewBag.XuatThang = _context.PhieuXuats
                .Count(p => p.NgayXuat.HasValue &&
                            p.NgayXuat.Value.Month == thang &&
                            p.NgayXuat.Value.Year == nam);


            // ================================
            // 2️⃣ THỐNG KÊ NHẬP – XUẤT 6 THÁNG
            // ================================
            var thongKeNX = new List<dynamic>();

            for (int i = 5; i >= 0; i--)
            {
                var t = DateTime.Now.AddMonths(-i).Month;
                var y = DateTime.Now.AddMonths(-i).Year;

                var soPN = _context.PhieuNhaps.Count(p =>
                    p.NgayNhap.HasValue && p.NgayNhap.Value.Month == t && p.NgayNhap.Value.Year == y);

                var soPX = _context.PhieuXuats.Count(p =>
                    p.NgayXuat.HasValue && p.NgayXuat.Value.Month == t && p.NgayXuat.Value.Year == y);

                var tongNhap = _context.PhieuNhaps
                    .Where(p => p.NgayNhap.HasValue &&
                                p.NgayNhap.Value.Month == t &&
                                p.NgayNhap.Value.Year == y)
                    .Sum(p => (decimal?)p.TongTien ?? 0);

                var tongXuat = _context.PhieuXuats
                    .Where(p => p.NgayXuat.HasValue &&
                                p.NgayXuat.Value.Month == t &&
                                p.NgayXuat.Value.Year == y)
                    .Sum(p => (decimal?)p.TongTien ?? 0);

                thongKeNX.Add(new
                {
                    Thang = t,
                    SoPN = soPN,
                    SoPX = soPX,
                    TongNhap = tongNhap,
                    TongXuat = tongXuat
                });
            }

            ViewBag.ThongKeNX = thongKeNX;


            // ================================
            // 3️⃣ THỐNG KÊ ĐƠN HÀNG
            // ================================
            ViewBag.TongDonHang = _context.DonHangs.Count(d => d.TrangThai != "Đã hủy");

            var today = DateTime.Today;
            ViewBag.DonMoiHomNay = _context.DonHangs
                .Count(d => d.NgayDat.HasValue && d.NgayDat.Value.Date == today);

            ViewBag.DonDangXuLy = _context.DonHangs
                .Count(d => d.TrangThai == "Đã xác nhận"
                         || d.TrangThai == "Đang chuẩn bị"
                         || d.TrangThai == "Chờ giao");

            ViewBag.DonHoanTat = _context.DonHangs.Count(d => d.TrangThai == "Hoàn tất");


            // Doanh thu tháng này = phiếu xuất + đơn hoàn tất
            var now = DateTime.Now;

            var doanhThuPX_Thang = _context.PhieuXuats
                .Where(px => px.NgayXuat.HasValue &&
                             px.NgayXuat.Value.Month == now.Month &&
                             px.NgayXuat.Value.Year == now.Year)
                .Sum(px => (decimal?)px.TongTien ?? 0);

            var doanhThuDH_Thang = _context.DonHangs
                .Where(d => d.NgayDat.HasValue &&
                            d.NgayDat.Value.Month == now.Month &&
                            d.NgayDat.Value.Year == now.Year &&
                            d.TrangThai == "Hoàn tất")
                .SelectMany(d => d.ChiTietDonHangs)
                .Sum(ct => (decimal?)(ct.SoLuong * ct.DonGia) ?? 0);

            ViewBag.DoanhThuThang = doanhThuPX_Thang + doanhThuDH_Thang;


            // ================================
            // 4️⃣ THỐNG KÊ ĐƠN HÀNG 6 THÁNG
            // ================================
            var thongKeDonHang = new List<dynamic>();

            for (int i = 5; i >= 0; i--)
            {
                var t = DateTime.Now.AddMonths(-i).Month;
                var y = DateTime.Now.AddMonths(-i).Year;

                var soDH = _context.DonHangs.Count(d =>
                    d.NgayDat.HasValue &&
                    d.NgayDat.Value.Month == t &&
                    d.NgayDat.Value.Year == y &&
                    d.TrangThai != "Đã hủy");

                // Doanh thu từ PHIẾU XUẤT
                var doanhThuPX = _context.PhieuXuats
                    .Where(px => px.NgayXuat.HasValue &&
                                 px.NgayXuat.Value.Month == t &&
                                 px.NgayXuat.Value.Year == y)
                    .Sum(px => (decimal?)px.TongTien ?? 0);

                // Doanh thu từ ĐƠN HÀNG HOÀN TẤT → tính đúng từ chi tiết đơn
                var doanhThuDH = _context.DonHangs
                    .Where(d => d.NgayDat.HasValue &&
                                d.NgayDat.Value.Month == t &&
                                d.NgayDat.Value.Year == y &&
                                d.TrangThai == "Hoàn tất")
                    .SelectMany(d => d.ChiTietDonHangs)
                    .Sum(ct => (decimal?)(ct.SoLuong * ct.DonGia) ?? 0);

                thongKeDonHang.Add(new
                {
                    Thang = t,
                    SoDon = soDH,
                    DoanhThu = doanhThuPX + doanhThuDH
                });
            }

            ViewBag.ThongKeDonHang = thongKeDonHang;


            // ================================
            // RETURN VIEW
            // ================================
            return View();
        }
    }
}

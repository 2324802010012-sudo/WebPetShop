using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;
using WebPetShop.ViewModels;

namespace WebPetShop.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GioHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🧾 HIỂN THỊ GIỎ HÀNG
        public IActionResult Index()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "⚠️ Vui lòng đăng nhập để tiếp tục!";
                return RedirectToAction("Login", "Auth");
            }

            int maNguoiDung = int.Parse(userId);

            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
                return View(new List<ChiTietGioHang>());

            return View(gioHang.ChiTietGioHangs);
        }

        // 📝 CẬP NHẬT SỐ LƯỢNG
        [HttpPost]
        public IActionResult Update(int maCtgh, int soLuong)
        {
            var chiTiet = _context.ChiTietGioHangs.Find(maCtgh);
            if (chiTiet != null)
            {
                chiTiet.SoLuong = soLuong;
                _context.Update(chiTiet);
                _context.SaveChanges();
                TempData["Success"] = "✅ Cập nhật số lượng thành công!";
            }
            return RedirectToAction("Index");
        }

        // ➕ THÊM SẢN PHẨM VÀO GIỎ
        [HttpGet]
        public IActionResult Them(int id, int soLuong = 1)
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "⚠️ Vui lòng đăng nhập để thêm sản phẩm!";
                return RedirectToAction("Login", "Auth");
            }

            int maNguoiDung = int.Parse(userId);

            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null)
            {
                gioHang = new GioHang { MaNguoiDung = maNguoiDung, NgayTao = DateTime.Now };
                _context.GioHangs.Add(gioHang);
                _context.SaveChanges();
            }

            var chiTiet = _context.ChiTietGioHangs
                .FirstOrDefault(c => c.MaGh == gioHang.MaGh && c.MaSp == id);

            if (chiTiet == null)
            {
                chiTiet = new ChiTietGioHang
                {
                    MaGh = gioHang.MaGh,
                    MaSp = id,
                    SoLuong = soLuong
                };
                _context.ChiTietGioHangs.Add(chiTiet);
            }
            else
            {
                chiTiet.SoLuong += soLuong;
                _context.ChiTietGioHangs.Update(chiTiet);
            }

            _context.SaveChanges();
            TempData["Success"] = "🛒 Đã thêm sản phẩm vào giỏ hàng!";
            return RedirectToAction("Index");
        }

        // ⚡ MUA NGAY
        [HttpGet]
        public IActionResult MuaNgay(int id, int soLuong = 1)
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "⚠️ Vui lòng đăng nhập để mua hàng!";
                return RedirectToAction("Login", "Auth");
            }

            var sp = _context.SanPhams.Find(id);
            if (sp == null) return NotFound();

            ViewBag.SoLuong = soLuong;
            return View("~/Views/ThanhToan/Index.cshtml", sp);
        }

        // ❌ XÓA SẢN PHẨM
        public IActionResult Remove(int id)
        {
            var chiTiet = _context.ChiTietGioHangs.Find(id);
            if (chiTiet != null)
            {
                _context.ChiTietGioHangs.Remove(chiTiet);
                _context.SaveChanges();
                TempData["Success"] = "🗑️ Đã xóa sản phẩm khỏi giỏ hàng!";
            }
            return RedirectToAction("Index");
        }

        // 💳 THANH TOÁN TOÀN BỘ GIỎ
        public IActionResult Checkout()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            int maNguoiDung = int.Parse(userId);

            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
            {
                TempData["Error"] = "⚠️ Giỏ hàng trống!";
                return RedirectToAction("Index");
            }

            return View("~/Views/ThanhToan/Index.cshtml", gioHang);
        }

        // ✅ XÁC NHẬN ĐẶT HÀNG (GIỎ HÀNG + MUA NGAY)
        [HttpPost]
        public IActionResult XacNhan(string HoTen, string SoDienThoai, string DiaChi, string? GhiChu, int? MaSp, int? SoLuong, string PhuongThuc)
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            int maNguoiDung = int.Parse(userId);
            decimal tongTien = 0;

            // ============================================================
            // 🔹 TRƯỜNG HỢP 1: MUA NGAY (1 sản phẩm duy nhất)
            // ============================================================
            if (MaSp != null && SoLuong != null)
            {
                var sp = _context.SanPhams.Find(MaSp);
                if (sp == null)
                    return NotFound("Không tìm thấy sản phẩm cần mua.");

                // ✅ Kiểm tra tồn kho
                if (sp.SoLuongTon < SoLuong.Value)
                {
                    TempData["Error"] = $"❌ Sản phẩm '{sp.TenSp}' chỉ còn {sp.SoLuongTon} sản phẩm trong kho.";
                    return RedirectToAction("Index", "GioHang");
                }

                tongTien = sp.Gia * SoLuong.Value;

                var donHang = new DonHang
                {
                    MaNguoiDung = maNguoiDung,
                    NgayDat = DateTime.Now,
                    TrangThai = "Chờ xác nhận",
                    HoTenNhan = HoTen,
                    SoDienThoai = SoDienThoai,
                    DiaChiGiao = DiaChi,
                    PhuongThucThanhToan = PhuongThuc,
                    GhiChu = GhiChu
                };
                _context.DonHangs.Add(donHang);
                _context.SaveChanges();

                _context.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaDh = donHang.MaDh,
                    MaSp = sp.MaSp,
                    SoLuong = SoLuong.Value,
                    DonGia = sp.Gia
                });
                _context.SaveChanges();

                // ✅ Trừ tồn kho sản phẩm
                sp.SoLuongTon -= SoLuong.Value;
                _context.Update(sp);
                _context.SaveChanges();

                // ✅ Lưu lịch sử đơn hàng
                _context.LichSuTrangThaiDonHangs.Add(new LichSuTrangThaiDonHang
                {
                    MaDh = donHang.MaDh,
                    TrangThaiCu = null,
                    TrangThaiMoi = "Chờ xác nhận",
                    NguoiThucHien = maNguoiDung,
                    NgayCapNhat = DateTime.Now
                });
                _context.SaveChanges();

                // 🧾 Nếu chọn Online → thêm bản ghi ThanhToanTrucTuyen
                if (PhuongThuc == "Online")
                {
                    var thanhToan = new ThanhToanTrucTuyen
                    {
                        MaDh = donHang.MaDh,
                        MaGiaoDich = "GD" + DateTime.Now.Ticks,
                        PhuongThuc = "Online",
                        SoTien = tongTien,
                        TrangThai = "Đang xử lý"
                    };
                    _context.ThanhToanTrucTuyens.Add(thanhToan);
                    _context.SaveChanges();

                    // donHang.MaTttt = thanhToan.MaTttt; // nếu có cột
                    _context.SaveChanges();
                }

                TempData["Success"] = "🎉 Đặt hàng thành công!";
                return RedirectToAction("ThanhToanThanhCong", new { maDh = donHang.MaDh });
            }

            // ============================================================
            // 🔹 TRƯỜNG HỢP 2: THANH TOÁN GIỎ HÀNG
            // ============================================================
            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
            {
                TempData["Error"] = "⚠️ Giỏ hàng trống!";
                return RedirectToAction("Index");
            }

            foreach (var ct in gioHang.ChiTietGioHangs)
            {
                // ✅ Kiểm tra tồn kho từng sản phẩm
                if (ct.MaSpNavigation.SoLuongTon < ct.SoLuong)
                {
                    TempData["Error"] = $"❌ Sản phẩm '{ct.MaSpNavigation.TenSp}' chỉ còn {ct.MaSpNavigation.SoLuongTon} trong kho.";
                    return RedirectToAction("Index");
                }

                tongTien += (ct.SoLuong ?? 1) * ct.MaSpNavigation.Gia;
            }

            var donHangGH = new DonHang
            {
                MaNguoiDung = maNguoiDung,
                NgayDat = DateTime.Now,
                TrangThai = "Chờ xác nhận",
                HoTenNhan = HoTen,
                SoDienThoai = SoDienThoai,
                DiaChiGiao = DiaChi,
                PhuongThucThanhToan = PhuongThuc,
                GhiChu = GhiChu
            };
            _context.DonHangs.Add(donHangGH);
            _context.SaveChanges();

            foreach (var ct in gioHang.ChiTietGioHangs)
            {
                _context.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaDh = donHangGH.MaDh,
                    MaSp = ct.MaSp,
                    SoLuong = ct.SoLuong ?? 1,
                    DonGia = ct.MaSpNavigation.Gia
                });

                // ✅ Trừ tồn kho
                ct.MaSpNavigation.SoLuongTon -= (ct.SoLuong ?? 1);
                _context.Update(ct.MaSpNavigation);
            }

            _context.ChiTietGioHangs.RemoveRange(gioHang.ChiTietGioHangs);
            _context.SaveChanges();

            // ✅ Lưu lịch sử đơn hàng
            _context.LichSuTrangThaiDonHangs.Add(new LichSuTrangThaiDonHang
            {
                MaDh = donHangGH.MaDh,
                TrangThaiCu = null,
                TrangThaiMoi = "Chờ xác nhận",
                NguoiThucHien = maNguoiDung,
                NgayCapNhat = DateTime.Now
            });
            _context.SaveChanges();

            // 🧾 Nếu chọn Online → thêm bản ghi ThanhToanTrucTuyen
            if (PhuongThuc == "Online")
            {
                var thanhToan = new ThanhToanTrucTuyen
                {
                    MaDh = donHangGH.MaDh,
                    MaGiaoDich = "GD" + DateTime.Now.Ticks,
                    PhuongThuc = "Online",
                    SoTien = tongTien,
                    TrangThai = "Đang xử lý"
                };
                _context.ThanhToanTrucTuyens.Add(thanhToan);
                _context.SaveChanges();

                // donHangGH.MaTttt = thanhToan.MaTttt; // nếu có cột này
                _context.SaveChanges();
            }

            TempData["Success"] = "🎉 Đặt hàng thành công!";
            return RedirectToAction("ThanhToanThanhCong", new { maDh = donHangGH.MaDh });
        }

        // 🎉 TRANG XÁC NHẬN ĐẶT HÀNG THÀNH CÔNG
        public IActionResult ThanhToanThanhCong(int maDh)
        {
            var donHang = _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .Include(d => d.MaNguoiDungNavigation)
                .FirstOrDefault(d => d.MaDh == maDh);

            if (donHang == null)
                return RedirectToAction("Index", "SanPham");

            return View(donHang);
        }
    }
}

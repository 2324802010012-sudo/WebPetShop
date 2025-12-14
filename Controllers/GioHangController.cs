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
                return RedirectToAction("Login", "Auth");

            int maNguoiDung = int.Parse(userId);

            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                    .ThenInclude(ct => ct.MaSpNavigation)
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
                return View(new CheckoutVM());

            decimal tongTien = 0;

            foreach (var ct in gioHang.ChiTietGioHangs)
            {
                var sp = ct.MaSpNavigation;

                ct.GiaGoc = sp.Gia;
                ct.GiaSauGiam = GiaSauGiam(sp);

                tongTien += (ct.SoLuong ?? 1) * ct.GiaSauGiam;
            }

            var vm = new CheckoutVM
            {
                GioHang = gioHang.ChiTietGioHangs.ToList(),
                TienHang = tongTien,
                TongTien = tongTien,
                PhiVanChuyen = 0
            };

            return View(vm);
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

                // ✅ Cập nhật lại số lượng giỏ hàng trong Session
                var gioHang = _context.GioHangs
                    .Include(g => g.ChiTietGioHangs)
                    .FirstOrDefault(g => g.MaGh == chiTiet.MaGh);
                int tong = gioHang?.ChiTietGioHangs.Sum(c => c.SoLuong ?? 0) ?? 0;
                HttpContext.Session.SetInt32("CartCount", tong);

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

            // ✅ Cập nhật lại tổng số lượng trong Session
            int tongSoLuong = _context.ChiTietGioHangs
                .Where(c => c.MaGh == gioHang.MaGh)
                .Sum(c => c.SoLuong ?? 0);
            HttpContext.Session.SetInt32("CartCount", tongSoLuong);

            TempData["Success"] = "🛒 Đã thêm sản phẩm vào giỏ hàng!";
            return RedirectToAction("Index");
        }

        // ⚡ MUA NGAY
        public IActionResult MuaNgay(int id, int soLuong = 1, decimal gia = 0)
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            var sp = _context.SanPhams.Find(id);
            if (sp == null) return NotFound();

            // Nếu có giá giảm từ URL thì dùng giá đó
            var giaBan = gia > 0 ? gia : GiaSauGiam(sp);

            var vm = new CheckoutVM
            {
                SanPhamMuaNgay = sp,
                SoLuong = soLuong,
                TienHang = giaBan * soLuong,
                TongTien = giaBan * soLuong,
                GioHang = new List<ChiTietGioHang>(),
                PhiVanChuyen = 0
            };

            return View("~/Views/GioHang/Checkout.cshtml", vm);
        }

        // ❌ XÓA SẢN PHẨM
        public IActionResult Remove(int id)
        {
            var chiTiet = _context.ChiTietGioHangs.Find(id);
            if (chiTiet != null)
            {
                _context.ChiTietGioHangs.Remove(chiTiet);
                _context.SaveChanges();

                // ✅ Cập nhật lại Session sau khi xóa sản phẩm
                var gioHang = _context.GioHangs
                    .Include(g => g.ChiTietGioHangs)
                    .FirstOrDefault(g => g.MaGh == chiTiet.MaGh);
                int tongSoLuong = gioHang?.ChiTietGioHangs.Sum(c => c.SoLuong ?? 0) ?? 0;
                HttpContext.Session.SetInt32("CartCount", tongSoLuong);

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

            decimal tong = 0;

            // TÍNH GIÁ GIẢM ✓
            foreach (var ct in gioHang.ChiTietGioHangs)
            {
                var sp = ct.MaSpNavigation;

                ct.GiaGoc = sp.Gia;
                ct.GiaSauGiam = GiaSauGiam(sp);

                tong += (ct.SoLuong ?? 1) * ct.GiaSauGiam;
            }

            var vm = new CheckoutVM
            {
                GioHang = gioHang.ChiTietGioHangs.ToList(),
                PhiGiaoHangList = _context.PhiGiaoHangs.ToList(),
                TienHang = tong,
                GiamGia = 0,
                PhiVanChuyen = 0,
                TongTien = tong
            };

            return View("~/Views/GioHang/Checkout.cshtml", vm);
        }


        // ✅ XÁC NHẬN ĐẶT HÀNG (THEO CHECKOUTVM)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XacNhan(CheckoutVM model)
        {
            string? userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            int maNguoiDung = int.Parse(userId);

            // ⭐ KIỂM TRA MUA NGAY
            bool isMuaNgay = Request.Form["IsMuaNgay"] == "true";
            int maSp = 0;
            int soLuong = 0;

            if (isMuaNgay)
            {
                maSp = Convert.ToInt32(Request.Form["MaSp"]);
                soLuong = Convert.ToInt32(Request.Form["SoLuong"]);
            }

            var donHang = new DonHang
            {
                MaNguoiDung = maNguoiDung,
                NgayDat = DateTime.Now,
                TrangThai = "Chờ xác nhận",
                HoTenNhan = model.HoTenNhan,
                SoDienThoai = model.SDTNhan,
                DiaChiGiao = model.DiaChiGiao,
                PhuongThucThanhToan = model.PhuongThuc
            };
            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            decimal tongTien = 0;

            // -------------------------
            // ⭐ 1. XỬ LÝ MUA NGAY
            // -------------------------
            if (isMuaNgay)
            {
                var sp = _context.SanPhams.Find(maSp);
                if (sp == null) return RedirectToAction("Index", "SanPham");

                _context.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaDh = donHang.MaDh,
                    MaSp = sp.MaSp,
                    SoLuong = soLuong,
                    DonGia = GiaSauGiam(sp)

                });

                sp.SoLuongTon -= soLuong;
                _context.Update(sp);

                tongTien = sp.Gia * soLuong;
            }
            else
            {
                // -------------------------
                // ⭐ 2. XỬ LÝ CHECKOUT GIỎ HÀNG
                // -------------------------
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
                    _context.ChiTietDonHangs.Add(new ChiTietDonHang
                    {
                        MaDh = donHang.MaDh,
                        MaSp = ct.MaSp,
                        SoLuong = ct.SoLuong ?? 1,
                        DonGia = GiaSauGiam(ct.MaSpNavigation)

                    });

                    ct.MaSpNavigation.SoLuongTon -= (ct.SoLuong ?? 1);
                    _context.Update(ct.MaSpNavigation);

                    tongTien += (ct.SoLuong ?? 1) * ct.MaSpNavigation.Gia;
                }

                _context.ChiTietGioHangs.RemoveRange(gioHang.ChiTietGioHangs);
                HttpContext.Session.SetInt32("CartCount", 0);
            }

            // Lịch sử
            _context.LichSuTrangThaiDonHangs.Add(new LichSuTrangThaiDonHang
            {
                MaDh = donHang.MaDh,
                TrangThaiCu = null,
                TrangThaiMoi = "Chờ xác nhận",
                NguoiThucHien = maNguoiDung,
                NgayCapNhat = DateTime.Now
            });

            _context.SaveChanges();

            TempData["Success"] = "🎉 Đặt hàng thành công!";
            return RedirectToAction("ThanhToanThanhCong", new { maDh = donHang.MaDh });
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
        [HttpGet]
        public IActionResult GetPhiShip(string tinh, string huyen)
        {
            tinh = tinh.ToLower();
            huyen = huyen.ToLower();

            // Nếu trong tỉnh Bình Dương
            if (tinh.Contains("bình dương"))
            {
                if (huyen.Contains("thủ dầu một"))
                {
                    var phi = _context.PhiGiaoHangs.FirstOrDefault(x => x.KhuVuc == "Nội thành");
                    return Json(phi);
                }
                else
                {
                    var phi = _context.PhiGiaoHangs.FirstOrDefault(x => x.KhuVuc == "Ngoại thành");
                    return Json(phi);
                }
            }

            // Tỉnh khác → Toàn quốc
            var qc = _context.PhiGiaoHangs.FirstOrDefault(x => x.KhuVuc == "Toàn quốc");
            return Json(qc);
        }

        // ⭐ Hàm tính giá sau giảm — phiên bản đúng nhất
        private decimal GiaSauGiam(SanPham sp)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            decimal giaGoc = sp.Gia;

            var km = _context.KhuyenMais
                .Where(x =>
                    x.TrangThai == true &&
                    x.MaSP == sp.MaSp &&
                    x.NgayBatDau <= today &&
                    x.NgayKetThuc >= today
                )
                .FirstOrDefault();

            if (km == null)
                return giaGoc;

            decimal phanTram = km.PhanTramGiam ?? 0;

            decimal mucGiam = giaGoc * (phanTram / 100m);

            return giaGoc - mucGiam;
        }


    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WebPetShop.Data;
using WebPetShop.Models;
using ClosedXML.Excel;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace WebPetShop.Controllers
{
    public class KeToanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KeToanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // 📌 DASHBOARD KẾ TOÁN
        // =====================================================
        public IActionResult Index()
        {
            var now = DateTime.Now;
            int thang = now.Month;
            int nam = now.Year;

            ViewBag.TongHoaDon = _context.HoaDons.Count();

            ViewBag.DoanhThuThang = _context.HoaDons
                .Where(h => h.NgayLap.HasValue &&
                            h.NgayLap.Value.Month == thang &&
                            h.NgayLap.Value.Year == nam)
                .Sum(h => (decimal?)h.SoTien ?? 0);

            ViewBag.DonCODChuaThu = _context.DonHangs
                .Where(d => d.PhuongThucThanhToan == "COD" &&
                            d.TrangThai == "Đã giao")
                .Count();

            ViewBag.TongChiPhiNhap = _context.PhieuNhaps
                .Where(p => p.NgayNhap.HasValue &&
                            p.NgayNhap.Value.Month == thang &&
                            p.NgayNhap.Value.Year == nam)
                .Sum(p => (decimal?)p.TongTien ?? 0);

            return View();
        }

        // =====================================================
        // 📄 DANH SÁCH HÓA ĐƠN
        // =====================================================
        public IActionResult HoaDon()
        {
            var ds = _context.HoaDons
                .Include(h => h.MaDhNavigation)
                .Include(h => h.MaKyGuiNavigation)
                .Include(h => h.MaKeToanNavigation)
                .OrderByDescending(h => h.NgayLap)
                .ToList();

            return View(ds);
        }


        // =====================================================
        // 📄 CHI TIẾT HÓA ĐƠN
        // =====================================================
        public IActionResult ChiTietHoaDon(int id)
        {
            var hd = _context.HoaDons
                .Include(h => h.MaDhNavigation)
                    .ThenInclude(d => d.ChiTietDonHangs)
                        .ThenInclude(ct => ct.MaSpNavigation)
                .Include(h => h.MaKyGuiNavigation)
                .Include(h => h.MaKeToanNavigation)
                .FirstOrDefault(h => h.MaHd == id);

            if (hd == null) return NotFound();

            return View(hd);
        }

        // =====================================================
        // 🐾 LẬP HÓA ĐƠN KÝ GỬI
        // =====================================================
        [HttpGet]
        public IActionResult LapHoaDonKyGui(int maKyGui)
        {
            var kg = _context.KyGuiThuCungs
                .Include(k => k.MaKhNavigation)
                .FirstOrDefault(k => k.MaKyGui == maKyGui);

            if (kg == null) return NotFound();

            return View(kg);
        }

        [HttpPost]
        public IActionResult LapHoaDonKyGui(int MaKyGui, decimal SoTien, string HinhThuc, string? GhiChu)
        {
            var hd = new HoaDon
            {
                MaKyGui = MaKyGui,
                MaKeToan = HttpContext.Session.GetInt32("UserId") ?? 2,
                SoTien = SoTien,
                HinhThuc = HinhThuc,
                GhiChu = GhiChu,
                NgayLap = DateTime.Now
            };

            _context.HoaDons.Add(hd);
            _context.SaveChanges();

            TempData["Success"] = "Đã lập hóa đơn ký gửi thành công!";
            return RedirectToAction("HoaDon");
        }

        // =====================================================
        // 💰 BÁO CÁO DOANH THU – CHI PHÍ – LỢI NHUẬN
        // =====================================================
        public IActionResult BaoCaoTaiChinh(
       DateTime? from,
       DateTime? to,
       int? month,
       int? year,
       string loaiHd,             // "donhang" | "kygui" | null
       string hinhThuc,           // "COD" | "Chuyển khoản" | null
       int? keToan                // id người lập hóa đơn
   )
        {
            var query = _context.HoaDons
                .Include(h => h.MaKeToanNavigation)
                .AsQueryable();

            // ===========================
            // Lọc ngày
            // ===========================
            if (from.HasValue)
                query = query.Where(h => h.NgayLap >= from);

            if (to.HasValue)
                query = query.Where(h => h.NgayLap <= to);

            // ===========================
            // Lọc tháng – năm
            // ===========================
            if (month.HasValue)
                query = query.Where(h => h.NgayLap.Value.Month == month);

            if (year.HasValue)
                query = query.Where(h => h.NgayLap.Value.Year == year);
            else
                year = DateTime.Now.Year;

            // ===========================
            // Lọc loại hóa đơn
            // ===========================
            if (!string.IsNullOrEmpty(loaiHd))
            {
                if (loaiHd == "donhang")
                    query = query.Where(h => h.MaDh != null);

                if (loaiHd == "kygui")
                    query = query.Where(h => h.MaKyGui != null);
            }

            // ===========================
            // Lọc hình thức thanh toán
            // ===========================
            if (!string.IsNullOrEmpty(hinhThuc))
                query = query.Where(h => h.HinhThuc == hinhThuc);

            // ===========================
            // Lọc theo người lập
            // ===========================
            if (keToan.HasValue)
                query = query.Where(h => h.MaKeToan == keToan);

            // ===========================
            // KPI tổng hợp
            // ===========================
            ViewBag.DoanhThu = query.Sum(h => (decimal?)h.SoTien) ?? 0;

            var chiPhi = _context.PhieuNhaps
                .Where(p => p.NgayNhap.HasValue && p.NgayNhap.Value.Year == year)
                .Sum(p => (decimal?)p.TongTien) ?? 0;

            ViewBag.ChiPhi = chiPhi;
            ViewBag.LoiNhuan = (ViewBag.DoanhThu - chiPhi);

            // ===========================
            // Biểu đồ đường: 12 tháng
            // ===========================
            var thongKe = new List<dynamic>();

            for (int i = 1; i <= 12; i++)
            {
                var dt = query
                    .Where(h => h.NgayLap.Value.Month == i)
                    .Sum(h => (decimal?)h.SoTien) ?? 0;

                var cpThang = _context.PhieuNhaps
                    .Where(p => p.NgayNhap.HasValue &&
                                p.NgayNhap.Value.Month == i &&
                                p.NgayNhap.Value.Year == year)
                    .Sum(p => (decimal?)p.TongTien) ?? 0;

                thongKe.Add(new { Thang = i, DoanhThu = dt, ChiPhi = cpThang, LoiNhuan = dt - cpThang });
            }

            ViewBag.ThongKe = thongKe;

            // ==========================================================
            // 🔥 THÊM MỚI: TỶ TRỌNG CHI PHÍ NHẬP HÀNG (Pie Chart)
            // ==========================================================
            var chiPhiTheoThang = _context.PhieuNhaps
                .Where(p => p.NgayNhap.HasValue && p.NgayNhap.Value.Year == year)
                .GroupBy(p => p.NgayNhap.Value.Month)
                .Select(g => new {
                    Thang = g.Key,
                    TongTien = g.Sum(x => x.TongTien)
                })
                .OrderBy(x => x.Thang)
                .ToList();

            ViewBag.ChiPhiTyTrong = new
            {
                Labels = chiPhiTheoThang.Select(x => "Tháng " + x.Thang).ToList(),
                Values = chiPhiTheoThang.Select(x => (decimal)x.TongTien).ToList()
            };

            // ==========================================================
            // 🔥 THÊM MỚI: DOANH THU THEO DANH MỤC (Bar Chart)
            // ==========================================================
            var doanhThuTheoDanhMuc = _context.ChiTietDonHangs
                .Include(ct => ct.MaSpNavigation)
                    .ThenInclude(sp => sp.MaDanhMucNavigation)
                .Include(ct => ct.MaDhNavigation)
                .Where(ct => ct.MaDhNavigation.NgayDat.HasValue &&
                             ct.MaDhNavigation.NgayDat.Value.Year == year)
                .GroupBy(ct => ct.MaSpNavigation.MaDanhMucNavigation.TenDanhMuc)
                .Select(g => new {
                    DanhMuc = g.Key,
                    DoanhThu = g.Sum(x => x.SoLuong * x.DonGia)
                })
                .ToList();

            ViewBag.DoanhThuTheoDanhMuc = new
            {
                Labels = doanhThuTheoDanhMuc.Select(x => x.DanhMuc).ToList(),
                Values = doanhThuTheoDanhMuc.Select(x => (decimal)x.DoanhThu).ToList()
            };

            // ===========================
            // Danh sách kế toán
            // ===========================
            ViewBag.DanhSachKeToan = _context.NguoiDungs
                .Where(n => n.VaiTro == "KeToan")
                .ToList();

            return View();
        }

        // =====================================================
        // 📌 QUẢN LÝ KHUYẾN MÃI
        // =====================================================
        // =====================================================
        // 📌 QUẢN LÝ KHUYẾN MÃI + LỌC + TÌM KIẾM
        // =====================================================
        public IActionResult KhuyenMai(string? search, string? status)
        {
            var query = _context.KhuyenMais
                                .Include(x => x.SanPham)
                                .AsQueryable();

            // 🔍 Tìm kiếm theo mã code hoặc mô tả
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.MaCode.Contains(search) ||
                                         x.MoTa.Contains(search));
            }

            // 🔎 Lọc theo trạng thái (true / false)
            if (!string.IsNullOrEmpty(status))
            {
                bool st = status == "true";
                query = query.Where(x => x.TrangThai == st);
            }

            ViewBag.SanPham = _context.SanPhams.ToList();
            ViewBag.Search = search;   // giữ lại giá trị trên ô input
            ViewBag.Status = status;

            return View(query.ToList());
        }

        [HttpGet]
        public IActionResult KhuyenMaiThem()
        {
            ViewBag.SanPham = _context.SanPhams.ToList();
            return View(new KhuyenMai());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KhuyenMaiThem(KhuyenMai model)
        {
            ViewBag.SanPham = _context.SanPhams.ToList();

            if (!ModelState.IsValid)
            {
                // Debug validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = string.Join(" | ", errors);
                return View(model);
            }

            _context.KhuyenMais.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Thêm khuyến mãi thành công!";
            return RedirectToAction("KhuyenMai");
        }



        [HttpGet]
        public IActionResult KhuyenMaiSua(int id)
        {
            var km = _context.KhuyenMais.Find(id);
            if (km == null) return NotFound();

            ViewBag.SanPham = _context.SanPhams.ToList();
            return View(km);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KhuyenMaiSua(KhuyenMai model)
        {
            ViewBag.SanPham = _context.SanPhams.ToList();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var km = _context.KhuyenMais.FirstOrDefault(x => x.MaKm == model.MaKm);
            if (km == null)
            {
                TempData["Error"] = "Không tìm thấy khuyến mãi!";
                return RedirectToAction("KhuyenMai");
            }

            km.MaSP = model.MaSP;
            km.MaCode = model.MaCode;
            km.MoTa = model.MoTa;
            km.PhanTramGiam = model.PhanTramGiam;
            km.GiaTriToiDa = model.GiaTriToiDa;
            km.NgayBatDau = model.NgayBatDau;
            km.NgayKetThuc = model.NgayKetThuc;
            km.SoLanSuDungToiDa = model.SoLanSuDungToiDa;
            km.TrangThai = model.TrangThai;

            _context.Update(km);
            _context.SaveChanges();

            TempData["Success"] = "Cập nhật khuyến mãi thành công!";
            return RedirectToAction("KhuyenMai");
        }

        [HttpGet]
        public IActionResult KhuyenMaiXoa(int id)
        {
            var km = _context.KhuyenMais.FirstOrDefault(x => x.MaKm == id);

            if (km == null)
            {
                TempData["Error"] = "Không tìm thấy khuyến mãi!";
                return RedirectToAction("KhuyenMai");
            }

            _context.KhuyenMais.Remove(km);
            _context.SaveChanges();

            TempData["Success"] = "Xóa khuyến mãi thành công!";
            return RedirectToAction("KhuyenMai");
        }


        public IActionResult ExportExcel()
        {
            int nam = DateTime.Now.Year;

            // Lấy dữ liệu 12 tháng
            var thongKe = new List<dynamic>();
            for (int i = 1; i <= 12; i++)
            {
                var doanhThu = _context.HoaDons
                    .Where(h => h.NgayLap.HasValue &&
                                h.NgayLap.Value.Month == i &&
                                h.NgayLap.Value.Year == nam)
                    .Sum(h => (decimal?)h.SoTien ?? 0);

                var chiPhi = _context.PhieuNhaps
                    .Where(p => p.NgayNhap.HasValue &&
                                p.NgayNhap.Value.Month == i &&
                                p.NgayNhap.Value.Year == nam)
                    .Sum(p => (decimal?)p.TongTien ?? 0);

                thongKe.Add(new
                {
                    Thang = i,
                    DoanhThu = doanhThu,
                    ChiPhi = chiPhi,
                    LoiNhuan = doanhThu - chiPhi
                });
            }

            // Tạo Excel
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Báo Cáo Tài Chính");

                // ============================
                // HEADER
                // ============================
                ws.Cell("A1").Value = "BÁO CÁO TÀI CHÍNH PETSHOP";
                ws.Range("A1:D1").Merge().Style.Font.SetBold().Font.FontSize = 18;
                ws.Range("A1:D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                ws.Cell("A3").Value = "Năm:";
                ws.Cell("B3").Value = nam;

                // ============================
                // TABLE HEADER
                // ============================
                ws.Cell("A5").Value = "Tháng";
                ws.Cell("B5").Value = "Doanh thu (₫)";
                ws.Cell("C5").Value = "Chi phí (₫)";
                ws.Cell("D5").Value = "Lợi nhuận (₫)";

                ws.Range("A5:D5").Style.Font.SetBold();
                ws.Range("A5:D5").Style.Fill.BackgroundColor = XLColor.AshGrey;

                // ============================
                // DỮ LIỆU
                // ============================
                int row = 6;
                foreach (var x in thongKe)
                {
                    ws.Cell(row, 1).Value = "Tháng " + x.Thang;
                    ws.Cell(row, 2).Value = x.DoanhThu;
                    ws.Cell(row, 3).Value = x.ChiPhi;
                    ws.Cell(row, 4).Value = x.LoiNhuan;

                    row++;
                }

                // Auto width
                ws.Columns().AdjustToContents();

                // Lưu file
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"BaoCaoTaiChinh_{nam}.xlsx"
                    );
                }
            }
        }
    }
}

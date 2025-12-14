using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;

namespace WebPetShop.Controllers
{
    public class DichVuController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DichVuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trang giới thiệu dịch vụ

        public IActionResult Index()
        {
            // Lấy 3 bé nổi bật đang hiển thị để đưa vào phần "Một số bé đang chờ nhận nuôi"
            var beNoiBat = _context.BaiDangNhanNuois
                .Include(b => b.MaThuCungNhanNuoiNavigation.MaKyGuiNavigation)
                .Where(b => b.TrangThai == "Đang hiển thị")
                .OrderByDescending(b => b.NgayDang)
                .Take(3)
                .Select(b => new VDanhSachThuCungNhanNuoi
                {
                    MaBaiDang = b.MaBaiDang,
                    TenThuCung = b.MaThuCungNhanNuoiNavigation.MaKyGuiNavigation.TenThuCung,
                    GiongLoai = b.MaThuCungNhanNuoiNavigation.MaKyGuiNavigation.GiongLoai,
                    Tuoi = b.MaThuCungNhanNuoiNavigation.MaKyGuiNavigation.Tuoi,
                    TieuDe = b.TieuDe,
                    MoTa = b.MoTa,
                    HinhAnh = b.HinhAnh,
                    NgayDang = b.NgayDang
                })
                .ToList();

            ViewBag.BeNoiBat = beNoiBat;

            return View();
        }

        // Form đặt ký gửi (GET)
        [HttpGet]
        public IActionResult DatKyGui()
        {
            return View(); // Views/DichVu/DatKyGui.cshtml (file bạn đang dùng)
        }

        // XỬ LÝ ĐƠN ĐẶT KÝ GỬI (POST) – ĐÃ NÂNG CẤP HOÀN TOÀN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatKyGui(KyGuiThuCung model)
        {
            // 1. Kiểm tra đăng nhập
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["Error"] = "Bạn cần đăng nhập để đặt dịch vụ!";
                return View(model);
            }
            int userId = int.Parse(userIdStr);
            model.MaKh = userId;

            // 2. Kiểm tra ngày hợp lệ
            if (!model.NgayGui.HasValue || !model.NgayHetHan.HasValue)
            {
                TempData["Error"] = "Vui lòng chọn đầy đủ ngày gửi và ngày nhận!";
                return View(model);
            }
            if (model.NgayHetHan <= model.NgayGui)
            {
                TempData["Error"] = "Ngày nhận phải sau ngày gửi ít nhất 1 ngày!";
                return View(model);
            }

            // 3. TÍNH GIÁ + SỐ NGÀY (dự phòng nếu JS lỗi)
            decimal donGia = model.LoaiThuCung switch
            {
                "ChoNho" => 80000,
                "ChoLon" => 120000,
                "Meo" => 90000,
                _ => 60000
            };

            int soNgay = (model.NgayHetHan.Value.ToDateTime(TimeOnly.MinValue) -
                         model.NgayGui.Value.ToDateTime(TimeOnly.MinValue)).Days + 1; // +1 để tính cả ngày nhận

            decimal tongTien = soNgay * donGia;

            // 4. Gán giá trị tự động
            model.PhiKyGui = tongTien;
            model.TongTien = tongTien; // dùng cho báo cáo sau này

            // 5. XÁC ĐỊNH TRẠNG THÁI THEO HÌNH THỨC THANH TOÁN
            if (model.HinhThucThanhToan == "ChuyenKhoan")
            {
                model.TrangThaiThanhToan = "DaThanhToan";
                model.TrangThaiDon = "DangChamSoc";        // hiện ngay bên chăm sóc
                model.TrangThai = "Đang ký gửi";
            }
            else // Tiền mặt
            {
                model.TrangThaiThanhToan = "ChoXacNhan";
                model.TrangThaiDon = "ChoXacNhan";          // chờ lễ tân xác nhận
                model.TrangThai = "Chờ xác nhận thanh toán";
            }

            // 6. Lưu đơn ký gửi
            _context.KyGuiThuCungs.Add(model);
            await _context.SaveChangesAsync();

            // 7. Tạo hóa đơn (giữ nguyên logic cũ + cập nhật trạng thái)
            var hoaDon = new HoaDon
            {
                MaKyGui = model.MaKyGui,
                MaKeToan = 2,
                NgayLap = DateTime.Now,
                SoTien = tongTien,
                HinhThuc = model.HinhThucThanhToan == "ChuyenKhoan" ? "Chuyển khoản" : "Tiền mặt (chưa thu)",
                GhiChu = $"Ký gửi {soNgay} ngày | {model.TenThuCung} | {donGia:N0}/ngày"
            };
            _context.HoaDons.Add(hoaDon);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đặt ký gửi thành công! Tổng tiền: {tongTien:N0} ₫";
            return RedirectToAction("LichSuKyGui", "KyGuiThuCung");
        }

        // Danh sách đơn ký gửi cho lễ tân / admin
        public async Task<IActionResult> DanhSachDonKyGui()
        {
            var data = await _context.KyGuiThuCungs
                .Include(k => k.MaKhNavigation)
                .OrderByDescending(k => k.MaKyGui)
                .ToListAsync();

            return View(data);
        }

        // LỄ TÂN XÁC NHẬN ĐÃ THU TIỀN MẶT
        [HttpPost]
        public async Task<IActionResult> XacNhanThanhToan(int id)
        {
            var don = await _context.KyGuiThuCungs.FindAsync(id);
            if (don == null)
            {
                TempData["Error"] = "Không tìm thấy đơn ký gửi!";
                return RedirectToAction("DanhSachDonKyGui");
            }

            don.TrangThaiThanhToan = "DaThanhToan";
            don.TrangThaiDon = "DangChamSoc";    // BÂY GIỜ MỚI HIỆN Ở PHẦN CHĂM SÓC
            don.TrangThai = "Đang ký gửi";

            // Cập nhật hóa đơn
            var hd = await _context.HoaDons.FirstOrDefaultAsync(h => h.MaKyGui == id);
            if (hd != null)
            {
                hd.HinhThuc = "Tiền mặt (đã thu)";
                hd.NgayLap = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Đã xác nhận thanh toán cho đơn #{id}";
            return RedirectToAction("DanhSachDonKyGui");
        }

        // Các action cũ bạn đã có (giữ nguyên 100%)
        [HttpGet]
        public async Task<IActionResult> NhanNuoi()
        {
            var danhSachThuCung = await _context.DanhSachThuCungNhanNuoi
                .OrderByDescending(x => x.NgayDang)
                .ToListAsync();
            return View(danhSachThuCung);
        }

        public IActionResult LichSuKyGui()
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung") ?? int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
            if (userId == 0) return RedirectToAction("Login", "Auth");

            var danhSach = _context.KyGuiThuCungs
                .Where(k => k.MaKh == userId)
                .OrderByDescending(k => k.NgayGui)
                .ToList();
            return View(danhSach);
        }

        public IActionResult ChiTiet(int id)
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung") ?? int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
            if (userId == 0) return RedirectToAction("Login", "Auth");

            var kyGui = _context.KyGuiThuCungs
                .Include(k => k.MaKhNavigation)
                .Include(k => k.ChiTietChamSocs)
                .FirstOrDefault(k => k.MaKyGui == id && k.MaKh == userId);

            return kyGui == null ? NotFound() : View(kyGui);
        }

        [HttpPost]
        public IActionResult HuyKyGui(int id)
        {
            var userId = HttpContext.Session.GetInt32("MaNguoiDung") ?? int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
            if (userId == 0) return RedirectToAction("Login", "Auth");

            var kyGui = _context.KyGuiThuCungs.FirstOrDefault(k => k.MaKyGui == id && k.MaKh == userId);
            if (kyGui == null || kyGui.TrangThai != "Đang ký gửi")
            {
                TempData["Error"] = "Không thể hủy đơn này!";
                return RedirectToAction("LichSuKyGui");
            }

            kyGui.TrangThai = "Đã hủy";
            _context.SaveChanges();
            TempData["Success"] = "Hủy dịch vụ thành công!";
            return RedirectToAction("LichSuKyGui");
        }
    }
}
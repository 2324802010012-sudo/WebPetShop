using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;

public class GiaoHangController : Controller
{
    private readonly ApplicationDbContext _context;

    public GiaoHangController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ===================== TRANG CHÍNH =====================
    public IActionResult Index()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "GiaoHang" && role != "Admin")
            return RedirectToAction("Login", "Auth");

        return View();
    }

    // ===================== DANH SÁCH ĐƠN GIAO =====================
    public IActionResult DanhSachGiaoHang()
    {
        var ds = _context.DonHangs
            .Include(d => d.MaNguoiDungNavigation)
            .Where(d =>
                d.TrangThai == "Chờ giao" ||
                d.TrangThai == "Chờ lấy hàng" ||
                d.TrangThai == "Đang giao" ||
                d.TrangThai == "Hoàn tất")
            .OrderByDescending(d => d.NgayDat)
            .ToList();

        return View(ds);
    }

    // ===================== CHI TIẾT ĐƠN =====================
    public IActionResult ChiTiet(int id)
    {
        var don = _context.DonHangs
            .Include(d => d.MaNguoiDungNavigation)
            .Include(d => d.ChiTietDonHangs).ThenInclude(ct => ct.MaSpNavigation)
            .FirstOrDefault(d => d.MaDh == id);

        if (don == null) return NotFound();

        return View(don);
    }

    // ===================== BẮT ĐẦU GIAO =====================
    public IActionResult BatDauGiao(int id)
    {
        var don = _context.DonHangs.Find(id);
        if (don == null) return NotFound();

        don.TrangThai = "Đang giao";
        _context.SaveChanges();

        return RedirectToAction("ChiTiet", new { id });
    }

    // ===================== VIEW CẬP NHẬT TRẠNG THÁI =====================
    public IActionResult CapNhatTrangThai(int id)
    {
        var don = _context.DonHangs
            .Include(d => d.HinhThucThanhToanThucTeNavigation)
            .FirstOrDefault(d => d.MaDh == id);

        if (don == null) return NotFound();

        ViewBag.DanhSachHTTT = _context.HinhThucThanhToanThucTes.ToList();
        return View(don);
    }

    // ===================== POST CẬP NHẬT TRẠNG THÁI =====================
    [HttpPost]
    public IActionResult CapNhatTrangThai(int id, string trangThai, int? thanhToanThucTe)
    {
        var don = _context.DonHangs
            .FirstOrDefault(d => d.MaDh == id);

        if (don == null) return NotFound();

        string trangThaiCu = don.TrangThai;

        // Cập nhật trạng thái mới
        don.TrangThai = trangThai;

        // Đánh dấu EF biết entity đã bị thay đổi
        _context.Entry(don).State = EntityState.Modified;

        // ==========================
        // XỬ LÝ COD KHI HOÀN TẤT
        // ==========================
        if (don.PhuongThucThanhToan == "COD" && trangThai == "Hoàn tất")
        {
            if (thanhToanThucTe == null)
            {
                TempData["Error"] = "Bạn phải chọn hình thức thu COD!";
                return RedirectToAction("CapNhatTrangThai", new { id });
            }

            don.MaHTTT = thanhToanThucTe;
            string hinhThuc = thanhToanThucTe == 1 ? "Tiền mặt" : "Chuyển khoản";

            TaoHoaDonTuDong(don, hinhThuc);
        }

        // ==========================
        // THANH TOÁN ONLINE
        // ==========================
        if (don.PhuongThucThanhToan == "Online" && trangThai == "Hoàn tất")
        {
            TaoHoaDonTuDong(don, "Online");
        }

        // ==========================
        // LƯU LỊCH SỬ
        // ==========================
        _context.LichSuTrangThaiDonHangs.Add(new LichSuTrangThaiDonHang
        {
            MaDh = don.MaDh,
            TrangThaiCu = trangThaiCu,
            TrangThaiMoi = trangThai,
            NgayCapNhat = DateTime.Now,
            NguoiThucHien = HttpContext.Session.GetInt32("UserId")
        });

        _context.SaveChanges();

        TempData["Success"] = "Cập nhật thành công!";
        return RedirectToAction("DanhSachGiaoHang");
    }


    // ===================== HÀM TẠO HÓA ĐƠN TỰ ĐỘNG =====================
    private void TaoHoaDonTuDong(DonHang don, string hinhThuc)
    {
        // 1️⃣ Kiểm tra đã có hóa đơn hay chưa
        if (_context.HoaDons.Any(h => h.MaDh == don.MaDh))
            return;

        // 2️⃣ Tính tổng tiền từ chi tiết đơn hàng
        decimal tongTien = _context.ChiTietDonHangs
            .Where(ct => ct.MaDh == don.MaDh)
            .Sum(ct => ct.SoLuong * ct.DonGia);

        // 3️⃣ Nếu bằng 0 (đơn hàng rỗng?), fallback từ DonHang
        if (tongTien == 0)
            tongTien = don.TongTien ?? 0;

        // 4️⃣ Tạo hóa đơn
        var hd = new HoaDon
        {
            MaDh = don.MaDh,
            MaKeToan = HttpContext.Session.GetInt32("UserId") ?? 2,
            SoTien = tongTien,
            HinhThuc = hinhThuc,
            NgayLap = DateTime.Now,
            GhiChu = "Tự tạo khi giao hàng hoàn tất"
        };

        _context.HoaDons.Add(hd);
        _context.SaveChanges();
    }

    // ===================== HOÀN TẤT =====================
    public IActionResult HoanTat(int id)
    {
        var don = _context.DonHangs.Find(id);
        if (don == null) return NotFound();

        don.TrangThai = "Hoàn tất";
        _context.SaveChanges();

        return RedirectToAction("ChiTiet", new { id });
    }

    // ===================== BÁO CÁO GIAO HÀNG =====================
    public IActionResult BaoCaoGiaoHang()
    {
        ViewBag.TongGiao = _context.DonHangs.Count(d =>
            d.TrangThai == "Chờ giao" ||
            d.TrangThai == "Đang chuẩn bị" ||
            d.TrangThai == "Hoàn tất" ||
            d.TrangThai == "Đang giao");

        ViewBag.DangGiao = _context.DonHangs.Count(d =>
            d.TrangThai == "Đang chuẩn bị" ||
            d.TrangThai == "Chờ giao");

        ViewBag.ThanhCong = _context.DonHangs.Count(d => d.TrangThai == "Hoàn tất");

        ViewBag.ThatBai = _context.DonHangs.Count(d => d.TrangThai == "Đã hủy");

        // ======= 6 THÁNG GẦN NHẤT =======
        var thongKe = new List<dynamic>();

        for (int i = 5; i >= 0; i--)
        {
            var t = DateTime.Now.AddMonths(-i).Month;
            var y = DateTime.Now.AddMonths(-i).Year;

            var tong = _context.DonHangs.Count(d =>
                d.NgayDat.HasValue &&
                d.NgayDat.Value.Month == t &&
                d.NgayDat.Value.Year == y);

            var thanhCong = _context.DonHangs.Count(d =>
                d.NgayDat.HasValue &&
                d.NgayDat.Value.Month == t &&
                d.NgayDat.Value.Year == y &&
                d.TrangThai == "Hoàn tất");

            var thatBai = _context.DonHangs.Count(d =>
                d.NgayDat.HasValue &&
                d.NgayDat.Value.Month == t &&
                d.NgayDat.Value.Year == y &&
                d.TrangThai == "Đã hủy");

            thongKe.Add(new { Thang = t, Tong = tong, ThanhCong = thanhCong, ThatBai = thatBai });
        }

        ViewBag.ThongKeGiaoHang = thongKe;
        return View();
    }
}

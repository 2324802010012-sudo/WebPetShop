using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebPetShop.Data;
using WebPetShop.Models;

namespace WebPetShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string _conn;
        public AdminController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _conn = configuration.GetConnectionString("DefaultConnection");
        }

        // 🏠 Dashboard
        public IActionResult Index()
        {
            // ✅ Thống kê tổng
            ViewBag.TongNguoiDung = _context.NguoiDungs.Count();
            ViewBag.TongSanPham = _context.SanPhams.Count();
            ViewBag.TongBaiDang = _context.BaiDangNhanNuois.Count();
            ViewBag.TongDanhMuc = _context.DanhMucs.Count();

            // ✅ Đơn hàng gần đây
            var donHangGanDay = _context.DonHangs
                .Include(d => d.MaNguoiDungNavigation)
                .OrderByDescending(d => d.NgayDat)
                .Take(5)
                .ToList();

            ViewBag.DonHangGanDay = donHangGanDay ?? new List<DonHang>();

            // ✅ Biểu đồ doanh thu
            var now = DateTime.Now;
            var last7Days = Enumerable.Range(0, 7).Select(i => now.AddDays(-i).Date).Reverse().ToList();

            var doanhThuData = last7Days.Select(d =>
                _context.DonHangs
                    .Where(x => x.NgayDat.HasValue && x.NgayDat.Value.Date == d && x.TrangThai == "Đã giao")
                    .Sum(x => (decimal?)x.TongTien) ?? 0
            ).ToList();

            ViewBag.NgayDoanhThu = last7Days.Select(d => d.ToString("dd/MM")).ToList();
            ViewBag.GiaTriDoanhThu = doanhThuData;

            // ✅ Biểu đồ trạng thái
            var groupTrangThai = _context.DonHangs
                .GroupBy(d => d.TrangThai)
                .Select(g => new { TrangThai = g.Key ?? "Không xác định", SoLuong = g.Count() })
                .ToList();

            ViewBag.TrangThaiDonHang = groupTrangThai.Select(g => g.TrangThai).ToList();
            ViewBag.SoLuongTrangThai = groupTrangThai.Select(g => g.SoLuong).ToList();

            // ✅ Chống null
            ViewBag.NgayDoanhThu ??= new List<string>();
            ViewBag.GiaTriDoanhThu ??= new List<decimal>();
            ViewBag.TrangThaiDonHang ??= new List<string>();
            ViewBag.SoLuongTrangThai ??= new List<int>();

            return View();
        }

        // 👤 Quản lý người dùng
        public IActionResult QuanLyNguoiDung()
        {
            var list = _context.NguoiDungs.OrderByDescending(u => u.MaNguoiDung).ToList();
            return View(list);
        }

        // 📊 Giám sát hệ thống
        public IActionResult GiamSatHeThong()
        {
            var lichSu = _context.LichSuHeThongs
                .Include(l => l.MaNguoiDungNavigation)
                .OrderByDescending(l => l.NgayThucHien)
                .Take(100)
                .ToList();

            // ✅ Thống kê theo loại hành động
            var thongKeLoai = lichSu
                .GroupBy(l => l.HanhDong)
                .Select(g => new { Ten = g.Key, SoLuong = g.Count() })
                .ToList();

            // ✅ Thống kê theo ngày (7 ngày gần nhất)
            var now = DateTime.Now.Date;
            var last7Days = Enumerable.Range(0, 7).Select(i => now.AddDays(-i)).Reverse().ToList();

            var thongKeNgay = last7Days.Select(d => new
            {
                Ngay = d.ToString("dd/MM"),
                SoLuong = lichSu.Count(x => x.NgayThucHien.HasValue && x.NgayThucHien.Value.Date == d)

            }).ToList();

            ViewBag.ThongKeLoai = thongKeLoai;
            ViewBag.ThongKeNgay = thongKeNgay;
            return View(lichSu);
        }

        // 🐾 Duyệt bài đăng nhận nuôi
        public IActionResult DuyetBaiDang()
        {
            var list = _context.BaiDangNhanNuois
                .Include(b => b.MaNguoiTaoNavigation)
                .OrderByDescending(b => b.NgayDang)
                .ToList();

            return View(list);
        }

        [HttpPost]
        public IActionResult DuyetBai(int id, string hanhDong)
        {
            var bai = _context.BaiDangNhanNuois.Find(id);
            if (bai == null) return NotFound();

            if (hanhDong == "duyet") bai.TrangThai = "Đã duyệt";
            else if (hanhDong == "tu_choi") bai.TrangThai = "Từ chối";

            _context.Update(bai);
            _context.SaveChanges();

            TempData["Success"] = "✅ Cập nhật trạng thái bài đăng thành công!";
            return RedirectToAction("DuyetBaiDang");
        }

        // 🧩 Quản lý danh mục sản phẩm
        public IActionResult DanhMucSanPham()
        {
            var list = _context.DanhMucs.OrderBy(d => d.TenDanhMuc).ToList();
            return View(list);
        }
        // 🧾 Xem chi tiết đơn hàng (AJAX)
        [HttpGet]
        public IActionResult ChiTietDonHang(int id)
        {
            using var conn = _context.Database.GetDbConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_ChiTietDonHang";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            var p = cmd.CreateParameter();
            p.ParameterName = "@MaDh";
            p.Value = id;
            cmd.Parameters.Add(p);

            using var reader = cmd.ExecuteReader();

            // Đọc đơn hàng
            dynamic donHang = null;
            if (reader.Read())
            {
                donHang = new
                {
                    MaDh = reader["MaDh"],
                    KhachHang = reader["KhachHang"],
                    NgayDat = reader["NgayDat"],
                    TrangThai = reader["TrangThai"],
                    TongTien = reader["TongTien"]
                };
            }

            // Chuyển sang bảng 2 (chi tiết)
            var chiTiet = new List<object>();
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    chiTiet.Add(new
                    {
                        SanPham = reader["SanPham"],
                        SoLuong = reader["SoLuong"],
                        DonGia = reader["DonGia"],
                        ThanhTien = reader["ThanhTien"]
                    });
                }
            }

            if (donHang == null)
                return Json(new { success = false, message = "Không tìm thấy đơn hàng!" });

            return Json(new { success = true, donHang.MaDh, donHang.KhachHang, donHang.NgayDat, donHang.TrangThai, donHang.TongTien, ChiTiet = chiTiet });
        }


        // 🔄 Cập nhật trạng thái đơn hàng
    
        [HttpPost]
        public IActionResult CapNhatTrangThai(int id, string trangThai)
        {
            try
            {
                int? nguoiThucHien = null;
                var userId = HttpContext.Session.GetString("UserId");
                if (!string.IsNullOrEmpty(userId))
                    nguoiThucHien = int.Parse(userId);

                var result = _context.Database
                    .SqlQueryRaw<(int Success, string Message)>(
                        "EXEC sp_CapNhatTrangThaiDonHang @p0, @p1, @p2",
                        id, trangThai, nguoiThucHien
                    )
                    .AsEnumerable()
                    .FirstOrDefault();

                return Json(new { success = result.Success == 1, message = result.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
   

        // ==============================
        // 👤 QUẢN LÝ NGƯỜI DÙNG (AJAX)
        // ==============================

        [HttpGet]
        public IActionResult LayNguoiDung(int id)
        {
            var nd = _context.NguoiDungs.Find(id);
            if (nd == null) return Json(new { success = false, message = "Không tìm thấy người dùng." });

            return Json(new
            {
                success = true,
                data = new
                {
                    nd.MaNguoiDung,
                    nd.HoTen,
                    nd.Email,
                    nd.SoDienThoai,
                    nd.VaiTro,
                    TrangThai = nd.TrangThai == true ? "Hoạt động" : "Khóa"
                }
            });
        }
        [HttpPost]
        public IActionResult CapNhatNguoiDung(int id, string hoTen, string email, string soDienThoai, string vaiTro)
        {
            try
            {
                using var conn = new SqlConnection(_conn);
                using var cmd = new SqlCommand("sp_CapNhatNguoiDung", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", id);
                cmd.Parameters.AddWithValue("@HoTen", hoTen ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@VaiTro", vaiTro ?? (object)DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Json(new { success = true, message = "✅ Đã cập nhật thông tin người dùng (SP)." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // =============================
        // 🔒 KHÓA / MỞ NGƯỜI DÙNG - GỌI SP
        // =============================
        [HttpPost]
        public IActionResult KhoaNguoiDung(int id)
        {
            try
            {
                using var conn = new SqlConnection(_conn);
                using var cmd = new SqlCommand("sp_KhoaNguoiDung", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", id);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Json(new { success = true, message = "🔄 Cập nhật trạng thái tài khoản (SP)." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // =============================
        // 🗑️ XÓA NGƯỜI DÙNG - GỌI SP
        // =============================
        [HttpPost]
        public IActionResult XoaNguoiDung(int id)
        {
            try
            {
                using var conn = new SqlConnection(_conn);
                using var cmd = new SqlCommand("sp_XoaNguoiDung", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNguoiDung", id);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Json(new { success = true, message = "🗑️ Đã xóa người dùng (SP)." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
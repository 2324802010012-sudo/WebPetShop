using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;

namespace WebPetShop.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================================
        // 🔹 LOGIN (GET)
        // ==========================================================
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // ==========================================================
        // 🔹 LOGIN (POST)
        // ==========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string TenDangNhap, string MatKhau, string? returnUrl = null)
        {
            // 1️⃣ Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(TenDangNhap) || string.IsNullOrWhiteSpace(MatKhau))
            {
                TempData["Error"] = "⚠️ Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            // 2️⃣ Tìm tài khoản
            var user = await _context.NguoiDungs
                .FirstOrDefaultAsync(x => x.TenDangNhap == TenDangNhap);

            if (user == null || user.TrangThai == false)
            {
                TempData["Error"] = "❌ Tài khoản không tồn tại hoặc đã bị khóa.";
                return View();
            }

            // 3️⃣ So khớp mật khẩu
            var incomingHash = HashPassword(MatKhau);
            bool matched = false;

            if (LooksLikeSha256(user.MatKhau))
            {
                matched = user.MatKhau == incomingHash;
            }
            else
            {
                matched = user.MatKhau == MatKhau;
                if (matched)
                {
                    // ✅ Nếu mật khẩu cũ dạng thường → tự động nâng cấp lên SHA256
                    user.MatKhau = incomingHash;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
            }

            if (!matched)
            {
                TempData["Error"] = "❌ Tên đăng nhập hoặc mật khẩu không đúng.";
                return View();
            }

            // 4️⃣ Lưu Session
            HttpContext.Session.SetString("UserId", user.MaNguoiDung.ToString());
            HttpContext.Session.SetString("Role", user.VaiTro ?? "KhachHang");
            HttpContext.Session.SetString("HoTen", user.HoTen ?? user.TenDangNhap);
            // ✅ Tự động cập nhật số lượng giỏ hàng sau khi đăng nhập
            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .FirstOrDefault(g => g.MaNguoiDung == user.MaNguoiDung);

            int tongSoLuong = gioHang?.ChiTietGioHangs.Sum(c => c.SoLuong ?? 0) ?? 0;
            HttpContext.Session.SetInt32("CartCount", tongSoLuong);


            // 5️⃣ Điều hướng theo vai trò hoặc quay lại trang trước đó
            // 5️⃣ Điều hướng theo vai trò hoặc quay lại trang trước đó
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl); // Nếu có returnUrl → quay lại trang trước

            string role = user.VaiTro?.Trim() ?? "KhachHang";

            switch (role)
            {
                case "Admin":
                    return RedirectToAction("Index", "Admin");

                case "LeTan":
                    return RedirectToAction("Index", "LeTan");

                case "KeToan":
                    return RedirectToAction("Index", "KeToan");

                case "Kho":
                    return RedirectToAction("Index", "Kho"); // ✅ VÀO GIAO DIỆN KHO

                case "ChamSoc":
                    return RedirectToAction("Index", "ChamSoc");

                case "GiaoHang":
                    return RedirectToAction("Index", "GiaoHang");

                default:
                    return RedirectToAction("Index", "Home"); // Khách hàng
            }
        }
            // ==========================================================
            // 🔹 REGISTER
            // ==========================================================
            [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string HoTen, string TenDangNhap, string Email, string MatKhau)
        {
            if (string.IsNullOrWhiteSpace(HoTen) ||
                string.IsNullOrWhiteSpace(TenDangNhap) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(MatKhau))
            {
                TempData["Error"] = "⚠️ Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            // Kiểm tra trùng tên đăng nhập
            if (await _context.NguoiDungs.AnyAsync(x => x.TenDangNhap == TenDangNhap))
            {
                TempData["Error"] = "❌ Tên đăng nhập đã tồn tại.";
                return View();
            }

            // Kiểm tra trùng email
            if (await _context.NguoiDungs.AnyAsync(x => x.Email == Email))
            {
                TempData["Error"] = "❌ Email đã được sử dụng.";
                return View();
            }

            var user = new NguoiDung
            {
                HoTen = HoTen,
                TenDangNhap = TenDangNhap,
                Email = Email,
                MatKhau = HashPassword(MatKhau),
                VaiTro = "KhachHang",
                TrangThai = true
            };

            _context.NguoiDungs.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "🎉 Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        // ==========================================================
        // 🔹 FORGOT PASSWORD (theo yêu cầu đồ án)
        // ==========================================================
        [HttpGet]
        public IActionResult Forgot() => View();

        // ==========================================================
        // 🔹 LOGOUT
        // ==========================================================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ==========================================================
        // 🔹 Helper functions (Hash, kiểm tra SHA256)
        // ==========================================================
        private static string HashPassword(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        private static bool LooksLikeSha256(string? s)
            => !string.IsNullOrEmpty(s) && s.Length == 64 && s.All(IsHex);

        private static bool IsHex(char c)
            => (c >= '0' && c <= '9') ||
               (c >= 'a' && c <= 'f') ||
               (c >= 'A' && c <= 'F');
    }
}

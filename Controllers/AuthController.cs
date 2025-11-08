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

        // ========== LOGIN ==========
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string TenDangNhap, string MatKhau)
        {
            if (string.IsNullOrWhiteSpace(TenDangNhap) || string.IsNullOrWhiteSpace(MatKhau))
            {
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            var user = await _context.NguoiDungs
                .FirstOrDefaultAsync(x => x.TenDangNhap == TenDangNhap);

            if (user == null || user.TrangThai == false)
            {
                TempData["Error"] = "Tài khoản không tồn tại hoặc đã bị khóa.";
                return View();
            }

            // So khớp hash
            var incomingHash = HashPassword(MatKhau);
            bool matched = false;

            if (LooksLikeSha256(user.MatKhau))
            {
                // DB đã lưu dạng SHA256
                matched = user.MatKhau == incomingHash;
            }
            else
            {
                // DB còn mật khẩu thuần (cũ) → so plain và nâng cấp lên hash nếu đúng
                matched = user.MatKhau == MatKhau;
                if (matched)
                {
                    user.MatKhau = incomingHash; // nâng cấp lên SHA256
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
            }

            if (!matched)
            {
                TempData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return View();
            }

            // Set session
            HttpContext.Session.SetString("UserId", user.MaNguoiDung.ToString());
            HttpContext.Session.SetString("Role", user.VaiTro ?? "KhachHang");
            HttpContext.Session.SetString("HoTen", user.HoTen ?? user.TenDangNhap);

            // Điều hướng theo vai trò
            // ✅ Điều hướng theo vai trò
            if ((user.VaiTro ?? "").Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");

        }

        // ========== REGISTER ==========
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
                TempData["Error"] = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            // Kiểm tra trùng username / email
            bool existsUser = await _context.NguoiDungs.AnyAsync(x => x.TenDangNhap == TenDangNhap);
            if (existsUser)
            {
                TempData["Error"] = "Tên đăng nhập đã tồn tại.";
                return View();
            }

            bool existsEmail = await _context.NguoiDungs.AnyAsync(x => x.Email == Email);
            if (existsEmail)
            {
                TempData["Error"] = "Email đã được sử dụng.";
                return View();
            }

            var user = new NguoiDung
            {
                HoTen = HoTen,
                TenDangNhap = TenDangNhap,
                Email = Email,
                MatKhau = HashPassword(MatKhau),  // Lưu dạng SHA256
                VaiTro = "KhachHang",
                TrangThai = true
            };

            _context.NguoiDungs.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        // ========== FORGOT (placeholder theo yêu cầu đồ án) ==========
        [HttpGet]
        public IActionResult Forgot() => View();

        // ========== LOGOUT ==========
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ========== Helpers ==========
        private static string HashPassword(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        private static bool LooksLikeSha256(string? s)
            => !string.IsNullOrEmpty(s) && s.Length == 64 && s.All(IsHex);
        private static bool IsHex(char c)
            => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
    }
}

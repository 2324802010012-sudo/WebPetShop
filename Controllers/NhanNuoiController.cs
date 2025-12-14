using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Data;
using WebPetShop.Models;

namespace WebPetShop.Controllers
{
    //[Authorize(Roles = "Admin,LeTan,NhanVien")]
    public class NhanNuoiController : Controller
    {
        private readonly ApplicationDbContext _context;
        public NhanNuoiController(ApplicationDbContext context)
        {
            _context = context;
        }
        private const string TRANG_THAI_DANG_HIEN_THI = "Đang hiển thị";
        private const string TRANG_THAI_DA_CO_CHU = "Đã có chủ";

        // 1. Danh sách ký gửi quá 30 ngày sau khi hết hạn → lễ tân thấy để chuyển
        public async Task<IActionResult> DanhSachDeXuat()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var data = await _context.KyGuiThuCungs
                .Where(k => k.NgayHetHan != null)
                .Where(k => k.NgayHetHan.Value.AddDays(30) < today)
                .Where(k => k.TrangThaiDon != "HoanThanh" && k.TrangThaiDon != "DaHuy")
                .Where(k => !_context.ThuCungNhanNuois.Any(t => t.MaKyGui == k.MaKyGui))
                .Select(k => new ThuCungNhanNuoiView
                {
                    MaBaiDang = k.MaKyGui,
                    TenThuCung = k.TenThuCung,
                    GiongLoai = k.GiongLoai ?? "Không rõ",
                    Tuoi = k.Tuoi,
                    TrangThaiKyGui = k.TrangThaiDon ?? "Chưa xác định",
                    HinhAnh = "/images/default-pet.jpg" // bạn có thể thêm trường ảnh sau nếu cần
                })
                .ToListAsync();

            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> ChuyenSangNhanNuoi(int maKyGui)
        {
            var exist = await _context.ThuCungNhanNuois.AnyAsync(x => x.MaKyGui == maKyGui);
            if (exist)
            {
                TempData["Error"] = "Thú cưng đã được chuyển sang nhận nuôi trước đó!";
                return RedirectToAction("DanhSachDeXuat");
            }

            var nn = new ThuCungNhanNuoi
            {
                MaKyGui = maKyGui,
                NgayChuyen = DateOnly.FromDateTime(DateTime.Today),
                TrangThai = "Chờ đăng bài"
            };
            _context.ThuCungNhanNuois.Add(nn);

            var kyGui = await _context.KyGuiThuCungs.FindAsync(maKyGui);
            if (kyGui != null)
                kyGui.TrangThaiDon = "HoanThanh";

            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã chuyển sang nhận nuôi!";
            return RedirectToAction("DanhSachDeXuat");
        }

        // 2. Ajax: Chuyển sang nhận nuôi
        [HttpPost]
        public async Task<JsonResult> ChuyenSangNhanNuoi_Ajax(int maKyGui)
        {
            var exist = await _context.ThuCungNhanNuois.AnyAsync(x => x.MaKyGui == maKyGui);
            if (exist)
                return Json(new { success = false, message = "Thú cưng đã được chuyển sang nhận nuôi trước đó!" });

            var nn = new ThuCungNhanNuoi
            {
                MaKyGui = maKyGui,
                NgayChuyen = DateOnly.FromDateTime(DateTime.Today),
                TrangThai = "Chờ đăng bài"
            };
            _context.ThuCungNhanNuois.Add(nn);

            // Cập nhật trạng thái đơn ký gửi
            var kyGui = await _context.KyGuiThuCungs.FindAsync(maKyGui);
            if (kyGui != null)
            {
                kyGui.TrangThaiDon = "HoanThanh"; // hoặc tạo trạng thái mới: "ChuyenNhanNuoi"
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Đã chuyển sang danh sách nhận nuôi!" });
        }

        // 3. Danh sách đang chờ lễ tân tạo bài đăng
        public async Task<IActionResult> ChoTaoBaiDang()
        {
            var data = await _context.ThuCungNhanNuois
                .Include(t => t.MaKyGuiNavigation)
                .Where(t => t.TrangThai == "Chờ đăng bài")   // ❗ Chỉ lấy thú chưa đăng bài

                // ❗ KHÔNG lấy thú đã có bài đăng
                .Where(t => !_context.BaiDangNhanNuois
                                .Any(b => b.MaThuCungNhanNuoi == t.MaNhanNuoi))

                .Select(t => new ThuCungNhanNuoiView
                {
                    MaNhanNuoi = t.MaNhanNuoi,
                    MaKyGui = t.MaKyGui,
                    TenThuCung = t.MaKyGuiNavigation.TenThuCung,
                    GiongLoai = t.MaKyGuiNavigation.GiongLoai ?? "Không rõ giống",
                    Tuoi = t.MaKyGuiNavigation.Tuoi,
                    HinhAnh = "/images/default-pet.jpg"
                })
                .ToListAsync();

            return View(data);
        }

        // ===========================
        // 4. FORM TẠO BÀI ĐĂNG
        // ===========================
        public async Task<IActionResult> TaoBaiDang(int maNhanNuoi)
        {
            var nn = await _context.ThuCungNhanNuois
                .Include(t => t.MaKyGuiNavigation)
                .FirstOrDefaultAsync(t => t.MaNhanNuoi == maNhanNuoi);

            if (nn == null)
                return NotFound();

            ViewBag.ThongTin = new
            {
                Ten = nn.MaKyGuiNavigation.TenThuCung,
                GiongLoai = nn.MaKyGuiNavigation.GiongLoai,
                Tuoi = nn.MaKyGuiNavigation.Tuoi,
                LoaiThuCung = nn.MaKyGuiNavigation.LoaiThuCung
            };

            return View(new BaiDangNhanNuoi
            {
                MaThuCungNhanNuoi = maNhanNuoi,
                NgayDang = DateOnly.FromDateTime(DateTime.Today)
            });
        }




        // ========================================
        // POST: LƯU BÀI ĐĂNG
        // ========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaoBaiDang(BaiDangNhanNuoi model, IFormFile? hinhAnh)
        {
            await RebuildViewBag(model.MaThuCungNhanNuoi);


            if (!ModelState.IsValid)
                return View(model);

            string imgPath = "/images/default-pet.jpg";

            if (hinhAnh != null && hinhAnh.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(hinhAnh.FileName);
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(folder);

                var filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await hinhAnh.CopyToAsync(stream);
                }

                imgPath = "/uploads/" + fileName;
            }

            model.HinhAnh = imgPath;
            model.NgayDang = DateOnly.FromDateTime(DateTime.Today);
            model.TrangThai = "Đang hiển thị";
            model.MaNguoiTao = int.Parse(HttpContext.Session.GetString("UserId"));

            _context.BaiDangNhanNuois.Add(model);

            var tc = await _context.ThuCungNhanNuois.FindAsync(model.MaThuCungNhanNuoi);
            if (tc != null)
                tc.TrangThai = "Đã đăng bài";

            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng bài thành công!";

            // ⭐⭐ QUAN TRỌNG — QUAY LẠI TRANG CHỜ TẠO BÀI ⭐⭐
            return RedirectToAction("ChoTaoBaiDang");
        }

        [HttpPost]
        public async Task<JsonResult> TaoBaiDang_Ajax(BaiDangNhanNuoi model, IFormFile? hinhAnh)
        {
            if (string.IsNullOrWhiteSpace(model.TieuDe))
                return Json(new { success = false, message = "Vui lòng nhập tiêu đề!" });

            if (string.IsNullOrWhiteSpace(model.MoTa))
                return Json(new { success = false, message = "Vui lòng nhập mô tả!" });

            // Gắn người đăng (lễ tân)
            int maNguoiTao = int.Parse(HttpContext.Session.GetString("UserId"));
            model.MaNguoiTao = maNguoiTao;

            // Xử lý hình ảnh
            string imgPath = "/images/default-pet.jpg";

            if (hinhAnh != null && hinhAnh.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(hinhAnh.FileName);
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(folder);

                var filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await hinhAnh.CopyToAsync(stream);
                }

                imgPath = "/uploads/" + fileName;
            }

            model.HinhAnh = imgPath;
            model.NgayDang = DateOnly.FromDateTime(DateTime.Today);
            model.TrangThai = "Đang hiển thị";

            _context.BaiDangNhanNuois.Add(model);

            var nn = await _context.ThuCungNhanNuois.FindAsync(model.MaThuCungNhanNuoi);
            if (nn != null)
                nn.TrangThai = "Đã đăng bài";

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đăng bài thành công!" });
        }


        // ========================================
        // HÀM PHỤ: GẮN LẠI THÔNG TIN THÚ CƯNG CHO VIEW
        // ========================================
        private async Task RebuildViewBag(int maNhanNuoi)
        {
            var nn = await _context.ThuCungNhanNuois
                .Include(t => t.MaKyGuiNavigation)
                .FirstOrDefaultAsync(t => t.MaNhanNuoi == maNhanNuoi);

            if (nn != null)
            {
                ViewBag.ThongTin = new
                {
                    Ten = nn.MaKyGuiNavigation.TenThuCung,
                    GiongLoai = nn.MaKyGuiNavigation.GiongLoai ?? "Không rõ",
                    Tuoi = nn.MaKyGuiNavigation.Tuoi ?? 0,
                    LoaiThuCung = nn.MaKyGuiNavigation.LoaiThuCung
                };
            }
        }

        // 5. Trang công khai khách xem danh sách nhận nuôi
        [AllowAnonymous]
        public async Task<IActionResult> DanhSachNhanNuoi()
        {
            var data = await _context.BaiDangNhanNuois
                .Include(b => b.MaThuCungNhanNuoiNavigation.MaKyGuiNavigation)
                .Where(b => b.TrangThai == "Đang hiển thị")
                .OrderByDescending(b => b.NgayDang)
                .Select(b => new VDanhSachThuCungNhanNuoi
                {
                    MaBaiDang = b.MaBaiDang,
                    TenThuCung = b.MaThuCungNhanNuoiNavigation.MaKyGuiNavigation.TenThuCung,
                    GiongLoai = b.MaThuCungNhanNuoiNavigation.MaKyGuiNavigation.GiongLoai,
                    Tuoi = b.MaThuCungNhanNuoiNavigation.MaKyGuiNavigation.Tuoi,
                    TieuDe = b.TieuDe,
                    MoTa = b.MoTa,
                    HinhAnh = b.HinhAnh,
                    TrangThaiBaiDang = b.TrangThai,
                    NgayDang = b.NgayDang
                })
                .ToListAsync();

            return View(data);
        }

        // NhanNuoiController.cs – ĐÃ FIX 100% CHO BẢNG NguoiDung

        // 6. Khách gửi yêu cầu nhận nuôi
        [HttpPost]
        public async Task<JsonResult> GuiYeuCauNhanNuoi(int maBaiDang)
        {
            var maKhStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(maKhStr) || !int.TryParse(maKhStr, out int maKh))
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });

            // Kiểm tra đã gửi chưa
            bool daGui = await _context.YeuCauNhanNuois
                .AnyAsync(y => y.MaBaiDang == maBaiDang && y.MaNguoiDung == maKh);

            if (daGui)
                return Json(new { success = false, message = "Bạn đã gửi yêu cầu cho bé này rồi!" });

            var baiDang = await _context.BaiDangNhanNuois
                .FirstOrDefaultAsync(b => b.MaBaiDang == maBaiDang && b.TrangThai == TRANG_THAI_DANG_HIEN_THI);

            if (baiDang == null)
                return Json(new { success = false, message = "Bài đăng không tồn tại hoặc đã có chủ!" });

            // Kiểm tra đã có người được duyệt chưa
            bool daDuyet = await _context.YeuCauNhanNuois
                .AnyAsync(y => y.MaBaiDang == maBaiDang && y.TrangThai == "Đã duyệt");

            if (daDuyet)
                return Json(new { success = false, message = "Bé này đã có người nhận nuôi!" });

            var yeuCau = new YeuCauNhanNuoi
            {
                MaBaiDang = maBaiDang,
                MaNguoiDung = maKh,        // ← Đúng cột trong SQL
                NgayYeuCau = DateTime.Now,
                TrangThai = "Chờ xác nhận"
            };

            _context.YeuCauNhanNuois.Add(yeuCau);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Gửi yêu cầu thành công! Lễ tân sẽ liên hệ bạn sớm!" });
        }

        // 7. Lễ tân: Danh sách tất cả yêu cầu nhận nuôi
        //[Authorize(Roles = "Admin,LeTan,NhanVien")]
        public async Task<IActionResult> LichSuNhanNuoi()
        {
            var data = await _context.YeuCauNhanNuois
                // THÊM DÒNG NÀY – QUAN TRỌNG NHẤT!
                .Include(y => y.BaiDangNavigation!)
                    .ThenInclude(b => b.MaThuCungNhanNuoiNavigation!)
                    .ThenInclude(t => t.MaKyGuiNavigation)
                .Include(y => y.NguoiDungNavigation)
                .OrderByDescending(y => y.NgayYeuCau)
                .ToListAsync();

            return View(data);
        }

        // // Xác nhận / Từ chối (lễ tân)
        [HttpPost]
        public async Task<JsonResult> XacNhanNhanNuoi(int maYeuCau, bool xacNhan)
        {
            try
            {
                var yeuCau = await _context.YeuCauNhanNuois
    .AsTracking()
    .Include(y => y.BaiDangNavigation)
    .FirstOrDefaultAsync(y => y.MaYeuCau == maYeuCau);


                if (yeuCau == null)
                    return Json(new { success = false, message = "Không tìm thấy yêu cầu!" });

                if (yeuCau.TrangThai != "Chờ xác nhận")
                    return Json(new { success = false, message = "Yêu cầu đã được xử lý rồi!" });

                // CẬP NHẬT TRẠNG THÁI TRONG BẢNG YeuCauNhanNuoi
                if (xacNhan)
                {
                    yeuCau.TrangThai = "Đã xác nhận";
                    if (yeuCau.BaiDangNavigation != null)
                        yeuCau.BaiDangNavigation.TrangThai = "Đã có chủ"; // hoặc TRANG_THAI_DA_CO_CHU
                }
                else
                {
                    yeuCau.TrangThai = "Đã từ chối";
                }

                // DÒNG QUAN TRỌNG NHẤT – BẮT BUỘC PHẢI CÓ!
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đã cập nhật trạng thái thành công!" });
            }
            catch (Exception ex)
            {
                // Nếu có lỗi → bạn sẽ thấy ngay trong console hoặc Swal
                return Json(new { success = false, message = "Lỗi CSDL: " + ex.Message });
            }
        }

        // 8. Khách xem lịch sử yêu cầu của mình
        //[Authorize(Roles = "KhachHang")]
        public async Task<IActionResult> LichSuCuaToi()
        {
            if (!int.TryParse(HttpContext.Session.GetString("UserId"), out int maKh))
                return RedirectToAction("Login", "Account");

            var data = await _context.YeuCauNhanNuois
                .AsNoTracking() // THÊM DÒNG NÀY NỮA!
                .Include(y => y.BaiDangNavigation!)
                    .ThenInclude(b => b.MaThuCungNhanNuoiNavigation!)
                    .ThenInclude(t => t.MaKyGuiNavigation)
                .Where(y => y.MaNguoiDung == maKh)
                .OrderByDescending(y => y.NgayYeuCau)
                .ToListAsync();

            return View(data);
        }
        // Khách hủy yêu cầu
        [HttpPost]
        //[Authorize(Roles = "KhachHang")]
        public async Task<JsonResult> HuyYeuCau(int maYeuCau)
        {
            if (!int.TryParse(HttpContext.Session.GetString("UserId"), out int maKh))
                return Json(new { success = false });

            var yc = await _context.YeuCauNhanNuois
                .FirstOrDefaultAsync(y => y.MaYeuCau == maYeuCau && y.MaNguoiDung == maKh);

            if (yc == null || yc.TrangThai != "Chờ xác nhận")
                return Json(new { success = false, message = "Không thể hủy yêu cầu này!" });

            yc.TrangThai = "Đã hủy";
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã hủy yêu cầu thành công!" });
        }

        // API REALTIME: Lấy trạng thái mới nhất của yêu cầu nhận nuôi
        // Dùng cho trang LichSuCuaToi – tự động cập nhật không cần F5
        [HttpGet]
        [AllowAnonymous] // Cho phép cả khách chưa đăng nhập cũng xem được (nếu cần)
        public async Task<JsonResult> LayTrangThaiYeuCau(int maYeuCau)
        {
            var trangThai = await _context.YeuCauNhanNuois
                .Where(y => y.MaYeuCau == maYeuCau)
                .Select(y => y.TrangThai)
                .FirstOrDefaultAsync();

            if (trangThai == null)
                return Json(new { success = false, message = "Yêu cầu không tồn tại" });

            return Json(new { success = true, trangThai });
        }
    }
}

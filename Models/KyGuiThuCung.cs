using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebPetShop.Models
{
    public partial class KyGuiThuCung
    {
        [Key]
        public int MaKyGui { get; set; }

        [Required]
        public int MaKh { get; set; }

        [Required]
        [StringLength(100)]
        public string TenThuCung { get; set; } = null!; // giữ nguyên bắt buộc như cũ

        public string? GiongLoai { get; set; }

        // TRƯỜNG MỚI – BẮT BUỘC (vì form có required)
        [Required]
        [StringLength(20)]
        public string LoaiThuCung { get; set; } = null!; // ChoNho, ChoLon, Meo, Khac

        public int? Tuoi { get; set; }

        public DateOnly? NgayGui { get; set; }
        public DateOnly? NgayHetHan { get; set; }

        public decimal? PhiKyGui { get; set; }

        public string? TrangThai { get; set; } // Đang ký gửi, Đã hủy, v.v.

        public string? GhiChu { get; set; }

        // === CÁC TRƯỜNG MỚI THÊM (KHÔNG ĐỘNG VÀO CODE CŨ) ===
        [Required]
        [StringLength(20)]
        public string HinhThucThanhToan { get; set; } = "TienMat";
        // Giá trị: "TienMat" hoặc "ChuyenKhoan" – BẮT BUỘC vì form có radio required

        public decimal TongTien { get; set; } = 0;

        public string TrangThaiThanhToan { get; set; } = "ChoXacNhan";
        // ChoXacNhan | DaThanhToan

        public string TrangThaiDon { get; set; } = "ChoXacNhan";
        // ChoXacNhan | DangChamSoc | HoanThanh

        // === NAVIGATION – GIỮ NGUYÊN 100% CŨ CỦA BẠN ===
        public virtual ICollection<ChiTietChamSoc> ChiTietChamSocs { get; set; } = new List<ChiTietChamSoc>();
        public virtual ICollection<DeXuatNhanNuoi> DeXuatNhanNuois { get; set; } = new List<DeXuatNhanNuoi>();
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
        public virtual ICollection<ThuCungNhanNuoi> ThuCungNhanNuois { get; set; } = new List<ThuCungNhanNuoi>();

        public virtual NguoiDung MaKhNavigation { get; set; } = null!;
        public DateTime? NgayThanhToan { get; set; }
    }
}
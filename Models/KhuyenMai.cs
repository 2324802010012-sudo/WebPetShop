using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebPetShop.Models
{
    public partial class KhuyenMai
    {
        public int MaKm { get; set; }

        [Required(ErrorMessage = "Mã code bắt buộc")]
        public string MaCode { get; set; } = null!;

        public string? MoTa { get; set; }

        [Range(0, 100, ErrorMessage = "% giảm từ 0 đến 100")]
        public int? PhanTramGiam { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá trị tối đa >= 0")]
        public decimal? GiaTriToiDa { get; set; }

        public DateOnly? NgayBatDau { get; set; }
        public DateOnly? NgayKetThuc { get; set; }

        public int? SoLanSuDungToiDa { get; set; }

        [Required(ErrorMessage = "Chọn trạng thái")]
        public bool TrangThai { get; set; }

        // Bắt buộc chọn sản phẩm (foreign key)
        [Required(ErrorMessage = "Chọn sản phẩm")]
        public int MaSP { get; set; }

        // Navigation property nullable để không bị lỗi validation
        [ForeignKey("MaSP")]
        public virtual SanPham? SanPham { get; set; }

        public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    }
}

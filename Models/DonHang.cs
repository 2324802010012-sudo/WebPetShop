using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class DonHang
{
    public int MaDh { get; set; }

    public int MaNguoiDung { get; set; }

    public DateTime? NgayDat { get; set; }

    public decimal? TongTien { get; set; }

    public string? TrangThai { get; set; }

    public string? MaVanDon { get; set; }

    public string? TenDonViGiao { get; set; }

    // 🧾 Thông tin người nhận hàng
    public string? HoTenNguoiNhan { get; set; }

    public string? SoDienThoai { get; set; }

    public string? DiaChiGiaoHang { get; set; }

    public string? GhiChu { get; set; }

    public int? MaKm { get; set; }

    public int? MaPhi { get; set; }

    // 🔗 Liên kết navigation
    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<LichSuTrangThaiDonHang> LichSuTrangThaiDonHangs { get; set; } = new List<LichSuTrangThaiDonHang>();

    public virtual KhuyenMai? MaKmNavigation { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual PhiGiaoHang? MaPhiNavigation { get; set; }

    public virtual ICollection<ThanhToanTrucTuyen> ThanhToanTrucTuyens { get; set; } = new List<ThanhToanTrucTuyen>();
}

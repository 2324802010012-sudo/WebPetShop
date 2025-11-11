using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

    public string? HoTenNhan { get; set; }

    public string? DiaChiGiao { get; set; }

    public string? PhuongThucThanhToan { get; set; }

    public int? MaKm { get; set; }

    public int? MaPhi { get; set; }

    public int? MaTttt { get; set; }

    public string? SoDienThoai { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<GiaoHang> GiaoHangs { get; set; } = new List<GiaoHang>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<LichSuTrangThaiDonHang> LichSuTrangThaiDonHangs { get; set; } = new List<LichSuTrangThaiDonHang>();

    public virtual KhuyenMai? MaKmNavigation { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual PhiGiaoHang? MaPhiNavigation { get; set; }

    public virtual ThanhToanTrucTuyen? MaTtttNavigation { get; set; }

    public virtual ICollection<ThanhToanTrucTuyen> ThanhToanTrucTuyens { get; set; } = new List<ThanhToanTrucTuyen>();
    public virtual ICollection<PhieuXuat> PhieuXuats { get; set; } = new List<PhieuXuat>();
    [NotMapped]
    public string? TenKhachHang => MaNguoiDungNavigation?.HoTen;

    [NotMapped]
    public string? EmailKhachHang => MaNguoiDungNavigation?.Email;
}

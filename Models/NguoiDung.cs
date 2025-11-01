using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class NguoiDung
{
    public int MaNguoiDung { get; set; }

    public string HoTen { get; set; } = null!;

    public string? GioiTinh { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string VaiTro { get; set; } = null!;

    public bool? TrangThai { get; set; }

    public virtual ICollection<BaiDangNhanNuoi> BaiDangNhanNuois { get; set; } = new List<BaiDangNhanNuoi>();

    public virtual ICollection<BaoCaoDoanhThu> BaoCaoDoanhThus { get; set; } = new List<BaoCaoDoanhThu>();

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<KhoHang> KhoHangs { get; set; } = new List<KhoHang>();

    public virtual ICollection<KyGuiThuCung> KyGuiThuCungs { get; set; } = new List<KyGuiThuCung>();

    public virtual ICollection<LichLamViec> LichLamViecs { get; set; } = new List<LichLamViec>();

    public virtual ICollection<LichSuHeThong> LichSuHeThongs { get; set; } = new List<LichSuHeThong>();

    public virtual ICollection<LichSuTrangThaiDonHang> LichSuTrangThaiDonHangs { get; set; } = new List<LichSuTrangThaiDonHang>();

    public virtual ICollection<YeuThich> YeuThiches { get; set; } = new List<YeuThich>();
}

using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class SanPham
{
    public int MaSp { get; set; }

    public string TenSp { get; set; } = null!;

    public decimal Gia { get; set; }

    public string? MoTa { get; set; }

    public string? HinhAnh { get; set; }

    public int? SoLuongTon { get; set; }

    public int MaDanhMuc { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<KhoHang> KhoHangs { get; set; } = new List<KhoHang>();

    public virtual DanhMuc MaDanhMucNavigation { get; set; } = null!;

    public virtual ICollection<YeuThich> YeuThiches { get; set; } = new List<YeuThich>();
}

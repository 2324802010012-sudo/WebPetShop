using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class ChiTietPhieuNhap
{
    public int MaCtpn { get; set; }

    public int? MaPn { get; set; }

    public int? MaSp { get; set; }

    public int? SoLuong { get; set; }

    public decimal? DonGia { get; set; }

    public virtual PhieuNhap? MaPnNavigation { get; set; }

    public virtual SanPham? MaSpNavigation { get; set; }
}

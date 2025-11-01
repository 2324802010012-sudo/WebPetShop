using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class KhuyenMai
{
    public int MaKm { get; set; }

    public string MaCode { get; set; } = null!;

    public string? MoTa { get; set; }

    public int? PhanTramGiam { get; set; }

    public decimal? GiaTriToiDa { get; set; }

    public DateOnly? NgayBatDau { get; set; }

    public DateOnly? NgayKetThuc { get; set; }

    public int? SoLanSuDungToiDa { get; set; }

    public bool? TrangThai { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}

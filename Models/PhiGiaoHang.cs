using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class PhiGiaoHang
{
    public int MaPhi { get; set; }

    public string? KhuVuc { get; set; }

    public string? DonViGiao { get; set; }

    public decimal? PhiCoDinh { get; set; }

    public decimal? PhiTheoKm { get; set; }

    public string? ThoiGianUocTinh { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}

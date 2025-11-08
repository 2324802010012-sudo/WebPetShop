using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class ThanhToanTrucTuyen
{
    public int MaTttt { get; set; }

    public int MaDh { get; set; }

    public string? MaGiaoDich { get; set; }

    public string? PhuongThuc { get; set; }

    public decimal? SoTien { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? NgayThanhToan { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual DonHang MaDhNavigation { get; set; } = null!;
}

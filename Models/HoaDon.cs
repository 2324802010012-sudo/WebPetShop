using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class HoaDon
{
    public int MaHd { get; set; }

    public int? MaDh { get; set; }

    public int? MaKyGui { get; set; }

    public int MaKeToan { get; set; }

    public DateTime? NgayLap { get; set; }

    public decimal SoTien { get; set; }

    public string? HinhThuc { get; set; }

    public string? GhiChu { get; set; }

    public virtual DonHang? MaDhNavigation { get; set; }

    public virtual NguoiDung MaKeToanNavigation { get; set; } = null!;

    public virtual KyGuiThuCung? MaKyGuiNavigation { get; set; }
}

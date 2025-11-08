using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class LichSuTrangThaiDonHang
{
    public int MaLichSuDh { get; set; }

    public int MaDh { get; set; }

    public string? TrangThaiCu { get; set; }

    public string? TrangThaiMoi { get; set; }

    public DateTime NgayCapNhat { get; set; } = DateTime.Now;

    public int? NguoiThucHien { get; set; }

    public string? GhiChu { get; set; }

    public virtual DonHang MaDhNavigation { get; set; } = null!;

    public virtual NguoiDung? NguoiThucHienNavigation { get; set; }
}

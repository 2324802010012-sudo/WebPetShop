using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class GiaoHang
{
    public int MaGiaoHang { get; set; }

    public int MaDh { get; set; }

    public int? MaNhanVienGiao { get; set; }

    public string? DonViGiao { get; set; }

    public DateTime? NgayGiao { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public virtual DonHang MaDhNavigation { get; set; } = null!;

    public virtual NguoiDung? MaNhanVienGiaoNavigation { get; set; }
}

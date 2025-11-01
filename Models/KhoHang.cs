using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class KhoHang
{
    public int MaKho { get; set; }

    public int MaSp { get; set; }

    public int? MaNcc { get; set; }

    public string? LoaiGiaoDich { get; set; }

    public int SoLuong { get; set; }

    public DateTime? NgayThucHien { get; set; }

    public int NguoiThucHien { get; set; }

    public string? GhiChu { get; set; }

    public virtual NhaCungCap? MaNccNavigation { get; set; }

    public virtual SanPham MaSpNavigation { get; set; } = null!;

    public virtual NguoiDung NguoiThucHienNavigation { get; set; } = null!;
}

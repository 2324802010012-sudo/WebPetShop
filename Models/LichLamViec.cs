using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class LichLamViec
{
    public int MaLichLam { get; set; }

    public int MaNhanVien { get; set; }

    public DateOnly NgayLam { get; set; }

    public string? CaLam { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public virtual NguoiDung MaNhanVienNavigation { get; set; } = null!;
}

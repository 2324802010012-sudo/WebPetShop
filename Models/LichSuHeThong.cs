using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class LichSuHeThong
{
    public int MaLichSu { get; set; }

    public int MaNguoiDung { get; set; }

    public string? HanhDong { get; set; }

    public DateTime? NgayThucHien { get; set; }

    public string? GhiChu { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
}

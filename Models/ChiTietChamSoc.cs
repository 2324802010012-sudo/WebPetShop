using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class ChiTietChamSoc
{
    public int MaCtcs { get; set; }

    public int MaKyGui { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public string? TinhTrang { get; set; }

    public string? GhiChu { get; set; }

    public string? HinhAnh { get; set; }

    public virtual KyGuiThuCung MaKyGuiNavigation { get; set; } = null!;
}

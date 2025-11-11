using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class ChiTietPhieuXuat
{
    public int MaCtpx { get; set; }

    public int MaPx { get; set; }

    public int MaSp { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }
 

    // 🔗 Navigation tới sản phẩm
    public virtual SanPham? MaSpNavigation { get; set; }
    public virtual PhieuXuat MaPxNavigation { get; set; } = null!;
}

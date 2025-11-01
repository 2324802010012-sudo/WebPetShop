using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class YeuThich
{
    public int MaYt { get; set; }

    public int MaNguoiDung { get; set; }

    public int MaSp { get; set; }

    public DateTime? NgayThem { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}

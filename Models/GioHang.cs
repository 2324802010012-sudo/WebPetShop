using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class GioHang
{
    public int MaGh { get; set; }

    public int MaNguoiDung { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
}

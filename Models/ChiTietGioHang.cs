using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebPetShop.Models;

public partial class ChiTietGioHang
{
    public int MaCtgh { get; set; }

    public int MaGh { get; set; }

    public int MaSp { get; set; }

    public int? SoLuong { get; set; }

    public virtual GioHang MaGhNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
    [NotMapped]
    public decimal GiaGoc { get; set; }

    [NotMapped]
    public decimal GiaSauGiam { get; set; }

}

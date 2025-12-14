using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebPetShop.Models;

public partial class GioHang
{
    public int MaGh { get; set; }

    public int MaNguoiDung { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
    [NotMapped]
    public decimal GiaGoc { get; set; }

    [NotMapped]
    public decimal GiaSauGiam { get; set; }

}

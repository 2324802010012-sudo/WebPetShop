using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class DanhGium
{
    public int MaDanhGia { get; set; }

    public int MaNguoiDung { get; set; }

    public int? MaSp { get; set; }

    public string? NoiDung { get; set; }

    public int? Diem { get; set; }

    public DateTime? NgayDanhGia { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual SanPham? MaSpNavigation { get; set; }
}

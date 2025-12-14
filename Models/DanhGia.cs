using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class DanhGia
{
    public int MaDanhGia { get; set; }

    public int MaNguoiDung { get; set; }

    public int? MaSp { get; set; }

    public string? NoiDung { get; set; }

    public int? Diem { get; set; }

    public DateTime? NgayDanhGia { get; set; }
    public virtual NguoiDung MaNguoiDungNavigation { get; set; }
    public virtual SanPham MaSpNavigation { get; set; }
    public string? HinhAnh { get; set; }

}

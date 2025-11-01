using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class BaiDangNhanNuoi
{
    public int MaBaiDang { get; set; }

    public int MaThuCungNhanNuoi { get; set; }

    public int MaNguoiTao { get; set; }

    public DateOnly? NgayDang { get; set; }

    public string? TieuDe { get; set; }

    public string? MoTa { get; set; }

    public string? HinhAnh { get; set; }

    public string? TrangThai { get; set; }

    public virtual NguoiDung MaNguoiTaoNavigation { get; set; } = null!;

    public virtual ThuCungNhanNuoi MaThuCungNhanNuoiNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class ThuCungNhanNuoi
{
    public int MaNhanNuoi { get; set; }

    public int MaKyGui { get; set; }

    public DateOnly? NgayChuyen { get; set; }

    public string? TrangThai { get; set; }
    public enum TrangThaiBaiDang
    {
        DangHienThi,
        DaCoChu,
        DaHuy
    }

    public virtual ICollection<BaiDangNhanNuoi> BaiDangNhanNuois { get; set; } = new List<BaiDangNhanNuoi>();

    public virtual KyGuiThuCung MaKyGuiNavigation { get; set; } = null!;
}

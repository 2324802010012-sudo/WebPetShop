using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class DeXuatNhanNuoi
{
    public int MaDeXuat { get; set; }

    public int MaKyGui { get; set; }

    public int MaNhanVienChamSoc { get; set; }

    public DateOnly? NgayDeXuat { get; set; }

    public string? LyDo { get; set; }

    public string? TrangThai { get; set; }

    public virtual KyGuiThuCung MaKyGuiNavigation { get; set; } = null!;

    public virtual NguoiDung MaNhanVienChamSocNavigation { get; set; } = null!;
}

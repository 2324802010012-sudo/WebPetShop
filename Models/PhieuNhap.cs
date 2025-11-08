using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class PhieuNhap
{
    public int MaPn { get; set; }

    public int? MaNcc { get; set; }

    public int? MaNhanVien { get; set; }

    public DateTime? NgayNhap { get; set; }

    public decimal? TongTien { get; set; }

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual NhaCungCap? MaNccNavigation { get; set; }

    public virtual NguoiDung? MaNhanVienNavigation { get; set; }
}

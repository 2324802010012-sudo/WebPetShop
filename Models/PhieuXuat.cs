using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class PhieuXuat
{
    public int MaPx { get; set; }

    public int? MaNhanVien { get; set; }

    public int? MaKhachHang { get; set; }

    public DateTime? NgayXuat { get; set; }

    public decimal? TongTien { get; set; }
    public int? MaDh { get; set; }
    public virtual NguoiDung? MaNhanVienNavigation { get; set; }   
    public virtual NguoiDung? MaKhachHangNavigation { get; set; }
    public virtual DonHang? MaDhNavigation { get; set; }

    public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; } = new List<ChiTietPhieuXuat>();
}

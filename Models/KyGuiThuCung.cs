using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class KyGuiThuCung
{
    public int MaKyGui { get; set; }

    public int MaKh { get; set; }

    public string TenThuCung { get; set; } = null!;

    public string? GiongLoai { get; set; }

    public int? Tuoi { get; set; }

    public DateOnly? NgayGui { get; set; }

    public DateOnly? NgayHetHan { get; set; }

    public decimal? PhiKyGui { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietChamSoc> ChiTietChamSocs { get; set; } = new List<ChiTietChamSoc>();

    public virtual ICollection<DeXuatNhanNuoi> DeXuatNhanNuois { get; set; } = new List<DeXuatNhanNuoi>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual NguoiDung MaKhNavigation { get; set; } = null!;

    public virtual ICollection<ThuCungNhanNuoi> ThuCungNhanNuois { get; set; } = new List<ThuCungNhanNuoi>();
}

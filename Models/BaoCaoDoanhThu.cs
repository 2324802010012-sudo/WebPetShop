using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class BaoCaoDoanhThu
{
    public int MaBaoCao { get; set; }

    public int? Thang { get; set; }

    public int? Nam { get; set; }

    public int? TongDonHang { get; set; }

    public decimal? TongDoanhThu { get; set; }

    public decimal? TongChiPhi { get; set; }

    public decimal? LoiNhuan { get; set; }

    public DateTime? NgayTao { get; set; }

    public int? NguoiLap { get; set; }

    public virtual NguoiDung? NguoiLapNavigation { get; set; }
}

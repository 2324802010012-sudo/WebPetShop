using System;
using System.Collections.Generic;

namespace WebPetShop.Models;

public partial class VDanhSachThuCungNhanNuoi
{
    public int MaBaiDang { get; set; }

    public string TenThuCung { get; set; } = null!;

    public string? GiongLoai { get; set; }

    public int? Tuoi { get; set; }

    public string? TieuDe { get; set; }

    public string? MoTa { get; set; }

    public string? HinhAnh { get; set; }

    public string? TrangThaiKyGui { get; set; }

    public string? TrangThaiBaiDang { get; set; }

    public DateOnly? NgayDang { get; set; }
}

using System.Collections.Generic;
using WebPetShop.Models;

namespace WebPetShop.ViewModels
{
    public class CheckoutVM
    {
        // lấy sẵn từ NguoiDung và cho phép sửa
        public string HoTenNhan { get; set; }
        public string DiaChiGiao { get; set; }
        public string SDTNhan { get; set; }
        public SanPham? SanPhamMuaNgay { get; set; }
        public int SoLuong { get; set; }

        // vận chuyển
        public int MaPhi { get; set; }
        public IEnumerable<PhiGiaoHang>? PhiGiaoHangList { get; set; }

        // khuyến mãi
        public string? MaCode { get; set; }
        public decimal GiamGia { get; set; }

        // thanh toán
        public string PhuongThuc { get; set; } = "COD";

        // tổng tiền hiển thị
        public decimal TienHang { get; set; }
        public decimal PhiVanChuyen { get; set; }
        public decimal TongTien { get; set; }
        public List<ChiTietGioHang> GioHang { get; set; } = new();

    }
}

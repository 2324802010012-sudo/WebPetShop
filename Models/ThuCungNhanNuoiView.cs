using System;

namespace WebPetShop.Models
{
    public class ThuCungNhanNuoiView
    {
        public int MaBaiDang { get; set; }
        public string TenThuCung { get; set; }
        public string GiongLoai { get; set; }
        public int? Tuoi { get; set; }
        public string TieuDe { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public string TrangThaiKyGui { get; set; }
        public string TrangThaiBaiDang { get; set; }
        public DateTime? NgayDang { get; set; }
    }
}

namespace WebPetShop.Models
{
    public class HinhThucThanhToanThucTe
    {
        public int MaHTTT { get; set; }
        public string TenHinhThuc { get; set; }
        public string? MoTa { get; set; }
        public virtual ICollection<DonHang> DonHangs { get; set; }
    = new List<DonHang>();
    }
}

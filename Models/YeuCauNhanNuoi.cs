// Models/YeuCauNhanNuoi.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebPetShop.Models
{
    [Table("YeuCauNhanNuoi")]   // ← BẮT BUỘC PHẢI CÓ DÒNG NÀY!!!
    public class YeuCauNhanNuoi
    {
        [Key]
        public int MaYeuCau { get; set; }

        public int MaBaiDang { get; set; }

        public int MaNguoiDung { get; set; }

        public DateTime NgayYeuCau { get; set; } = DateTime.Now;

        public string TrangThai { get; set; } = "Chờ xác nhận";

        // Navigation properties (rất quan trọng để Join)
        [ForeignKey("MaBaiDang")]
        public BaiDangNhanNuoi BaiDangNavigation { get; set; } = null!;

        [ForeignKey("MaNguoiDung")]
        public NguoiDung NguoiDungNavigation { get; set; } = null!;
    }
}
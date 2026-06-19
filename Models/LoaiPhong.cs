using System.ComponentModel.DataAnnotations;

namespace HotelBookingManagement.Models
{
    public class LoaiPhong
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên loại phòng")]
        public string TenLoaiPhong { get; set; }

        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [Display(Name = "Số người tối đa")]
        public int SoNguoiToiDa { get; set; }

        [Display(Name = "Giá cơ bản")]
        public decimal GiaCoBan { get; set; }

        public bool IsActive { get; set; } = true;

        public List<Phong>? Phongs { get; set; }
    }
}
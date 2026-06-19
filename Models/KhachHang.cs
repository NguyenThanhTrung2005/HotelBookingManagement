using System.ComponentModel.DataAnnotations;

namespace HotelBookingManagement.Models
{
    public class KhachHang
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [Required]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "Căn cước công dân")]
        public string CCCD { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
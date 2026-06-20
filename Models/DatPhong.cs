using System.ComponentModel.DataAnnotations;

namespace HotelBookingManagement.Models
{
    public class DatPhong
    {
        public int Id { get; set; }

        [Display(Name = "Khách hàng")]
        public int KhachHangId { get; set; }
        public KhachHang? KhachHang { get; set; }

        [Display(Name = "Phòng")]
        public int PhongId { get; set; }
        public Phong? Phong { get; set; }

        [Required]
        [Display(Name = "Ngày nhận phòng")]
        public DateTime NgayNhanPhong { get; set; }

        [Required]
        [Display(Name = "Ngày trả phòng")]
        public DateTime NgayTraPhong { get; set; }

        [Display(Name = "Số người")]
        public int SoNguoi { get; set; }

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        [Display(Name = "Trạng thái")]
        public TrangThaiDatPhong TrangThai { get; set; }
            = TrangThaiDatPhong.ChoNhanPhong;

        public DateTime CreatedAt { get; set; }
            = DateTime.Now;

        public bool IsActive { get; set; } = true;
    }

    public enum TrangThaiDatPhong
    {
        ChoNhanPhong = 1,
        DaNhanPhong = 2,
        DaTraPhong = 3,
        DaHuy = 4
    }
}
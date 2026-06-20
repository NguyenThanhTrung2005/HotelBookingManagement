using System.ComponentModel.DataAnnotations;

namespace HotelBookingManagement.Models
{
    public class Phong
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Mã phòng")]
        public string MaPhong { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tên phòng")]
        public string TenPhong { get; set; }

        [Display(Name = "Số phòng")]
        public string? SoPhong { get; set; }

        [Display(Name = "Tầng")]
        public int Tang { get; set; }

        [Range(0, 100000000)]
        [Display(Name = "Giá phòng")]
        public decimal GiaPhong { get; set; }

        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }

        [Display(Name = "Tiện nghi")]
        public string? TienNghi { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? HinhAnh { get; set; }

        [Display(Name = "Trạng thái")]
        public TrangThaiPhong TrangThai { get; set; }
            = TrangThaiPhong.Trong;

        public int LoaiPhongId { get; set; }

        public LoaiPhong? LoaiPhong { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
            = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }

    public enum TrangThaiPhong
    {
        Trong = 1,
        DaDat = 2,
        DangSuDung = 3,
        DangDonDep = 4,
        BaoTri = 5
    }
}
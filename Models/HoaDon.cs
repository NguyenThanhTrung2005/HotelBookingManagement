using System.ComponentModel.DataAnnotations;

namespace HotelBookingManagement.Models
{
    public class HoaDon
    {
        public int Id { get; set; }

        [Display(Name = "Đặt phòng")]
        public int DatPhongId { get; set; }

        public DatPhong? DatPhong { get; set; }

        [Display(Name = "Ngày lập")]
        public DateTime NgayLap { get; set; } = DateTime.Now;

        [Display(Name = "Số đêm")]
        public int SoDem { get; set; }

        [Display(Name = "Tiền phòng")]
        public decimal TienPhong { get; set; }

        [Display(Name = "Phụ thu")]
        public decimal PhuThu { get; set; }

        [Display(Name = "Tổng tiền")]
        public decimal TongTien { get; set; }

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        [Display(Name = "Trạng thái")]
        public TrangThaiHoaDon TrangThai { get; set; }
            = TrangThaiHoaDon.ChuaThanhToan;

        public bool IsActive { get; set; } = true;
    }

    public enum TrangThaiHoaDon
    {
        ChuaThanhToan = 1,
        DaThanhToan = 2
    }
}
using Microsoft.AspNetCore.Identity;

namespace HotelBookingManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? HoTen { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
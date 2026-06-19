using HotelBookingManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<LoaiPhong> LoaiPhongs { get; set; }
        public DbSet<Phong> Phongs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LoaiPhong>()
                .Property(x => x.GiaCoBan)
                .HasPrecision(18, 2);

            builder.Entity<Phong>()
                .Property(x => x.GiaPhong)
                .HasPrecision(18, 2);
        }
    }
}
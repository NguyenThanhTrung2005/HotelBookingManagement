using HotelBookingManagement.Data;
using HotelBookingManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingManagement.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HoaDonController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Danh sách hóa đơn
        public async Task<IActionResult> Index()
        {
            var data = await _context.HoaDons
                .Include(x => x.DatPhong)
                .ThenInclude(x => x.KhachHang)
                .Include(x => x.DatPhong)
                .ThenInclude(x => x.Phong)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.NgayLap)
                .ToListAsync();

            return View(data);
        }

        //Chi tiết
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var hoaDon = await _context.HoaDons
                .Include(x => x.DatPhong)
                .ThenInclude(x => x.KhachHang)
                .Include(x => x.DatPhong)
                .ThenInclude(x => x.Phong)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (hoaDon == null)
                return NotFound();

            return View(hoaDon);
        }

        //Thanh toán
        public async Task<IActionResult> ThanhToan(int id)
        {
            var hoaDon = await _context.HoaDons
                .FirstOrDefaultAsync(x => x.Id == id);

            if (hoaDon == null)
                return NotFound();

            hoaDon.TrangThai =
                TrangThaiHoaDon.DaThanhToan;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Xóa mềm
        public async Task<IActionResult> Delete(int id)
        {
            var hoaDon = await _context.HoaDons
                .FindAsync(id);

            if (hoaDon == null)
                return NotFound();

            hoaDon.IsActive = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
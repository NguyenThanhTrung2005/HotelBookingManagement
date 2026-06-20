using HotelBookingManagement.Data;
using HotelBookingManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingManagement.Controllers
{
    public class DatPhongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DatPhongController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Danh sách đặt phòng
        public async Task<IActionResult> Index()
        {
            var data = await _context.DatPhongs
                .Include(x => x.KhachHang)
                .Include(x => x.Phong)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(data);
        }

        //Chi tiết
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var datPhong = await _context.DatPhongs
                .Include(x => x.KhachHang)
                .Include(x => x.Phong)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (datPhong == null)
                return NotFound();

            return View(datPhong);
        }

        //GET Create
        public IActionResult Create()
        {
            ViewBag.KhachHangId = new SelectList(
                _context.KhachHangs.Where(x => x.IsActive),
                "Id",
                "HoTen");

            ViewBag.PhongId = new SelectList(
                _context.Phongs.Where(x =>
                    x.IsActive &&
                    x.TrangThai == TrangThaiPhong.Trong),
                "Id",
                "TenPhong");

            return View();
        }

        //POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DatPhong datPhong)
        {
            if (ModelState.IsValid)
            {
                _context.DatPhongs.Add(datPhong);

                var phong = await _context.Phongs
                    .FindAsync(datPhong.PhongId);

                if (phong != null)
                {
                    phong.TrangThai =
                        TrangThaiPhong.DaDat;
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(datPhong);
        }

        //Check In
        public async Task<IActionResult> CheckIn(int id)
        {
            var datPhong = await _context.DatPhongs
                .Include(x => x.Phong)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (datPhong == null)
                return NotFound();

            datPhong.TrangThai =
                TrangThaiDatPhong.DaNhanPhong;

            datPhong.Phong!.TrangThai =
                TrangThaiPhong.DangSuDung;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Check Out
        public async Task<IActionResult> CheckOut(int id)
        {
            var datPhong = await _context.DatPhongs
                .Include(x => x.Phong)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (datPhong == null)
                return NotFound();

            // Cập nhật trạng thái đặt phòng
            datPhong.TrangThai = TrangThaiDatPhong.DaTraPhong;

            // Cập nhật trạng thái phòng
            datPhong.Phong!.TrangThai = TrangThaiPhong.Trong;

            // Tính số đêm
            int soDem = (datPhong.NgayTraPhong - datPhong.NgayNhanPhong).Days;

            if (soDem <= 0)
                soDem = 1;

            // Tính tiền phòng
            decimal tienPhong = soDem * datPhong.Phong.GiaPhong;

            // Tạo hóa đơn
            var hoaDon = new HoaDon
            {
                DatPhongId = datPhong.Id,
                NgayLap = DateTime.Now,
                SoDem = soDem,
                TienPhong = tienPhong,
                PhuThu = 0,
                TongTien = tienPhong,
                TrangThai = TrangThaiHoaDon.ChuaThanhToan,
                IsActive = true
            };

            _context.HoaDons.Add(hoaDon);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Hủy đặt phòng
        public async Task<IActionResult> Cancel(int id)
        {
            var datPhong = await _context.DatPhongs
                .Include(x => x.Phong)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (datPhong == null)
                return NotFound();

            datPhong.TrangThai =
                TrangThaiDatPhong.DaHuy;

            datPhong.Phong!.TrangThai =
                TrangThaiPhong.Trong;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
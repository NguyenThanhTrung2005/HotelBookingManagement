using HotelBookingManagement.Data;
using HotelBookingManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingManagement.Controllers
{
    public class LoaiPhongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoaiPhongController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LoaiPhong
        public async Task<IActionResult> Index(string searchString)
        {
            var query = _context.LoaiPhongs
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x =>
                    x.TenLoaiPhong.Contains(searchString));
            }

            var loaiPhongs = await query
                .OrderBy(x => x.TenLoaiPhong)
                .ToListAsync();

            ViewBag.SearchString = searchString;

            return View(loaiPhongs);
        }

        // GET: LoaiPhong/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var loaiPhong = await _context.LoaiPhongs
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.IsActive);

            if (loaiPhong == null)
                return NotFound();

            return View(loaiPhong);
        }

        // GET: LoaiPhong/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoaiPhong/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoaiPhong loaiPhong)
        {
            if (!ModelState.IsValid)
                return View(loaiPhong);

            bool exists = await _context.LoaiPhongs.AnyAsync(x =>
                x.TenLoaiPhong == loaiPhong.TenLoaiPhong &&
                x.IsActive);

            if (exists)
            {
                ModelState.AddModelError(
                    "TenLoaiPhong",
                    "Tên loại phòng đã tồn tại.");

                return View(loaiPhong);
            }

            loaiPhong.IsActive = true;

            _context.Add(loaiPhong);
            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Thêm loại phòng thành công.";

            return RedirectToAction(nameof(Index));
        }

        // GET: LoaiPhong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var loaiPhong = await _context.LoaiPhongs
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.IsActive);

            if (loaiPhong == null)
                return NotFound();

            return View(loaiPhong);
        }

        // POST: LoaiPhong/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LoaiPhong loaiPhong)
        {
            if (id != loaiPhong.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(loaiPhong);

            bool exists = await _context.LoaiPhongs.AnyAsync(x =>
                x.TenLoaiPhong == loaiPhong.TenLoaiPhong &&
                x.Id != loaiPhong.Id &&
                x.IsActive);

            if (exists)
            {
                ModelState.AddModelError(
                    "TenLoaiPhong",
                    "Tên loại phòng đã tồn tại.");

                return View(loaiPhong);
            }

            try
            {
                _context.Update(loaiPhong);
                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Cập nhật loại phòng thành công.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiPhongExists(loaiPhong.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LoaiPhong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var loaiPhong = await _context.LoaiPhongs
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.IsActive);

            if (loaiPhong == null)
                return NotFound();

            return View(loaiPhong);
        }

        // POST: LoaiPhong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiPhong = await _context.LoaiPhongs
                .Include(x => x.Phongs)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (loaiPhong == null)
                return RedirectToAction(nameof(Index));

            bool hasRooms = await _context.Phongs.AnyAsync(x =>
                x.LoaiPhongId == id &&
                x.IsActive);

            if (hasRooms)
            {
                TempData["Error"] =
                    "Không thể xóa vì loại phòng đang được sử dụng.";

                return RedirectToAction(nameof(Index));
            }

            loaiPhong.IsActive = false;

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Xóa loại phòng thành công.";

            return RedirectToAction(nameof(Index));
        }

        private bool LoaiPhongExists(int id)
        {
            return _context.LoaiPhongs
                .Any(e => e.Id == id);
        }
    }
}
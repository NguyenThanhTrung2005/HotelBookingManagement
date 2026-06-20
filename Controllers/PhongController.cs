using HotelBookingManagement.Data;
using HotelBookingManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingManagement.Controllers
{
    public class PhongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhongController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Phong
        public async Task<IActionResult> Index(string searchString)
        {
            var query = _context.Phongs
                .Include(p => p.LoaiPhong)
                .Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(p =>
                    p.MaPhong.Contains(searchString) ||
                    p.TenPhong.Contains(searchString));
            }

            ViewBag.SearchString = searchString;

            return View(await query.ToListAsync());
        }

        // GET: Phong/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var phong = await _context.Phongs
                .Include(p => p.LoaiPhong)
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);

            if (phong == null)
                return NotFound();

            return View(phong);
        }

        // GET: Phong/Create
        public IActionResult Create()
        {
            LoadLoaiPhong();
            return View();
        }

        // POST: Phong/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Phong phong)
        {
            if (!ModelState.IsValid)
            {
                LoadLoaiPhong(phong.LoaiPhongId);
                return View(phong);
            }

            bool exists = await _context.Phongs.AnyAsync(x =>
                x.MaPhong == phong.MaPhong &&
                x.IsActive);

            if (exists)
            {
                ModelState.AddModelError("MaPhong",
                    "Mã phòng đã tồn tại.");

                LoadLoaiPhong(phong.LoaiPhongId);
                return View(phong);
            }

            phong.IsActive = true;

            _context.Add(phong);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thêm phòng thành công.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Phong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var phong = await _context.Phongs.FindAsync(id);

            if (phong == null || !phong.IsActive)
                return NotFound();

            LoadLoaiPhong(phong.LoaiPhongId);

            return View(phong);
        }

        // POST: Phong/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Phong phong)
        {
            if (id != phong.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                LoadLoaiPhong(phong.LoaiPhongId);
                return View(phong);
            }

            bool exists = await _context.Phongs.AnyAsync(x =>
                x.MaPhong == phong.MaPhong &&
                x.Id != phong.Id &&
                x.IsActive);

            if (exists)
            {
                ModelState.AddModelError("MaPhong",
                    "Mã phòng đã tồn tại.");

                LoadLoaiPhong(phong.LoaiPhongId);
                return View(phong);
            }

            try
            {
                _context.Update(phong);
                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Cập nhật phòng thành công.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhongExists(phong.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Phong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var phong = await _context.Phongs
                .Include(p => p.LoaiPhong)
                .FirstOrDefaultAsync(m =>
                    m.Id == id &&
                    m.IsActive);

            if (phong == null)
                return NotFound();

            return View(phong);
        }

        // POST: Phong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phong = await _context.Phongs.FindAsync(id);

            if (phong != null)
            {
                phong.IsActive = false;

                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Xóa phòng thành công.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PhongExists(int id)
        {
            return _context.Phongs.Any(e => e.Id == id);
        }

        private void LoadLoaiPhong(object? selectedValue = null)
        {
            ViewBag.LoaiPhongId = new SelectList(
                _context.LoaiPhongs
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.TenLoaiPhong),
                "Id",
                "TenLoaiPhong",
                selectedValue);
        }
    }
}
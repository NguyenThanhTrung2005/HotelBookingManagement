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
        public async Task<IActionResult> Index()
        {
            var loaiPhongs = await _context.LoaiPhongs
                .Where(x => x.IsActive)
                .ToListAsync();

            return View(loaiPhongs);
        }

        // GET: LoaiPhong/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiPhong = await _context.LoaiPhongs
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            if (loaiPhong == null)
            {
                return NotFound();
            }

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
            if (ModelState.IsValid)
            {
                loaiPhong.IsActive = true;

                _context.LoaiPhongs.Add(loaiPhong);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(loaiPhong);
        }

        // GET: LoaiPhong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiPhong = await _context.LoaiPhongs.FindAsync(id);

            if (loaiPhong == null || !loaiPhong.IsActive)
            {
                return NotFound();
            }

            return View(loaiPhong);
        }

        // POST: LoaiPhong/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LoaiPhong loaiPhong)
        {
            if (id != loaiPhong.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiPhong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiPhongExists(loaiPhong.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(loaiPhong);
        }

        // GET: LoaiPhong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiPhong = await _context.LoaiPhongs
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            if (loaiPhong == null)
            {
                return NotFound();
            }

            return View(loaiPhong);
        }

        // POST: LoaiPhong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiPhong = await _context.LoaiPhongs.FindAsync(id);

            if (loaiPhong != null)
            {
                loaiPhong.IsActive = false;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LoaiPhongExists(int id)
        {
            return _context.LoaiPhongs.Any(e => e.Id == id);
        }
    }
}
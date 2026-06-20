using HotelBookingManagement.Data;
using HotelBookingManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingManagement.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KhachHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KhachHang
        public async Task<IActionResult> Index(string searchString)
        {
            var query = _context.KhachHangs
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x =>
                    x.HoTen.Contains(searchString) ||
                    x.SoDienThoai.Contains(searchString) ||
                    x.CCCD.Contains(searchString));
            }

            ViewBag.SearchString = searchString;

            return View(await query
                .OrderBy(x => x.HoTen)
                .ToListAsync());
        }

        // GET: KhachHang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.IsActive);

            if (khachHang == null)
                return NotFound();

            return View(khachHang);
        }

        // GET: KhachHang/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KhachHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhachHang khachHang)
        {
            if (!ModelState.IsValid)
                return View(khachHang);

            bool phoneExists = await _context.KhachHangs
                .AnyAsync(x =>
                    x.SoDienThoai == khachHang.SoDienThoai &&
                    x.IsActive);

            if (phoneExists)
            {
                ModelState.AddModelError(
                    "SoDienThoai",
                    "Số điện thoại đã tồn tại.");

                return View(khachHang);
            }

            bool cccdExists = await _context.KhachHangs
                .AnyAsync(x =>
                    x.CCCD == khachHang.CCCD &&
                    x.IsActive);

            if (cccdExists)
            {
                ModelState.AddModelError(
                    "CCCD",
                    "CCCD đã tồn tại.");

                return View(khachHang);
            }

            khachHang.CreatedAt = DateTime.Now;
            khachHang.IsActive = true;

            _context.Add(khachHang);
            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Thêm khách hàng thành công.";

            return RedirectToAction(nameof(Index));
        }

        // GET: KhachHang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.IsActive);

            if (khachHang == null)
                return NotFound();

            return View(khachHang);
        }

        // POST: KhachHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KhachHang khachHang)
        {
            if (id != khachHang.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(khachHang);

            bool phoneExists = await _context.KhachHangs
                .AnyAsync(x =>
                    x.SoDienThoai == khachHang.SoDienThoai &&
                    x.Id != khachHang.Id &&
                    x.IsActive);

            if (phoneExists)
            {
                ModelState.AddModelError(
                    "SoDienThoai",
                    "Số điện thoại đã tồn tại.");

                return View(khachHang);
            }

            bool cccdExists = await _context.KhachHangs
                .AnyAsync(x =>
                    x.CCCD == khachHang.CCCD &&
                    x.Id != khachHang.Id &&
                    x.IsActive);

            if (cccdExists)
            {
                ModelState.AddModelError(
                    "CCCD",
                    "CCCD đã tồn tại.");

                return View(khachHang);
            }

            try
            {
                khachHang.UpdatedAt = DateTime.Now;

                _context.Update(khachHang);

                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Cập nhật khách hàng thành công.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KhachHangExists(khachHang.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: KhachHang/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.IsActive);

            if (khachHang == null)
                return NotFound();

            return View(khachHang);
        }

        // POST: KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khachHang = await _context.KhachHangs
                .FindAsync(id);

            if (khachHang != null)
            {
                khachHang.IsActive = false;

                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Xóa khách hàng thành công.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
            return _context.KhachHangs
                .Any(e => e.Id == id);
        }
    }
}
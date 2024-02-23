using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinemas.Models;

namespace Cinemas.Controllers
{
    public class ThanhToanController : Controller
    {
        private readonly DbCinemasContext _context;

        public ThanhToanController(DbCinemasContext context)
        {
            _context = context;
        }

        // GET: ThanhToan
        public async Task<IActionResult> Index()
        {
            var dbCinemasContext = _context.HoaDons.Include(h => h.IdkhNavigation).Include(h => h.IdlichChieuNavigation);
            return View(await dbCinemasContext.ToListAsync());
        }

        // GET: ThanhToan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HoaDons == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.IdkhNavigation)
                .Include(h => h.IdlichChieuNavigation)
                .FirstOrDefaultAsync(m => m.IdhoaDon == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // GET: ThanhToan/Create
        public IActionResult Create()
        {
            ViewData["Idkh"] = new SelectList(_context.KhachHangs, "Idkh", "Idkh");
            ViewData["IdlichChieu"] = new SelectList(_context.LichChieuPhims, "IdlichChieu", "IdlichChieu");
            ViewData["Idphong"] = new SelectList(_context.Phongs, "Idphong", "Idphong");
            return View();
        }

        // POST: ThanhToan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdhoaDon,Idkh,IdlichChieu,Idphong,NgayMuaVe,SoLuongGhe,TenGhe,TongTien")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idkh"] = new SelectList(_context.KhachHangs, "Idkh", "Idkh", hoaDon.Idkh);
            ViewData["IdlichChieu"] = new SelectList(_context.LichChieuPhims, "IdlichChieu", "IdlichChieu", hoaDon.IdlichChieu);
            return View(hoaDon);
        }

        // GET: ThanhToan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HoaDons == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            ViewData["Idkh"] = new SelectList(_context.KhachHangs, "Idkh", "Idkh", hoaDon.Idkh);
            ViewData["IdlichChieu"] = new SelectList(_context.LichChieuPhims, "IdlichChieu", "IdlichChieu", hoaDon.IdlichChieu);
            return View(hoaDon);
        }

        // POST: ThanhToan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdhoaDon,Idkh,IdlichChieu,Idphong,NgayMuaVe,SoLuongGhe,TenGhe,TongTien")] HoaDon hoaDon)
        {
            if (id != hoaDon.IdhoaDon)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonExists(hoaDon.IdhoaDon))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idkh"] = new SelectList(_context.KhachHangs, "Idkh", "Idkh", hoaDon.Idkh);
            ViewData["IdlichChieu"] = new SelectList(_context.LichChieuPhims, "IdlichChieu", "IdlichChieu", hoaDon.IdlichChieu);
            return View(hoaDon);
        }

        // GET: ThanhToan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HoaDons == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.IdkhNavigation)
                .Include(h => h.IdlichChieuNavigation)
                .FirstOrDefaultAsync(m => m.IdhoaDon == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // POST: ThanhToan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HoaDons == null)
            {
                return Problem("Entity set 'DbCinemasContext.HoaDons'  is null.");
            }
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon != null)
            {
                _context.HoaDons.Remove(hoaDon);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonExists(int id)
        {
          return (_context.HoaDons?.Any(e => e.IdhoaDon == id)).GetValueOrDefault();
        }
    }
}

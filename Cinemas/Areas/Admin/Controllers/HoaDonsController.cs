using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinemas.Models;
using PagedList.Core;

namespace Cinemas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HoaDonsController : Controller
    {
        private readonly DbCinemasContext _context;

        public HoaDonsController(DbCinemasContext context)
        {
            _context = context;
        }

        // GET: Admin/HoaDons
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsHoaDon = _context.HoaDons
                .Include(h => h.IdkhNavigation)
                .Include(h => h.IdlichChieuNavigation)
                .Include(h => h.IdlichChieuNavigation.IdphongNavigation)
                .Include(h => h.IdlichChieuNavigation.IdphimNavigation)
                .Include(h => h.IdlichChieuNavigation.IdgioChieuNavigation)
                .OrderByDescending(x=>x.IdhoaDon);

            PagedList<HoaDon> models = new PagedList<HoaDon>(lsHoaDon.AsQueryable(), pageNumber, pageSize);

            // Lấy ngày hiện tại
            DateTime currentDate = DateTime.Now;

            List<SelectListItem> listNgay = new List<SelectListItem>();

            // Lấy 10 ngày gần nhất bằng cách lặp qua từ 0 đến 9 và trừ ngày tương ứng
            for (int i = 0; i < 10; i++)
            {
                DateTime previousDate = currentDate.AddDays(-i);
                listNgay.Add(new SelectListItem() { Text = previousDate.ToShortDateString(), Value = previousDate.ToShortDateString() });
            }

            ViewData["NgayChieu"] = new SelectList(listNgay, "Text", "Value");

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/HoaDons/Details/5
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

        // GET: Admin/HoaDons/Create
        public IActionResult Create()
        {
            ViewData["Idkh"] = new SelectList(_context.KhachHangs, "Idkh", "Email");
            ViewData["IdlichChieu"] = new SelectList(_context.LichChieuPhims, "IdlichChieu", "IdlichChieu");
            return View();
        }

        // POST: Admin/HoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdhoaDon,Idkh,IdlichChieu,NgayMuaVe,SoLuongGhe,TenGhe,TongTien")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idkh"] = new SelectList(_context.KhachHangs, "Idkh", "Email", hoaDon.Idkh);
            ViewData["IdlichChieu"] = new SelectList(_context.LichChieuPhims, "IdlichChieu", "IdlichChieu", hoaDon.IdlichChieu);
            return View(hoaDon);
        }

        // GET: Admin/HoaDons/Edit/5
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
            ViewData["Idkh"] = new SelectList(_context.KhachHangs, "Idkh", "Email", hoaDon.Idkh);
            ViewData["IdlichChieu"] = new SelectList(_context.LichChieuPhims, "IdlichChieu", "IdlichChieu", hoaDon.IdlichChieu);
            return View(hoaDon);
        }

        // POST: Admin/HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdhoaDon,Idkh,IdlichChieu,NgayMuaVe,SoLuongGhe,TenGhe,TongTien")] HoaDon hoaDon)
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
            ViewData["Idkh"] = new SelectList(_context.KhachHangs, "Idkh", "Email", hoaDon.Idkh);
            ViewData["IdlichChieu"] = new SelectList(_context.LichChieuPhims, "IdlichChieu", "IdlichChieu", hoaDon.IdlichChieu);
            return View(hoaDon);
        }

        // GET: Admin/HoaDons/Delete/5
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

        // POST: Admin/HoaDons/Delete/5
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

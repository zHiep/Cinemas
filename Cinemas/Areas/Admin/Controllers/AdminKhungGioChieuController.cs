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
    public class AdminKhungGioChieuController : Controller
    {
        private readonly DbCinemasContext _context;

        public AdminKhungGioChieuController(DbCinemasContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminKhungGioChieu
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsGioChieu = _context.GioChieus
                .AsNoTracking()
                .OrderByDescending(x => x.IdgioChieu);

            PagedList<GioChieu> models = new PagedList<GioChieu>(lsGioChieu, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminKhungGioChieu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GioChieus == null)
            {
                return NotFound();
            }

            var gioChieu = await _context.GioChieus
                .FirstOrDefaultAsync(m => m.IdgioChieu == id);
            if (gioChieu == null)
            {
                return NotFound();
            }

            return View(gioChieu);
        }

        // GET: Admin/AdminKhungGioChieu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminKhungGioChieu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdgioChieu,GioChieu1")] GioChieu gioChieu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gioChieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gioChieu);
        }

        // GET: Admin/AdminKhungGioChieu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GioChieus == null)
            {
                return NotFound();
            }

            var gioChieu = await _context.GioChieus.FindAsync(id);
            if (gioChieu == null)
            {
                return NotFound();
            }
            return View(gioChieu);
        }

        // POST: Admin/AdminKhungGioChieu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdgioChieu,GioChieu1")] GioChieu gioChieu)
        {
            if (id != gioChieu.IdgioChieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gioChieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GioChieuExists(gioChieu.IdgioChieu))
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
            return View(gioChieu);
        }

        // GET: Admin/AdminKhungGioChieu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GioChieus == null)
            {
                return NotFound();
            }

            var gioChieu = await _context.GioChieus
                .FirstOrDefaultAsync(m => m.IdgioChieu == id);
            if (gioChieu == null)
            {
                return NotFound();
            }

            return View(gioChieu);
        }

        // POST: Admin/AdminKhungGioChieu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GioChieus == null)
            {
                return Problem("Entity set 'DbCinemasContext.GioChieus'  is null.");
            }
            var gioChieu = await _context.GioChieus.FindAsync(id);
            if (gioChieu != null)
            {
                _context.GioChieus.Remove(gioChieu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GioChieuExists(int id)
        {
            return (_context.GioChieus?.Any(e => e.IdgioChieu == id)).GetValueOrDefault();
        }
    }
}

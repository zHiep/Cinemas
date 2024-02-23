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
    public class AdminChiTietPhongController : Controller
    {
        private readonly DbCinemasContext _context;

        public AdminChiTietPhongController(DbCinemasContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminChiTietPhong
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsChiTietPhong = _context.GhePhongs
                .AsNoTracking()
                .Include(x => x.IdphongNavigation)
                .Include(x => x.IdgheNavigation)
                .OrderByDescending(x => x.Id);

            PagedList<GhePhong> models = new PagedList<GhePhong>(lsChiTietPhong, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminChiTietPhong/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GhePhongs == null)
            {
                return NotFound();
            }

            var ghePhong = await _context.GhePhongs
                .Include(g => g.IdgheNavigation)
                .Include(g => g.IdphongNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ghePhong == null)
            {
                return NotFound();
            }

            return View(ghePhong);
        }

        // GET: Admin/AdminChiTietPhong/Create
        public IActionResult Create()
        {
            ViewData["Idghe"] = new SelectList(_context.Ghes, "Idghe", "Idghe");
            ViewData["Idphong"] = new SelectList(_context.Phongs, "Idphong", "Idphong");
            return View();
        }

        // POST: Admin/AdminChiTietPhong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Idphong,Idghe")] GhePhong ghePhong)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ghePhong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idghe"] = new SelectList(_context.Ghes, "Idghe", "Idghe", ghePhong.Idghe);
            ViewData["Idphong"] = new SelectList(_context.Phongs, "Idphong", "Idphong", ghePhong.Idphong);
            return View(ghePhong);
        }

        // GET: Admin/AdminChiTietPhong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GhePhongs == null)
            {
                return NotFound();
            }

            var ghePhong = await _context.GhePhongs.FindAsync(id);
            if (ghePhong == null)
            {
                return NotFound();
            }
            ViewData["Idghe"] = new SelectList(_context.Ghes, "Idghe", "Idghe", ghePhong.Idghe);
            ViewData["Idphong"] = new SelectList(_context.Phongs, "Idphong", "Idphong", ghePhong.Idphong);
            return View(ghePhong);
        }

        // POST: Admin/AdminChiTietPhong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Idphong,Idghe")] GhePhong ghePhong)
        {
            if (id != ghePhong.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ghePhong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GhePhongExists(ghePhong.Id))
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
            ViewData["Idghe"] = new SelectList(_context.Ghes, "Idghe", "Idghe", ghePhong.Idghe);
            ViewData["Idphong"] = new SelectList(_context.Phongs, "Idphong", "Idphong", ghePhong.Idphong);
            return View(ghePhong);
        }

        // GET: Admin/AdminChiTietPhong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GhePhongs == null)
            {
                return NotFound();
            }

            var ghePhong = await _context.GhePhongs
                .Include(g => g.IdgheNavigation)
                .Include(g => g.IdphongNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ghePhong == null)
            {
                return NotFound();
            }

            return View(ghePhong);
        }

        // POST: Admin/AdminChiTietPhong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GhePhongs == null)
            {
                return Problem("Entity set 'DbCinemasContext.GhePhongs'  is null.");
            }
            var ghePhong = await _context.GhePhongs.FindAsync(id);
            if (ghePhong != null)
            {
                _context.GhePhongs.Remove(ghePhong);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GhePhongExists(int id)
        {
          return (_context.GhePhongs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

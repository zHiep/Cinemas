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
    public class AdminPhongController : Controller
    {
        private readonly DbCinemasContext _context;

        public AdminPhongController(DbCinemasContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminPhong
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsPhong = _context.Phongs
                .AsNoTracking()
                .OrderByDescending(x => x.Idphong);

            PagedList<Phong> models = new PagedList<Phong>(lsPhong, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminPhong/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Phongs == null)
            {
                return NotFound();
            }

            var phong = await _context.Phongs
                .FirstOrDefaultAsync(m => m.Idphong == id);
            if (phong == null)
            {
                return NotFound();
            }

            return View(phong);
        }

        // GET: Admin/AdminPhong/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminPhong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idphong,TenPhong,SoGhe,SoGheTrong")] Phong phong)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phong);
        }

        // GET: Admin/AdminPhong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Phongs == null)
            {
                return NotFound();
            }

            var phong = await _context.Phongs.FindAsync(id);
            if (phong == null)
            {
                return NotFound();
            }
            return View(phong);
        }

        // POST: Admin/AdminPhong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idphong,TenPhong,SoGhe,SoGheTrong")] Phong phong)
        {
            if (id != phong.Idphong)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhongExists(phong.Idphong))
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
            return View(phong);
        }

        // GET: Admin/AdminPhong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Phongs == null)
            {
                return NotFound();
            }

            var phong = await _context.Phongs
                .FirstOrDefaultAsync(m => m.Idphong == id);
            if (phong == null)
            {
                return NotFound();
            }

            return View(phong);
        }

        // POST: Admin/AdminPhong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Phongs == null)
            {
                return Problem("Entity set 'DbCinemasContext.Phongs'  is null.");
            }
            var phong = await _context.Phongs.FindAsync(id);
            if (phong != null)
            {
                _context.Phongs.Remove(phong);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhongExists(int id)
        {
          return (_context.Phongs?.Any(e => e.Idphong == id)).GetValueOrDefault();
        }
    }
}

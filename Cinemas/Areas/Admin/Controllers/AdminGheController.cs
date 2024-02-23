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
    public class AdminGheController : Controller
    {
        private readonly DbCinemasContext _context;

        public AdminGheController(DbCinemasContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminGhe
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsGhe = _context.Ghes
                .AsNoTracking()
                .OrderByDescending(x => x.Idghe);

            PagedList<Ghe> models = new PagedList<Ghe>(lsGhe, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminGhe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ghes == null)
            {
                return NotFound();
            }

            var ghe = await _context.Ghes
                .FirstOrDefaultAsync(m => m.Idghe == id);
            if (ghe == null)
            {
                return NotFound();
            }

            return View(ghe);
        }

        // GET: Admin/AdminGhe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminGhe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idghe,TenGhe")] Ghe ghe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ghe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ghe);
        }

        // GET: Admin/AdminGhe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ghes == null)
            {
                return NotFound();
            }

            var ghe = await _context.Ghes.FindAsync(id);
            if (ghe == null)
            {
                return NotFound();
            }
            return View(ghe);
        }

        // POST: Admin/AdminGhe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idghe,TenGhe")] Ghe ghe)
        {
            if (id != ghe.Idghe)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ghe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GheExists(ghe.Idghe))
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
            return View(ghe);
        }

        // GET: Admin/AdminGhe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ghes == null)
            {
                return NotFound();
            }

            var ghe = await _context.Ghes
                .FirstOrDefaultAsync(m => m.Idghe == id);
            if (ghe == null)
            {
                return NotFound();
            }

            return View(ghe);
        }

        // POST: Admin/AdminGhe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ghes == null)
            {
                return Problem("Entity set 'DbCinemasContext.Ghes'  is null.");
            }
            var ghe = await _context.Ghes.FindAsync(id);
            if (ghe != null)
            {
                _context.Ghes.Remove(ghe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GheExists(int id)
        {
          return (_context.Ghes?.Any(e => e.Idghe == id)).GetValueOrDefault();
        }
    }
}

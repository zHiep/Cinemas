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
    public class AdminTheLoaiPhimController : Controller
    {
        private readonly DbCinemasContext _context;

        public AdminTheLoaiPhimController(DbCinemasContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminTheLoaiPhim
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsTheLoai = _context.TheLoaiPhims
                .AsNoTracking()
                .OrderByDescending(x => x.IdtheLoai);

            PagedList<TheLoaiPhim> models = new PagedList<TheLoaiPhim>(lsTheLoai, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminTheLoaiPhim/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TheLoaiPhims == null)
            {
                return NotFound();
            }

            var theLoaiPhim = await _context.TheLoaiPhims
                .FirstOrDefaultAsync(m => m.IdtheLoai == id);
            if (theLoaiPhim == null)
            {
                return NotFound();
            }

            return View(theLoaiPhim);
        }

        // GET: Admin/AdminTheLoaiPhim/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminTheLoaiPhim/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdtheLoai,TenTheLoai")] TheLoaiPhim theLoaiPhim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(theLoaiPhim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(theLoaiPhim);
        }

        // GET: Admin/AdminTheLoaiPhim/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TheLoaiPhims == null)
            {
                return NotFound();
            }

            var theLoaiPhim = await _context.TheLoaiPhims.FindAsync(id);
            if (theLoaiPhim == null)
            {
                return NotFound();
            }
            return View(theLoaiPhim);
        }

        // POST: Admin/AdminTheLoaiPhim/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdtheLoai,TenTheLoai")] TheLoaiPhim theLoaiPhim)
        {
            if (id != theLoaiPhim.IdtheLoai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(theLoaiPhim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheLoaiPhimExists(theLoaiPhim.IdtheLoai))
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
            return View(theLoaiPhim);
        }

        // GET: Admin/AdminTheLoaiPhim/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TheLoaiPhims == null)
            {
                return NotFound();
            }

            var theLoaiPhim = await _context.TheLoaiPhims
                .FirstOrDefaultAsync(m => m.IdtheLoai == id);
            if (theLoaiPhim == null)
            {
                return NotFound();
            }

            return View(theLoaiPhim);
        }

        // POST: Admin/AdminTheLoaiPhim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TheLoaiPhims == null)
            {
                return Problem("Entity set 'DbCinemasContext.TheLoaiPhims'  is null.");
            }
            var theLoaiPhim = await _context.TheLoaiPhims.FindAsync(id);
            if (theLoaiPhim != null)
            {
                _context.TheLoaiPhims.Remove(theLoaiPhim);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TheLoaiPhimExists(int id)
        {
          return (_context.TheLoaiPhims?.Any(e => e.IdtheLoai == id)).GetValueOrDefault();
        }
    }
}

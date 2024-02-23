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
    public class AdminKhachHangsController : Controller
    {
        private readonly DbCinemasContext _context;

        public AdminKhachHangsController(DbCinemasContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminKhachHangs
        public IActionResult Index(int? page)
        {   
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsKhachHang = _context.KhachHangs
                .AsNoTracking()
                .Include(x=>x.Role)
                .OrderByDescending(x => x.Idkh);

            PagedList<KhachHang> models = new PagedList<KhachHang>(lsKhachHang, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminKhachHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KhachHangs == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.Role)
                .FirstOrDefaultAsync(m => m.Idkh == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: Admin/AdminKhachHangs/Create
        public IActionResult Create()
        {
            ViewData["Idaccount"] = new SelectList(_context.Accounts, "Idaccount", "Idaccount");
            return View();
        }

        // POST: Admin/AdminKhachHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idkh,Idaccount,TenKh,Email,Sdt,NgaySinh,GioiTinh,DiaChi")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: Admin/AdminKhachHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KhachHangs == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        // POST: Admin/AdminKhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idkh,Idaccount,TenKh,Email,Sdt,NgaySinh,GioiTinh,DiaChi")] KhachHang khachHang)
        {
            if (id != khachHang.Idkh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.Idkh))
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
            return View(khachHang);
        }

        // GET: Admin/AdminKhachHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KhachHangs == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.Idkh == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // POST: Admin/AdminKhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KhachHangs == null)
            {
                return Problem("Entity set 'DbCinemasContext.KhachHangs'  is null.");
            }
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                _context.KhachHangs.Remove(khachHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
          return (_context.KhachHangs?.Any(e => e.Idkh == id)).GetValueOrDefault();
        }
    }
}

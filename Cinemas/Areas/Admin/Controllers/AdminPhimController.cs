using System;
/*using System.Web.Mvc;*/
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinemas.Models;
using PagedList.Core;
using Cinemas.Helpper;

namespace Cinemas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminPhimController : Controller
    {
        private readonly DbCinemasContext _context;

        public AdminPhimController(DbCinemasContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminPhim
        public IActionResult Index(int page = 1, int IdTheLoai = 0, int IdTinhTrang = 0)
        {
            var pageNumber = page;
            var pageSize = 20;

            List<Phim> lsPhim = new List<Phim>();
            if (IdTheLoai!=0 && IdTinhTrang != 0)
            {
                lsPhim = _context.Phims
                .AsNoTracking()
                .Where(x => x.IdtheLoai == IdTheLoai)
                .Where(x => x.IdtinhTrangPhim == IdTinhTrang)
                .Include(x => x.IdtheLoaiNavigation)
                .Include(x => x.IdtinhTrangPhimNavigation)
                .OrderByDescending(x => x.Idphim).ToList();
            } else if (IdTheLoai==0 && IdTinhTrang != 0) 
            {
                lsPhim = _context.Phims
                .AsNoTracking()
                .Where(x => x.IdtinhTrangPhim == IdTinhTrang)
                .Include(x => x.IdtheLoaiNavigation)
                .Include(x => x.IdtinhTrangPhimNavigation)
                .OrderByDescending(x => x.Idphim).ToList();
            }
            else if (IdTheLoai!=0 && IdTinhTrang == 0)
            {
                lsPhim = _context.Phims
                .AsNoTracking()
                .Where(x => x.IdtheLoai == IdTheLoai)
                .Include(x => x.IdtheLoaiNavigation)
                .Include(x => x.IdtinhTrangPhimNavigation)
                .OrderByDescending(x => x.Idphim).ToList();
            }
            else
            {
                lsPhim = _context.Phims
                .AsNoTracking()
                .Include(x => x.IdtheLoaiNavigation)
                .Include(x => x.IdtinhTrangPhimNavigation)
                .OrderByDescending(x => x.Idphim).ToList();
            }

            PagedList<Phim> models = new PagedList<Phim>(lsPhim.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrenCateID = IdTheLoai;
            ViewBag.CurrentPage = pageNumber; 

            ViewData["TenTheLoai"] = new SelectList(_context.TheLoaiPhims, "IdtheLoai", "TenTheLoai", IdTheLoai);
            ViewData["TenTinhTrang"] = new SelectList(_context.TinhTrangPhims, "IdtinhTrangPhim", "TenTinhTrang", IdTinhTrang);
            return View(models);
        }

        public IActionResult Filter(int IdTheLoai = 0, int IdTinhTrang = 0)
        {

            var url = $"/Admin/AdminPhim?IdTheLoai={IdTheLoai}&IdTinhTrang={IdTinhTrang}";
            if(IdTheLoai == 0 && IdTinhTrang==0)
            {
                url = $"/Admin/AdminPhim";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

            // GET: Admin/AdminPhim/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Phims == null)
            {
                return NotFound();
            }

            var phim = await _context.Phims
                .Include(p => p.IdtheLoaiNavigation)
                .FirstOrDefaultAsync(m => m.Idphim == id);
            if (phim == null)
            {
                return NotFound();
            }

            return View(phim);
        }

        // GET: Admin/AdminPhim/Create
        public IActionResult Create()
        {
            ViewData["TenTheLoai"] = new SelectList(_context.TheLoaiPhims, "IdtheLoai", "TenTheLoai");
            ViewData["TenTinhTrang"] = new SelectList(_context.TinhTrangPhims, "IdtinhTrangPhim", "TenTinhTrang");
            return View();
        }

        // POST: Admin/AdminPhim/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idphim,IdtheLoai,IdtinhTrangPhim,Img,TenPhim,ThoiLuong,Gia,NgayKhoiChieu,DaoDien,DienVien,MoTa")] Phim phim, IFormFile fImg)
        {
            ViewData["TenTheLoai"] = new SelectList(_context.TheLoaiPhims, "IdtheLoai", "TenTheLoai", phim.IdtheLoai);
            ViewData["TenTinhTrang"] = new SelectList(_context.TinhTrangPhims, "IdtinhTrangPhim", "TenTinhTrang", phim.IdtinhTrangPhim);

            phim.TenPhim = Utilities.ToTittleCase(phim.TenPhim);

            if (fImg != null)
            {
                string extension = Path.GetExtension(fImg.FileName);
                string image = Utilities.SEOUrl(phim.TenPhim) + extension;
                phim.Img  = await Utilities.UploadFile(fImg, @"movies\", image.ToLower());
            }
            if (string.IsNullOrEmpty(phim.Img)) phim.Img = "default.jpg";

            _context.Add(phim);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Admin/AdminPhim/Edit/5
        public async Task<IActionResult> GetById(int? id)
        {
            if (id == null || _context.Phims == null)
            {
                return NotFound();
            }

            var phim = await _context.Phims.FindAsync(id);
            if (phim == null)
            {
                return NotFound();
            }
            ViewData["IdtheLoai"] = new SelectList(_context.TheLoaiPhims, "IdtheLoai", "IdtheLoai", phim.IdtheLoai);
            ViewData["TenTinhTrang"] = new SelectList(_context.TinhTrangPhims, "IdtinhTrangPhim", "TenTinhTrang", phim.IdtinhTrangPhim);
            return Json(new {status = "success", data = phim });
        }

        // POST: Admin/AdminPhim/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Idphim,IdtheLoai,IdtinhTrangPhim,Img,TenPhim,ThoiLuong,Gia,NgayKhoiChieu,DaoDien,DienVien,MoTa")] Phim phim, IFormFile fImg)
        {
            try
            {
                if (fImg != null)
                {
                    phim.TenPhim = Utilities.ToTittleCase(phim.TenPhim);    
                    string extension = Path.GetExtension(fImg.FileName);
                    string image = Utilities.SEOUrl(phim.TenPhim) + extension;
                    phim.Img  = await Utilities.UploadFile(fImg, @"movies\", image.ToLower());
                }
                _context.Update(phim);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhimExists(phim.Idphim))
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

        // GET
        public IActionResult getDateDelete(int? id)
        {
            if (id == null || _context.Phims == null)
            {
                return NotFound();
            }

            var phim = _context.Phims.Find(id);
            if (phim == null)
            {
                return NotFound();
            }

            Console.WriteLine(phim);

            return Json(new { data = phim });
        }

        // POST: Admin/AdminPhim/Delete/5
        public IActionResult Delete(int id)
        {
            if (_context.Phims == null)
            {
                return Problem("Entity set 'DbCinemasContext.Phims'  is null.");
            }
            var phim = _context.Phims.Find(id);
            if (phim != null)
            {
                _context.Phims.Remove(phim);
            }

                var url = "/Admin/AdminPhim/Index";
            _context.SaveChanges();
            return Json(new { status = "success", urlindex = url });
        }

        private bool PhimExists(int id)
        {
            return (_context.Phims?.Any(e => e.Idphim == id)).GetValueOrDefault();
        }
    }
}

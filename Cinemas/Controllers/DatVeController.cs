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
    public class DatVeController : Controller
    {
        private readonly DbCinemasContext _context;

        public DatVeController(DbCinemasContext context)
        {
            _context = context;
        }

        // GET: DatVe
        public IActionResult Index(int id,int idphong)
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            //Lấy lịch chiếu theo id
            var dbCinemasContext = _context.LichChieuPhims
                .Include(x => x.IdgioChieuNavigation)
                .Include(x => x.IdphimNavigation)
                .Include(x => x.IdphimNavigation.IdtheLoaiNavigation)
                .Include(x => x.IdphimNavigation.IdtinhTrangPhimNavigation)
                .Include(x => x.IdphongNavigation)
                .FirstOrDefault(x => x.IdlichChieu == id);

            //Lấy ghế đã đặt theo id lịch chiếu
            ViewData["GheDaDat"]= _context.HoaDons
                .AsNoTracking()
                .Where(x => x.IdlichChieu == id)
                .ToList();

            if (dbCinemasContext == null)
            {
                return NotFound();
            }

            //Lấy ghế của phòng
            ViewData["ChiTietPhong"] = _context.GhePhongs
                .AsNoTracking()
                .Where(x => x.Idphong == idphong)
                .Include(x => x.IdgheNavigation)
                .Select(x => new { x.IdgheNavigation.TenGhe, x.Idghe,x.TrangThaiGhe})
                .ToList();

            return View(dbCinemasContext);
        }

        public IActionResult PostDataDatVe(int id, int idphong)
        {
            var urldatve = $"/DatVe/Index?id={id}&idphong={idphong}";

            return Json(new { url = urldatve });
        }

            // GET: DatVe/Details/5
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
    }
}

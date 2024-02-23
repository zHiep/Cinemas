using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinemas.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Web.Helpers;

namespace Cinemas.Controllers
{
    public class PhimController : Controller
    {
        private readonly DbCinemasContext _context;
        public PhimController(DbCinemasContext context)
        {
            _context = context;
        }

        // Index tìm kiếm phim theo tên
        /*public IActionResult Index(string chuoitimkiem = "")
        {
            List<Phim> lsphim = new List<Phim>();
            chuoitimkiem = chuoitimkiem.ToLower();
            ViewData["ValInputSearch"] = chuoitimkiem;
            if (chuoitimkiem == "") {
                lsphim = _context.Phims
                    .AsNoTracking()
                    .Include(p => p.IdtheLoaiNavigation)
                    .Include(p => p.IdtinhTrangPhimNavigation)
                    .OrderByDescending(x=>x.Idphim).ToList();
            }else
            {
                lsphim = _context.Phims
                    .AsNoTracking()
                    .Include(p => p.IdtheLoaiNavigation)
                    .Include(p => p.IdtinhTrangPhimNavigation)
                    .Where(x => x.TenPhim.Contains(chuoitimkiem))
                    .OrderByDescending(x => x.Idphim).ToList();
            }

            ViewData["TenTheLoai"] = new SelectList(_context.TheLoaiPhims, "TenTheLoai", "TenTheLoai");

            return View(lsphim);
        }*/
        //Lọc theo thể loại
        public IActionResult Index(int IdTheLoai = 0)
        {
            List<Phim> lsphim = new List<Phim>();
            if (IdTheLoai == 0)
            {
                lsphim = _context.Phims
                    .AsNoTracking()
                    .Include(p => p.IdtheLoaiNavigation)
                    .Include(p => p.IdtinhTrangPhimNavigation)
                    .OrderByDescending(x => x.Idphim).ToList();
            }
            else
            {
                lsphim = _context.Phims
                    .AsNoTracking()
                    .Include(p => p.IdtheLoaiNavigation)
                    .Include(p => p.IdtinhTrangPhimNavigation)
                    .Where(x => x.IdtheLoai == IdTheLoai)
                    .OrderByDescending(x => x.Idphim).ToList();
            }

            return View(lsphim);
        }

        public IActionResult Filter(int IdTheLoai = 0)
        {
            var url = $"/Phim/Index?IdTheLoai={IdTheLoai}";
            if (IdTheLoai == 0)
            {
                url = $"/Phim/Index";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        public IActionResult SearchPhim (string chuoitimkiem) {

            var url = $"/Phim?chuoitimkiem={chuoitimkiem}";
            if (chuoitimkiem == null)
            {
                url = $"/Phim";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Phim/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Phims == null)
            {
                return NotFound();
            }

            var phim = await _context.Phims
                .Include(p => p.IdtheLoaiNavigation)
                .Include(p => p.IdtinhTrangPhimNavigation)
                .FirstOrDefaultAsync(m => m.Idphim == id);
            if (phim == null)
            {
                return NotFound();
            }

            return View(phim);
        }

        //Get lịch chiếu chi tiết
        public IActionResult GetLichChieuDetail(int id)
        {
            var LichChieu = _context.LichChieuPhims
                .Where(x => x.IdlichChieu == id)
                .Include(p => p.IdgioChieuNavigation)
                .Include(p => p.IdphimNavigation)
                .Select(x => new { x.IdgioChieuNavigation.GioChieu1, x.NgayChieu, x.IdphimNavigation.TenPhim, x.Idphong }).ToList();


            return Json(new {lich = LichChieu});
        }

        //Get LichChieuPhim
        public IActionResult GetLichChieu(int id)
        {
            var listNgayChieu = _context.LichChieuPhims
                .Where(x => x.Idphim == id)
                .GroupBy(x => new { x.NgayChieu })
                .Select(x => new { x.Key.NgayChieu })
                .ToList();
            int countNgay = listNgayChieu.Count();

            var listGioChieu = _context.LichChieuPhims
                .Where(x => x.Idphim == id)
                .Include(p => p.IdgioChieuNavigation)
                .Select(x => new { x.IdgioChieuNavigation.GioChieu1,x.NgayChieu,x.GheTrong,x.IdlichChieu })
                .ToList();
            int countGioChieu = listGioChieu.Count();

            var phim = _context.Phims.Find(id);

            return Json(new { ngaychieu = listNgayChieu, countgio = countGioChieu, countngay = countNgay, phim = phim, giochieu = listGioChieu });
        }

        
            


        private void DoSomething(object state)
        {
            Console.WriteLine("Đã chờ xong 3 giây");
        }

        public IActionResult Rap()
        {
            return View();
        }
    }
}

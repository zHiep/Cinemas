using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinemas.Models;
using Cinemas.Helpper;
using Cinemas.ModelViews;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using NuGet.Packaging.Signing;

namespace Cinemas.Controllers
{
    public class HoaDonController : Controller
    {

        private readonly DbCinemasContext _context;
        public INotyfService _notyfService { get; }
        public HoaDonController(DbCinemasContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public IActionResult Index(string strtimestart = "", string strtimeend = "")
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            int idkh = Convert.ToInt32(HttpContext.Session.GetString("KhId"));
            if (strtimestart == "")
            {
                ViewData["ValTimeStart"] = "null";
            }
            else
            {
            ViewData["ValTimeStart"] = strtimestart.ToString();
            }

            if (strtimeend == "")
            {
                ViewData["ValTimeEnd"] = "null";
            }
            else
            {
                ViewData["ValTimeEnd"] = strtimeend.ToString();

            }
           
            strtimestart = strtimestart.Trim();
            strtimeend = strtimeend.Trim();
            List<HoaDon> lshoadon = new List<HoaDon>();

            if (strtimestart == "" && strtimeend == "")
            {
                lshoadon = _context.HoaDons
                    .AsNoTracking()
                    .Include(x => x.IdkhNavigation)
                    .Include(x => x.IdlichChieuNavigation)
                    .Include(x => x.IdlichChieuNavigation.IdphimNavigation)
                    .Include(x => x.IdlichChieuNavigation.IdphongNavigation)
                    .Include(x => x.IdlichChieuNavigation.IdgioChieuNavigation)
                    .Where(x => x.Idkh == idkh)
                    .ToList();
            }else if(strtimestart == "" && strtimeend != "")
            {
                DateTime timeend = DateTime.Parse(strtimeend);
                lshoadon = _context.HoaDons
                    .AsNoTracking()
                    .Include(x => x.IdkhNavigation)
                    .Include(x => x.IdlichChieuNavigation)
                    .Include(x => x.IdlichChieuNavigation.IdphimNavigation)
                    .Include(x => x.IdlichChieuNavigation.IdphongNavigation)
                    .Include(x => x.IdlichChieuNavigation.IdgioChieuNavigation)
                    .Where(x => x.NgayMuaVe <= timeend)
                    .Where(x => x.Idkh == idkh)
                    .ToList();
            }else if (strtimeend == "" && strtimestart != "")
            {
                DateTime timestart = DateTime.Parse(strtimestart);
                lshoadon = _context.HoaDons
                    .AsNoTracking()
                    .Include(x => x.IdkhNavigation)
                    .Include(x => x.IdlichChieuNavigation)
                    .Include(x => x.IdlichChieuNavigation.IdphimNavigation)
                    .Include(x => x.IdlichChieuNavigation.IdphongNavigation)
                    .Include(x => x.IdlichChieuNavigation.IdgioChieuNavigation)
                    .Where(x => x.NgayMuaVe >= timestart)
                    .Where(x => x.Idkh == idkh)
                    .ToList();
            }
            else
            {
                DateTime timestart = DateTime.Parse(strtimestart);
                DateTime timeend = DateTime.Parse(strtimeend);
                lshoadon = _context.HoaDons
                    .AsNoTracking()
                    .Include(x => x.IdkhNavigation)
                    .Include(x => x.IdlichChieuNavigation)
                    .Include(x => x.IdlichChieuNavigation.IdphimNavigation)
                    .Include(x => x.IdlichChieuNavigation.IdphongNavigation)
                    .Include(x => x.IdlichChieuNavigation.IdgioChieuNavigation)
                    .Where(x => x.NgayMuaVe >= timestart && x.NgayMuaVe <= timeend)
                    .Where(x => x.Idkh == idkh)
                    .ToList();
            }


            return View(lshoadon);
        }

        public IActionResult Filter(string strtimestart = "", string strtimeend = "")
        {
            var url = $"/HoaDon/Index?strtimestart={strtimestart}&strtimeend={strtimeend}";
            if (strtimestart == null && strtimeend == null)
            {
                url = $"/HoaDon/Index";
            }else if (strtimestart == null && strtimeend != null)
            {
                url = $"/HoaDon/Index?strtimeend={strtimeend}";
            }
            else if(strtimestart != null && strtimeend == null) {
                url = $"/HoaDon/Index?strtimestart={strtimestart}";
            }
                return Json(new { status = "success", redirectUrl = url });
        }

        public IActionResult ThanhToan(int idkh, int idlichchieu, int soghe, string tenghe, int tongtien)
        {
            try
            {
                // Lấy thông tin LichChieuPhim từ ID lịch chiếu
                var lichChieu = _context.LichChieuPhims
                    .Include(x => x.IdgioChieuNavigation)
                    .Include(x => x.IdphimNavigation)
                    .Include(x => x.IdphongNavigation)
                    .FirstOrDefault(x => x.IdlichChieu == idlichchieu);
                var khachhang = _context.KhachHangs.FirstOrDefault(x => x.Idkh == idkh);
                HoaDon hoadon = new HoaDon
                {
                    Idkh = idkh,
                    IdlichChieu = idlichchieu,
                    NgayMuaVe = DateTime.Now,
                    SoLuongGhe = soghe,
                    TenGhe = tenghe,
                    TongTien = tongtien,
                    IdlichChieuNavigation = lichChieu,
                    IdkhNavigation = khachhang
                };
                try
                {
                    _context.Add(hoadon);
                    string url = "/Phim/Index";
                    //update lại số ghế trống
                    int ghetrong = Convert.ToInt32(lichChieu.GheTrong);
                    int ghetrongconlai = ghetrong - soghe;
                    lichChieu.GheTrong = ghetrongconlai;
                    _context.Update(lichChieu);
                    _context.SaveChanges();
                    return Json(new { success = true, urlTrangChu = url });
                }
                catch
                {
                    return Json(new { success = false });
                }
            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}

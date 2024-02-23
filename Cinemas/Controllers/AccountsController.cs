using System;
using System.Data;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using Cinemas.Extentions;
using Cinemas.Helpper;
using Cinemas.Models;
using Cinemas.ModelViews;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Cinemas.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly DbCinemasContext _context;
        public INotyfService _notyfService { get; }
        public AccountsController(DbCinemasContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }

            [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.Sdt.ToLower() == Phone.ToLower());
                if (khachhang != null)
                    return Json(data: "Số điện thoại : " + Phone + "đã được sử dụng");

                return Json(data: true);

            }
            catch
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateOldPass(string OldPass)
        {
            try
            {   
                var idkh = Convert.ToInt32(HttpContext.Session.GetString("KhId"));
                var account = _context.KhachHangs.Find(idkh);
                var pass = (OldPass.Trim() + account.Salt.Trim()).ToMD5();
                if (pass != account.Password)
                    return Json(data: "Mật khẩu không chính xác");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                _notyfService.Success("test");
                if (khachhang != null)
                    return Json(data: "Email : " + Email + " đã được sử dụng");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [Route("dang-nhap.html", Name = "Login")]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("KhId");
            if (taikhoanID != null) return RedirectToAction("Index", "Phim");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [Route("dang-nhap.html", Name = "Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(model.Email);
                    if (!isEmail) return View(model);

                    
                    KhachHang kh = _context.KhachHangs
                        .Include(p => p.Role)
                        .SingleOrDefault(p => p.Email.ToLower() == model.Email.ToLower().Trim());

                    if (kh == null)
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        return View(model);
                    }
                    string pass = (model.Password.Trim() + kh.Salt.Trim()).ToMD5();
                    if (kh.Password.Trim() != pass)
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        return View(model);
                    }
                    if (kh.Active == false)
                    {
                        return RedirectToAction("ThongBao", "Accounts");
                    }
                    // đăng nhập thành công
                    //ghi nhận thời gian đăng nhập
                    kh.LastLogin = DateTime.Now.ToLocalTime(); ;
                    _context.Update(kh);
                    await _context.SaveChangesAsync();

                    //identity
                    //lưu session makh
                    HttpContext.Session.SetString("KhId", kh.Idkh.ToString());

                    var idkh = HttpContext.Session.GetString("KhId");
                    if (idkh != null)
                    {
                        KhachHang thongtinkh = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.Idkh == Convert.ToInt32(idkh));
                        if (thongtinkh!=null)
                        {
                            HttpContext.Session.SetString("ThongTinKH", JsonConvert.SerializeObject(thongtinkh));
                        }
                    }
                    //set thời gian bắt đầu
                    HttpContext.Session.SetString("TimeSession", Convert.ToString(DateTime.Now));
                    HttpContext.Session.Remove("Time");
                    //identity
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, kh.TenKh),
                        new Claim(ClaimTypes.Email, kh.Email),
                        new Claim("KhId",kh.Idkh.ToString()),
                        new Claim("RoleId",kh.RoleId.ToString()),
                        new Claim(ClaimTypes.Role, kh.Role.RoleName)
                    };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    await HttpContext.SignInAsync(userPrincipal);
                    _notyfService.Success("Đăng nhập thành công");

                    if (kh.Role.RoleId == 1)
                    {
                        return RedirectToAction("Index", "Home", routeValues: new { Area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Phim");
                    }
                }
            }
            catch   
            {
                return RedirectToAction("Login", "Accounts");
            }
            return RedirectToAction("Login", "Accounts");
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangkyTaiKhoan")]
        public IActionResult DangkyTaiKhoan()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangkyTaiKhoan")]
        public async Task<IActionResult> DangkyTaiKhoan(RegisterViewModel taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();
                    KhachHang khachhang = new KhachHang
                    {
                        TenKh = taikhoan.FullName,
                        Sdt = taikhoan.Phone.Trim().ToLower(),
                        Email = taikhoan.Email.Trim().ToLower(),
                        Password = (taikhoan.Password + salt.Trim()).ToMD5(),
                        Active = true,
                        Salt = salt,
                        RoleId = 2,
                        CreateDate = DateTime.Now.ToLocalTime(),
                        Avatar = "default.jpg"
                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        //Lưu Session IdKh
                        HttpContext.Session.SetString("KhId", khachhang.Idkh.ToString());
                        var idkh = HttpContext.Session.GetString("KhId");
                        if (idkh != null)
                        {
                            KhachHang thongtinkh = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.Idkh == Convert.ToInt32(idkh));
                            if (thongtinkh!=null)
                            {
                                HttpContext.Session.SetString("ThongTinKH", JsonConvert.SerializeObject(thongtinkh));
                            }
                        }
                        //set thời gian bắt đầu
                        HttpContext.Session.SetString("TimeSession", Convert.ToString(DateTime.Now));
                        HttpContext.Session.Remove("Time");
                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,khachhang.TenKh),
                            new Claim("KhId", khachhang.Idkh.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng ký thành công");
                        return RedirectToAction("Index", "Phim");
                    }
                    catch
                    {
                        return RedirectToAction("DangkyTaiKhoan", "Accounts");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }
        
        [AllowAnonymous]
        [Authorize, HttpPost]
        public IActionResult ChangePasswordUser(ChangePasswordUserViewModel model)
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var IdKh = Convert.ToInt32(HttpContext.Session.GetString("KhId"));
            if (IdKh == null) return RedirectToAction("Login", "Accounts");
            if (ModelState.IsValid)
            {   
                var account = _context.KhachHangs.Find(IdKh);
                if (account == null) return RedirectToAction("Login", "Accounts");
                try
                {       string newpass = (model.NewPass.Trim() + account.Salt.Trim()).ToMD5();
                        account.Password = newpass;
                        _context.Update(account);
                        _context.SaveChanges();
                        _notyfService.Success("Đổi mật khẩu thành công");
                        return RedirectToAction("ThongTinTaiKhoan", "Accounts");
                }
                catch
                {
                    _notyfService.Success("Đổi mật khẩu THẤT BẠI");
                    return RedirectToAction("ThongTinTaiKhoan", "Accounts");
                }
            }
            _notyfService.Success("Đổi mật khẩu THẤT BẠI");
            return NotFound();
        }

        [Route("thong-tin-tai-khoan.html", Name = "ThongTinTaiKhoan")]
        [Authorize, HttpGet]
        public IActionResult ThongTinTaiKhoan()
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var idkh = HttpContext.Session.GetString("KhId");
            if (idkh == null) return RedirectToAction("Login", "Accounts");
            var thongtinkh = _context.KhachHangs.AsNoTracking().FirstOrDefault(x => x.Idkh == Convert.ToInt32(idkh));
            if (thongtinkh == null) return NotFound();
            List<SelectListItem> lsGioiTinh = new List<SelectListItem>();
            lsGioiTinh.Add(new SelectListItem() { Text = "Nam", Value = "true" });
            lsGioiTinh.Add(new SelectListItem() { Text = "Nữ", Value = "false" });
            ViewBag.LsGioiTinh = new SelectList(lsGioiTinh, "Value", "Text", thongtinkh.GioiTinh.ToString());
            return View(thongtinkh);
        }

        [Route("thong-tin-tai-khoan.html", Name = "ThongTinTaiKhoan")]
        [Authorize, HttpPost]
        public async Task<IActionResult> ThongTinTaiKhoan(KhachHang model, IFormFile fImg)
        {
            if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var KhId = HttpContext.Session.GetString("KhId");
            if (KhId == null) return RedirectToAction("Login", "Accounts");
            var account = _context.KhachHangs.AsNoTracking().FirstOrDefault(x => x.Idkh == int.Parse(KhId));
            try
            {
                if (fImg != null)
                {
                    string nameavt = model.TenKh.ToString() + model.Idkh.ToString();
                    string avt = Utilities.ToTittleCase(nameavt);
                    string extension = Path.GetExtension(fImg.FileName);
                    string image = Utilities.SEOUrl(avt) + extension;
                    account.Avatar  = await Utilities.UploadFile(fImg, @"avatars\", image.ToLower());
                }
                    account.DiaChi = model.DiaChi;
                    account.NgaySinh = model.NgaySinh;
                    account.GioiTinh = model.GioiTinh;
                    account.Email = model.Email;
                    account.TenKh = model.TenKh;
                    account.Sdt = model.Sdt;
                _context.Update(account);
                _context.SaveChanges();
                 return RedirectToAction("ThongTinTaiKhoan", "Accounts");
            }
            catch
            {
                return View(model);
            }
        }

        [Route("lich-su-giao-dich.html", Name = "LichSuGiaoDich")]
        public IActionResult LichSuGiaoDich()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }


        [HttpGet]
        [Route("dang-xuat.html", Name = "Logout")]
        public IActionResult Logout()
        {
            TimeSpan sessionElapsedTime = DateTime.Now - DateTime.Parse(HttpContext.Session.GetString("TimeSession")) ;
            //sesion thời gian đăng nhập
            string lowPrecisionTime = sessionElapsedTime.ToString(@"hh\:mm\:ss");
            Console.WriteLine(sessionElapsedTime);
            HttpContext.Session.SetString("Time", Convert.ToString(lowPrecisionTime));

            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("TimeSession");
            HttpContext.Session.Remove("KhId");
            HttpContext.Session.Remove("ThongTinKH");
            return RedirectToAction("Login","Accounts");
        }
    }
}
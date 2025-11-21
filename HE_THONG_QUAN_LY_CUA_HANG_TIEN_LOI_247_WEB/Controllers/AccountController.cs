using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPermissionServices _permissionService;
        private readonly IQuanLyServices _quanLyServices;

        public AccountController(IPermissionServices permissionService, IQuanLyServices quanLyServices)
        {
            _permissionService = permissionService;
            _quanLyServices = quanLyServices;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [Route("Account/LoginToSystem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginToSystem(string username, string password)
        {
            try
            {
                var user = _quanLyServices.Login(username, password);

                if (user == null)
                {
                    return Unauthorized(); // Đăng nhập thất bại
                }

                var permissions = _permissionService.GetPermissionsForUser(user.Id);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.TenDangNhap), 
                    new Claim(ClaimTypes.NameIdentifier, user.Id), 
                    new Claim(ClaimTypes.Email, user.Email)
                };

                foreach (var role in user.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Role.Code)); 
                }

                foreach (var permission in permissions)
                {
                    claims.Add(new Claim("Permission", permission.Code));  // Code quyền
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                var redirectUrl = Url.Action("Index", "HomeAdmin", new { area = "Admin" });

                return Json(new { status = "SUCCESS", redirectUrl });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                return Json(new { status = "error", message = "Lỗi đăng nhập", error = ex.ToString() });
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {

                // Đăng xuất người dùng và xóa cookie
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Xóa session nếu bạn đang sử dụng session
                HttpContext.Session.Clear();

                var redirectUrl = Url.Action("Login", "Account");

                return Json(new { status = "SUCCESS", redirectUrl });
            }
            catch(Exception ex)
            {
                return Json(new { status = "error", message = "Lỗi đăng xuất", error = ex.ToString() });
            }
        }

    }
}

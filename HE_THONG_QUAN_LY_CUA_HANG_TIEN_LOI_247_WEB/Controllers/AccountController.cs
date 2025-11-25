using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPermissionServices _permissionService;
        private readonly IQuanLyServices _quanLyServices;
        private readonly IEmailService _emailService;

        public AccountController(IPermissionServices permissionService, IQuanLyServices quanLyServices, IEmailService emailService)
        {
            _permissionService = permissionService;
            _quanLyServices = quanLyServices;
            _emailService = emailService;
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
                var redirectUrl = Url.Action("LoiDangNhap", "LoiDangNhap", new { area = "" });

                var user = _quanLyServices.Login(username, password);

                if (user == null)
                {

                    return Json(new { status = "error", redirectUrl });
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

                redirectUrl = Url.Action("Index", "HomeAdmin", new { area = "Admin" });

                return Json(new { status = "SUCCESS", redirectUrl });
            }
            catch (Exception ex)
            {
                var redirectUrl = Url.Action("LoiDangNhap", "LoiDangNhap", new { area = "" });

                return Json(new { status = "error", redirectUrl });

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


        [HttpPost]
        public async Task<IActionResult> SendResetRequest(string email)
        {
            // 1. Dùng Service để tìm user
            var user = _quanLyServices.GetByEmail(email);

            if (user == null)
            {
                return Json(new { success = true }); // Bảo mật: không báo lỗi nếu sai email
            }

            // 2. Tạo mật khẩu mới ngẫu nhiên
            string newPassword = GenerateRandomPassword();

            // 3. Mã hóa Base64 để truyền qua URL an toàn (đây CHƯA PHẢI là Hash lưu DB)
            string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(newPassword));

            var callbackUrl = Url.Action("ConfirmReset", "Account",
                new { email = email, newPassword = encodedPassword }, Request.Scheme);

            await _emailService.SendEmailAsync(email, "Xác nhận cấp mật khẩu mới",
                $"<h3>Yêu cầu cấp lại mật khẩu</h3>" +
                $"<p>Mật khẩu tạm thời: <strong style='color:red; font-size:18px'>{newPassword}</strong></p>" +
                $"<p>Vui lòng <a href='{callbackUrl}'>BẤM VÀO ĐÂY</a> để kích hoạt mật khẩu này.</p>");

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult ConfirmReset(string email, string newPassword)
        {
            if (email == null || newPassword == null)
            {
                return RedirectToAction("Login", new { status = "error" });
            }

            // Kiểm tra user tồn tại bằng Service
            var user = _quanLyServices.GetByEmail(email);
            if (user == null)
            {
                return RedirectToAction("Login", new { status = "error" });
            }

            try
            {
                // Giải mã Base64 lấy mật khẩu trần
                var decodedPlainPassword = Encoding.UTF8.GetString(Convert.FromBase64String(newPassword));

                // 4. GỌI SERVICE ĐỂ RESET MẬT KHẨU
                // Service sẽ tự lo việc Hash mật khẩu trước khi lưu
                bool result = _quanLyServices.ResetPassword(email, decodedPlainPassword);

                if (result)
                {
                    return RedirectToAction("Login", new { status = "reset_success" });
                }
                else
                {
                    return RedirectToAction("Login", new { status = "error" });
                }
            }
            catch
            {
                return RedirectToAction("Login", new { status = "error" });
            }
        }

        // Hàm random (giữ nguyên hoặc dùng hàm Helper nếu có)
        private string GenerateRandomPassword()
        {
            return "MatKhauMoi123@"; // Bạn nhớ thay bằng logic random thật nhé
        }
    }
}

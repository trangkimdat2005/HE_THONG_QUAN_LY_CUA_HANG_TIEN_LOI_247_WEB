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

                if(user.UserRoles.Any(ur => ur.Role.Code == "ADMIN"))
                {
                    redirectUrl = Url.Action("Index", "HomeAdmin", new { area = "Admin" });

                }
                else if (user.UserRoles.Any(ur => ur.Role.Code == "NV_KHO"))
                {
                    redirectUrl = Url.Action("DanhSachHangTonKho", "QuanLyKhoHang", new { area = "Admin" });
                }
                else if (user.UserRoles.Any(ur => ur.Role.Code == "NV_BANHANG"))
                {
                    redirectUrl = Url.Action("DanhSachSanPham", "QuanLyHangHoa", new { area = "Admin" });
                }
                else if (user.UserRoles.Any(ur => ur.Role.Code == "KHACHHANG"))
                {
                    redirectUrl = Url.Action("Index", "HomeKhachHang", new { area = "User" });
                }


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
            // BƯỚC 1: Gọi hàm mới để tạo Token và lưu vào RAM (hạn 5p)
            // Hàm này trả về null nếu email không tồn tại
            string token = _quanLyServices.GenerateRecoveryToken(email);

            if (token == null)
            {
                // Email không tồn tại -> Vẫn báo success để bảo mật
                return Json(new { success = true });
            }

            // BƯỚC 2: Tạo mật khẩu mới ngẫu nhiên (Chỉ để hiển thị, CHƯA LƯU DB)
            string newPassword = _quanLyServices.GenerateRandomPassword();

            // Mã hóa để truyền qua URL
            string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(newPassword));
            string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token)); // Mã hóa cả Token

            // BƯỚC 3: Tạo Link (Bây giờ phải kèm theo cả TOKEN)
            var callbackUrl = Url.Action("ConfirmReset", "Account",
                new { email = email, newPassword = encodedPassword, token = encodedToken }, Request.Scheme);

            // BƯỚC 4: Gửi Email
            await _emailService.SendEmailAsync(email, "Xác nhận cấp mật khẩu mới",
                $"<h3>Yêu cầu cấp lại mật khẩu</h3>" +
                $"<p>Mật khẩu tạm thời của bạn là: <strong style='color:red; font-size:18px'>{newPassword}</strong></p>" +
                $"<p>Link này chỉ có hiệu lực trong vòng <b>5 phút</b> và chỉ dùng được <b>1 lần</b>.</p>" +
                $"<p>Vui lòng <a href='{callbackUrl}'>BẤM VÀO ĐÂY</a> để kích hoạt mật khẩu này.</p>");

            return Json(new { success = true });
        }

        // 2. HÀM XÁC NHẬN TỪ LINK EMAIL (SỬA LẠI)
        [HttpGet]
        public IActionResult ConfirmReset(string email, string newPassword, string token)
        {
            // Phải có đủ 3 cái này mới làm việc được
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", new { status = "error" });
            }

            try
            {
                // Giải mã
                var decodedPlainPassword = Encoding.UTF8.GetString(Convert.FromBase64String(newPassword));
                var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));

                // BƯỚC 5: GỌI HÀM MỚI ĐỂ CHECK TOKEN & ĐỔI PASS
                // Hàm này sẽ tự vào RAM kiểm tra xem Token còn sống không, đúng không?
                bool result = _quanLyServices.ExecuteResetPassword(email, decodedToken, decodedPlainPassword);

                if (result)
                {
                    // Thành công -> Token trong RAM đã bị xóa ngay lập tức
                    return RedirectToAction("Login", new { status = "reset_success" });
                }
                else
                {
                    // Thất bại (Do hết hạn 5p hoặc link đã bấm rồi nên token bị xóa mất)
                    return RedirectToAction("Login", new { status = "error" });
                }
            }
            catch
            {
                return RedirectToAction("Login", new { status = "error" });
            }
        }
    }
}

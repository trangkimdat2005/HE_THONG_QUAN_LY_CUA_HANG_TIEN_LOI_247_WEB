using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Controllers
{
    //public class AccountController : Controller
    //{
    //    private readonly ApplicationDbContext _context;
    //    private readonly IPermissionServices _permissionService;

    //    public AccountController(ApplicationDbContext context, IPermissionServices permissionService)
    //    {
    //        _context = context;
    //        _permissionService = permissionService;
    //    }


    //    [Route("/login")]
    //    public IActionResult Login()
    //    {
    //        return View();
    //    }

        
    //    [HttpPost]
    //    public async Task<IActionResult> Login(string username, string password)
    //    {
    //        // 1. Kiểm tra thông tin đăng nhập (từ bảng TaiKhoan)
    //        var user = await _dbContext.TaiKhoan
    //            .FirstOrDefaultAsync(x => x.tenDangNhap == username && x.matKhauHash == HashPassword(password));

    //        if (user == null)
    //        {
    //            return Unauthorized(); // Đăng nhập thất bại
    //        }

    //        // 2. Lấy quyền của người dùng từ PermissionService
    //        var permissions = _permissionService.GetPermissionsForUser(user.id);

    //        // 3. Tạo Claims từ các quyền của người dùng
    //        var claims = new List<Claim>
    //    {
    //        new Claim(ClaimTypes.Name, user.tenDangNhap),  // Tên đăng nhập của người dùng
    //        new Claim(ClaimTypes.NameIdentifier, user.id)  // ID của người dùng
    //    };

    //        // Thêm quyền vào Claims
    //        foreach (var permission in permissions)
    //        {
    //            claims.Add(new Claim("Permission", permission.code));  // Code quyền
    //        }

    //        // 4. Tạo ClaimsIdentity và ClaimsPrincipal
    //        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    //        var principal = new ClaimsPrincipal(identity);

    //        // 5. Lưu Claims vào Cookie (Authentication Ticket)
    //        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

    //        // 6. Redirect người dùng đến trang chủ (hoặc trang cần thiết)
    //        return RedirectToAction("Index", "Home");
    //    }

    //}
}

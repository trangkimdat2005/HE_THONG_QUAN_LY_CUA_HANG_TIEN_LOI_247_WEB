using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Controllers
{
    public class LoiDangNhapController : Controller
    {
        public IActionResult LoiDangNhap()
        {
            return View();
        }

        public IActionResult KhongCoQuyen()
        {
            return View();
        }
    }
}

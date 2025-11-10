using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Controllers.Admin
{
    public class ThanhToanController : Controller
    {
        [Route("ThanhToan/ThanhToan")]
        public IActionResult ThanhToan()
        {
            return View();
        }
        [Route("ThanhToan/ThanhToanHoaDon")]
        public IActionResult ThanhToanHoaDon()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyKhuyenMaiController : Controller
    {
        [Route("/QuanLyKhuyenMai/KhuyenMai")]
        public IActionResult KhuyenMai()
        {
            return View();
        }
    }
}

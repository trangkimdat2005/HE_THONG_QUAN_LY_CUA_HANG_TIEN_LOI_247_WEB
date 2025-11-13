using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyHoaDonController : Controller
    {
        [Route("/QuanLyHoaDon/DanhSachHoaDon")]
        public IActionResult DanhSachHoaDon()
        {
            return View();
        }
        [Route("/QuanLyHoaDon/ThanhToanHoaDon")]
        public IActionResult ThanhToanHoaDon()
        {
            return View();
        }
    }
}

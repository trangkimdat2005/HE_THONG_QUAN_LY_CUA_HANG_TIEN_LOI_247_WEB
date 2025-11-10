using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GiaoDichHoanTraController : Controller
    {
        [Route("/GiaoDichHoanTra/ChinhSachDoiTra")]
        public IActionResult ChinhSachDoiTra()
        {
            return View();
        }
        [Route("/GiaoDichHoanTra/GiaoDichThanhToan")]
        public IActionResult GiaoDichThanhToan()
        {
            return View();
        }
        [Route("/GiaoDichHoanTra/PhieuDoiTra")]
        public IActionResult PhieuDoiTra()
        {
            return View();
        }
    }
}

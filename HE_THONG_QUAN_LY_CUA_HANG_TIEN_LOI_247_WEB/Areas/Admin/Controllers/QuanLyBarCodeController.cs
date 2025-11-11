using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyBarCodeController : Controller
    {
        [Route("/QuanLyBarCode/DanhSachTemNhan")]
        public IActionResult DanhSachTemNhan()
        {
            return View();
        }
        [Route("/QuanLyBarCode/DinhDanhSanPham")]
        public IActionResult DinhDanhSanPham()
        {
            return View();
        }
    }
}

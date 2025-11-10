using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyDonHangController : Controller
    {
        [Route("/QuanLyDonHang/DanhSachDonHang")]
        public IActionResult DanhSachDonHang()
        {
            return View();
        }
        [Route("/QuanLyDonHang/XemChiTietHoaDon")]
        public IActionResult XemChiTietHoaDon()
        {
            return View();
        }
    }
}

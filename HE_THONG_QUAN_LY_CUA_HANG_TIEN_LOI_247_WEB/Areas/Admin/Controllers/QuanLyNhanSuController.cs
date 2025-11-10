using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyNhanSuController : Controller
    {
        [Route("/QuanLyNhanSu/DanhSachNhanVien")]
        public IActionResult DanhSachNhanVien()
        {
            return View();
        }
        [Route("/QuanLyNhanSu/LichLamViec")]
        public IActionResult LichLamViec()
        {
            return View();
        }
        [Route("/QuanLyNhanSu/PhanCongCaLamViec")]
        public IActionResult PhanCongCaLamViec()
        {
            return View();
        }
    }
}

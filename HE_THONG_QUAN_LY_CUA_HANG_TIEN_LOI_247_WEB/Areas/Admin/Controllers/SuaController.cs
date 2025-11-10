using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SuaController : Controller
    {
        [Route("/Sua/SuaHoaDon")] 
        public IActionResult SuaHoaDon()
        {
            return View();
        }
        [Route("/Sua/SuaHoanTra")]
        public IActionResult SuaHoanTra()
        {
            return View();
        }
        [Route("/Sua/SuaKhachHang")]
        public IActionResult SuaKhachHang()
        {
            return View();
        }
        [Route("/Sua/SuaMaKhuyenMai")]
        public IActionResult SuaMaKhuyenMai()
        {
            return View();
        }
        [Route("/Sua/SuaNCC")]
        public IActionResult SuaNCC()
        {
            return View();
        }
        [Route("/Sua/SuaNhanSu")]
        public IActionResult SuaNhanSu()
        {
            return View();
        }
        [Route("/Sua/SuaPhanCongCaLamViec")]
        public IActionResult SuaPhanCongCaLamViec()
        {
            return View();
        }
        [Route("/Sua/SuaTaiKhoan")]
        public IActionResult SuaTaiKhoan()
        {
            return View();
        }
    }
}

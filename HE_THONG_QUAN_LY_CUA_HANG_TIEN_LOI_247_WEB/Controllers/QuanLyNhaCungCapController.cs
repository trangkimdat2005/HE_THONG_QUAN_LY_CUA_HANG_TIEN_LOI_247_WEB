using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Controllers.Admin
{
    public class QuanLyNhaCungCapController : Controller
    {
        [Route("/QuanLyNhaCungCap/DanhSachNhaCungCap")]
        public IActionResult DanhSachNhaCungCap()
        {
            return View();
        }
        [Route("/QuanLyNhaCungCap/LichSuGiaoDich")]
        public IActionResult LichSuGiaoDich()
        {
            return View();
        }
    }
}

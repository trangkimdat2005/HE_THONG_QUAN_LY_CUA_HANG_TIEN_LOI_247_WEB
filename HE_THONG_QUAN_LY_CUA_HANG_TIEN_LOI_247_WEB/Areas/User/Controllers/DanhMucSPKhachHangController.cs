using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.User.Controllers
{
    public class DanhMucSPKhachHangController : Controller
    {
        [Route("/DanhSachSanPhamKhachHang")]
        public IActionResult DanhSachSanPhamKhachHang()
        {
            return View();
        }
    }
}

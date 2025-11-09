using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Controllers.User
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

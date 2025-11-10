using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Controllers.Admin
{
    public class QuanLyKhachHangController : Controller
    {
        [Route("/QuanLyKhachHang/DanhSachKhachHang")]
        public IActionResult DanhSachKhachHang()
        {
            return View();
        }
        [Route("/QuanLyKhachHang/LichSuMuaHang")]
        public IActionResult LichSuMuaHang()
        {
            return View();
        }
        [Route("/QuanLyKhachHang/TheThanhVien")]
        public IActionResult TheThanhVien()
        {
            return View();
        }
    }
}

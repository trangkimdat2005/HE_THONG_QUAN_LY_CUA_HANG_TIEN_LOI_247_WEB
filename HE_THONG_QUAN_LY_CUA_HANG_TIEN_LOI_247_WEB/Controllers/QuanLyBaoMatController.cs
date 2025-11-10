using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Controllers.Admin
{
    public class QuanLyBaoMatController : Controller
    {
        [Route("/QuanLyBaoMat/DanhSachTaiKhoan")]
        public IActionResult DanhSachTaiKhoan()
        {
            return View();
        }
        [Route("/QuanLyBaoMat/DanhSachTaiKhoanKhachHang")]
        public IActionResult DanhSachTaiKhoanKhachHang()
        {
            return View();
        }
        [Route("/QuanLyBaoMat/DanhSachTaiKhoanNhanVien")]
        public IActionResult DanhSachTaiKhoanNhanVien()
        {
            return View();
        }
        [Route("/QuanLyBaoMat/LichSuMuaHang")]
        public IActionResult LichSuMuaHang()
        {
            return View();
        }
    }
}

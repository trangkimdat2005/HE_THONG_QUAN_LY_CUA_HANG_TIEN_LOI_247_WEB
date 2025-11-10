using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Controllers.Admin
{
    public class QuanLyHangHoaController : Controller
    {
        [Route("/QuanLyHangHoa/DanhSachDanhMuc")]
        public IActionResult DanhSachDanhMuc()
        {
            return View();
        }
        [Route("/QuanLyHangHoa/DanhSachDonVi")]
        public IActionResult DanhSachDonVi()
        {
            return View();
        }
        [Route("/QuanLyHangHoa/DanhSachNhanHieu")]
        public IActionResult DanhSachNhanHieu()
        {
            return View();
        }
        [Route("/QuanLyHangHoa/DanhSachSanPham")]
        public IActionResult DanhSachSanPham()
        {
            return View();
        }
        [Route("/QuanLyHangHoa/LichSuGiaBan")]
        public IActionResult LichSuGiaBan()
        {
            return View();
        }
    }
}

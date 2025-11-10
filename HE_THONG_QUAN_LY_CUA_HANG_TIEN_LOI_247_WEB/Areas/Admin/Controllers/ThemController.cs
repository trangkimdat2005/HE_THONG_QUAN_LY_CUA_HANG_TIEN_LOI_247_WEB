using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThemController : Controller
    {
        [Route("Admin/Them/ThemBanHang")]
        public IActionResult ThemDanhMucViTRi()
        {
            return View();
        }
        public IActionResult ThenHoaDon()
        {
            return View();
        }
        public IActionResult ThemHoanTra()
        {
            return View();
        }
        public IActionResult ThemKiemKe()
        {
            return View();
        }
        public IActionResult ThemKhachHang()
        {
            return View();
        }
        public IActionResult ThemMaKhuyenMai()
        {
            return View();
        }
        public IActionResult ThemNCC()
        {
            return View();
        }
        public IActionResult ThemNhanSu()
        {
            return View();
        }
        public IActionResult ThemNhapKho()
        {
            return View();
        }
        public IActionResult ThemPhanCongCaLamViec()
        {
            return View();
        }
        public IActionResult ThemPhieuDoiTra()
        {
            return View();
        }
        public IActionResult ThemTaiKhoan()
        {
            return View();
        }
        public IActionResult ThemViTriSanPham()
        {
            return View();
        }
    }
}

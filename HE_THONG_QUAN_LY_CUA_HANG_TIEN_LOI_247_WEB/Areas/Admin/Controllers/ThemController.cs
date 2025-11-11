using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThemController : Controller
    {
        [Route("/Them/ThemBanHang")]
        public IActionResult ThemDanhMucViTRi()
        {
            return View();
        }
        [Route("/Them/ThemHoaDon")]
        public IActionResult ThemHoaDon()
        {
            return View();
        }
        [Route("/Them/ThemHoanTra")]
        public IActionResult ThemHoanTra()
        {
            return View();
        }
        [Route("/Them/ThemKiemKe")]
        public IActionResult ThemKiemKe()
        {
            return View();
        }
        [Route("/Them/ThemKhachHang")]
        public IActionResult ThemKhachHang()
        {
            return View();
        }
        [Route("/Them/ThemMaKhuyenMai")]
        public IActionResult ThemMaKhuyenMai()
        {
            return View();
        }
        [Route("/Them/ThemNCC")]
        public IActionResult ThemNCC()
        {
            return View();
        }
        [Route("/Them/ThemNhanSu")]
        public IActionResult ThemNhanSu()
        {
            return View();
        }
        [Route("/Them/ThemNhapKho")]
        public IActionResult ThemNhapKho()
        {
            return View();
        }
        [Route("/Them/ThemPhanCongCaLamViec")]
        public IActionResult ThemPhanCongCaLamViec()
        {
            return View();
        }
        [Route("/Them/ThemPhieuDoiTra")]
        public IActionResult ThemPhieuDoiTra()
        {
            return View();
        }
        [Route("/Them/ThemTaiKhoan")]
        public IActionResult ThemTaiKhoan()
        {
            return View();
        }
        [Route("/Them/ThemViTriSanPham")]
        public IActionResult ThemViTriSanPham()
        {
            return View();
        }
    }
}

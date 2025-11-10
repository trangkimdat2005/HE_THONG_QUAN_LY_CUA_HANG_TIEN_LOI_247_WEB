using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyKhoHangController : Controller
    {
        [Route("/QuanLyKhoHang/DanhSachHangTonKho")]
        public IActionResult DanhSachHangTonKho()
        {
            return View();
        }
        [Route("/QuanLyKhoHang/DanhSachPhieuNhap")]
        public IActionResult DanhSachPhieuNhap()
        {
            return View();
        }
        [Route("/QuanLyKhoHang/DanhSachPhieuXuat")]
        public IActionResult DanhSachPhieuXuat()
        {
            return View();
        }
        [Route("/QuanLyKhoHang/DanhSachKiemKe")]
        public IActionResult KiemKeSanPham()
        {
            return View();
        }
        [Route("/QuanLyKhoHang/ViTriSanPham")]
        public IActionResult ViTriSanPham()
        {
            return View();
        }
        [Route("/QuanLyKhoHang/XemChiTietKiemKe")]
        public IActionResult XemChiTietKiemKe()
        {
            return View();
        }
        [Route("/QuanLyKhoHang/XemChiTietPhieuXuat")]
        public IActionResult XemChiTietPhieuXuat()
        {
            return View();
        }
        [Route("/QuanLyKhoHang/XemNhapKho")]
        public IActionResult XemNhapKho()
        {
            return View();
        }
    }
}

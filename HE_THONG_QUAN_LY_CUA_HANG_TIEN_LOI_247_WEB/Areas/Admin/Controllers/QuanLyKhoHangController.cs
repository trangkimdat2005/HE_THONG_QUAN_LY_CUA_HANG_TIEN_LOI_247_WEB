using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyKhoHangController : Controller
    {

        private readonly IQuanLyServices _quanLySevices;

        public QuanLyKhoHangController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/QuanLyKhoHang/DanhSachHangTonKho")]
        public IActionResult DanhSachHangTonKho()
        {
            var lstHangTonKho = _quanLySevices.GetList<TonKho>();

            ViewData["lstHangTonKho"] = lstHangTonKho;

            return View();
        }
        [Route("/QuanLyKhoHang/DanhSachPhieuNhap")]
        public IActionResult DanhSachPhieuNhap()
        {
            var lstPhieuNhap = _quanLySevices.GetList<PhieuNhap>();

            ViewData["lstPhieuNhap"] = lstPhieuNhap;

            return View();
        }
        [Route("/QuanLyKhoHang/DanhSachPhieuXuat")]
        public IActionResult DanhSachPhieuXuat()
        {
            var lstPhieuXuat = _quanLySevices.GetList<PhieuXuat>();

            ViewData["lstPhieuXuat"] = lstPhieuXuat;

            return View();
        }
        [Route("/QuanLyKhoHang/DanhSachKiemKe")]
        public IActionResult KiemKeSanPham()
        {
            var lstKiemKe = _quanLySevices.GetList<KiemKe>();

            ViewData["lstKiemKe"] = lstKiemKe;

            return View();
        }
        [Route("/QuanLyKhoHang/ViTriSanPham")]
        public IActionResult ViTriSanPham()
        {
            var lstSanPhamViTri = _quanLySevices.GetList<SanPhamViTri>();

            ViewData["lstSanPhamViTri"] = lstSanPhamViTri;

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

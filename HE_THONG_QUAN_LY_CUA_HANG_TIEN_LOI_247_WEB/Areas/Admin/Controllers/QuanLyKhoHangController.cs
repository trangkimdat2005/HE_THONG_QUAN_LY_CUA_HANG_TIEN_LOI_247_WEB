using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyKhoHangController : Controller
    {

        private readonly IQuanLyServices _quanLySevices;
        private readonly ITonKhoServices _tonKhoServices;

        public QuanLyKhoHangController(IQuanLyServices quanLySevices, ITonKhoServices tonKhoServices)
        {
            _quanLySevices = quanLySevices;
            _tonKhoServices = tonKhoServices;
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
        [Route("/QuanLyKhoHang/XemChiTietKiemKe/{id}")]
        public IActionResult XemChiTietKiemKe(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("KiemKeSanPham");
            }

            // Chỉ cần nạp KiemKe
            var phieuKiemKe = _tonKhoServices.GetKiemKeById(id);
            if (phieuKiemKe == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phiếu kiểm kê";
                return RedirectToAction("KiemKeSanPham");
            }

            // Gửi chỉ 1 object phieuKiemKe qua View
            ViewData["PhieuKiemKe"] = phieuKiemKe;
            // Bỏ dòng ViewData["ChiTietKiemKe"]

            return View();
        }
        [Route("/QuanLyKhoHang/XemChiTietPhieuXuat/{id}")]
        public IActionResult XemChiTietPhieuXuat(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("DanhSachPhieuXuat");
            }

            var phieuXuat = _tonKhoServices.GetPhieuXuatById(id);
            if (phieuXuat == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phiếu xuất";
                return RedirectToAction("DanhSachPhieuXuat");
            }

            var chiTietList = _tonKhoServices.GetChiTietPhieuXuat(id);

            ViewData["PhieuXuat"] = phieuXuat;
            ViewData["ChiTietPhieuXuat"] = chiTietList;

            return View();
        }
        [Route("/QuanLyKhoHang/XemNhapKho/{id}")]
        public IActionResult XemNhapKho(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("DanhSachPhieuNhap");
            }

            var phieuNhap = _tonKhoServices.GetPhieuNhapById(id);
            if (phieuNhap == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phiếu nhập";
                return RedirectToAction("DanhSachPhieuNhap");
            }

            var chiTietList = _tonKhoServices.GetChiTietPhieuNhap(id);

            ViewData["PhieuNhap"] = phieuNhap;
            ViewData["ChiTietPhieuNhap"] = chiTietList;

            return View();
        }
    }
}

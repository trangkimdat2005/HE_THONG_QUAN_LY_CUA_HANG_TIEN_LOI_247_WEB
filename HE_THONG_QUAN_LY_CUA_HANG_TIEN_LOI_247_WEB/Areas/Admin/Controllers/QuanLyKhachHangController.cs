using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyKhachHangController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyKhachHangController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/QuanLyKhachHang/DanhSachKhachHang")]
        public IActionResult DanhSachKhachHang()
        {
            var lstKhachHang = _quanLySevices.GetList<KhachHang>();
            ViewData["lstKhachHang"] = lstKhachHang;
            return View();
        }

        [Route("/QuanLyKhachHang/LichSuMuaHang")]
        public IActionResult LichSuMuaHang()
        {
            var lstLichSuMuaHang = _quanLySevices.GetList<LichSuMuaHang>();
            ViewData["lstLichSuMuaHang"] = lstLichSuMuaHang;
            return View();
        }

        [Route("/QuanLyKhachHang/TheThanhVien")]
        public IActionResult TheThanhVien()
        {
            var lstTheThanhVien = _quanLySevices.GetList<TheThanhVien>();
            ViewData["lstTheThanhVien"] = lstTheThanhVien;
            return View();
        }
    }
}

using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyBaoMatController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyBaoMatController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/QuanLyBaoMat/DanhSachTaiKhoan")]
        public IActionResult DanhSachTaiKhoan()
        {
            var lstRole= _quanLySevices.GetList<Role>();
            ViewData["lstRole"] = lstRole;
            return View();
        }
        [Route("/QuanLyBaoMat/DanhSachTaiKhoanKhachHang")]
        public IActionResult DanhSachTaiKhoanKhachHang()
        {
            var lstTKKH = _quanLySevices.GetList<TaiKhoanKhachHang>();
            ViewData["lstTKKH"] = lstTKKH;
            return View();
        }
        [Route("/QuanLyBaoMat/DanhSachTaiKhoanNhanVien")]
        public IActionResult DanhSachTaiKhoanNhanVien()
        {
            var lstTKNV = _quanLySevices.GetList<TaiKhoanNhanVien>();
            ViewData["lstTKNV"] = lstTKNV;
            return View();
        }
        [Route("/QuanLyBaoMat/LichSuMuaHang")]
        public IActionResult LichSuMuaHang()
        {
            var lstLSMH = _quanLySevices.GetList<LichSuMuaHang>();
            ViewData["lstLSMH"] = lstLSMH;
            return View();
        }
    }
}
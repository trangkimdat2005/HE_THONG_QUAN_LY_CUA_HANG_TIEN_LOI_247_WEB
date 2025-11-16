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
            var lstPer = _quanLySevices.GetList<Permission>();
            ViewData["lstPer"] = lstPer;
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
        public IActionResult LichSuMuaHang(string KhachHangId,string TaiKhoanid)
        {
            var lstLSMH = _quanLySevices.GetList<LichSuMuaHang>().Where(x=>x.KhachHangId==KhachHangId);
            ViewData["lstLSMH"] = lstLSMH;

            var lstHD = _quanLySevices.GetList<HoaDon>();
            ViewData["lstHD"] = lstHD;

            var lstCTHD = _quanLySevices.GetList<ChiTietHoaDon>();
            ViewData["lstCTHD"] = lstCTHD;

            var lstTTV = _quanLySevices.GetList<TheThanhVien>().Where(x => x.KhachHangId == KhachHangId);
            ViewData["lstTTV"] = lstTTV;

            var tkkh = _quanLySevices.GetList<TaiKhoanKhachHang>()
                        .Where(x => x.KhachHangId == KhachHangId && x.TaiKhoanid == TaiKhoanid)
                        .FirstOrDefault();

            if (tkkh == null)
            {
                return NotFound();
            }
            return View(tkkh);
        }
    }
}
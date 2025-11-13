using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyHoaDonController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyHoaDonController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }
        [Route("/QuanLyHoaDon/DanhSachHoaDon")]
        public IActionResult DanhSachHoaDon()
        {
            var lstHoaDon = _quanLySevices.GetList<HoaDon>();

            ViewData["lstHoaDon"] = lstHoaDon;

            return View();
        }
        [Route("/QuanLyHoaDon/ThanhToanHoaDon")]
        public IActionResult ThanhToanHoaDon()
        {
            var lstThanhToanHoaDon = _quanLySevices.GetList<HoaDon>();

            ViewData["lstThanhToanHoaDon"] = lstThanhToanHoaDon;

            return View();
        }
    }
}

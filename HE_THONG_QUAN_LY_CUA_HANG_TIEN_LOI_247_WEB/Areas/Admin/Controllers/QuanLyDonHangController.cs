using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyDonHangController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyDonHangController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/QuanLyDonHang/DanhSachDonHang")]
        public IActionResult DanhSachDonHang()
        {
            var lstHoaDon = _quanLySevices.GetList<HoaDon>();

            ViewData["lstHoaDon"] = lstHoaDon;

            return View();
        }

        [Route("/QuanLyDonHang/XemChiTietHoaDon")]
        public IActionResult XemChiTietHoaDon(string id)
        {
            var hoaDon = _quanLySevices.GetById<HoaDon>(id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            ViewData["HoaDon"] = hoaDon;

            return View();
        }
    }
}

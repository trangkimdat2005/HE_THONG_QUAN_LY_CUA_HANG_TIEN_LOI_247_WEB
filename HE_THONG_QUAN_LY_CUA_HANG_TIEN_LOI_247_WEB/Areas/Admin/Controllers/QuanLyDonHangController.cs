using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyDonHangController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;
        private readonly IGiaoDichThanhToanServices _giaoDichThanhToanServices;

        public QuanLyDonHangController(IQuanLyServices quanLySevices, IGiaoDichThanhToanServices giaoDichThanhToanServices)
        {
            _quanLySevices = quanLySevices;
            _giaoDichThanhToanServices = giaoDichThanhToanServices;
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
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("DanhSachDonHang");
            }

            var hoaDon = _giaoDichThanhToanServices.GetHoaDonById(id);
            if (hoaDon == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hóa đơn";
                return RedirectToAction("DanhSachDonHang");
            }

            var chiTietList = _giaoDichThanhToanServices.GetChiTietHoaDon(id);

            ViewData["HoaDon"] = hoaDon;
            ViewData["ChiTietHoaDon"] = chiTietList;

            return View();
        }
    }
}

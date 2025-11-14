using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GiaoDichHoanTraController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;
        private readonly IGiaoDichThanhToanServices _giaoDichThanhToanServices;
        private readonly IPhieuDoiTraServices _phieuDoiTraServices;
        private readonly IChinhSachHoanTraServices _chinhSachHoanTraServices;
        
        public GiaoDichHoanTraController(
            IQuanLyServices quanLySevices, 
            IGiaoDichThanhToanServices giaoDichThanhToanServices,
            IPhieuDoiTraServices phieuDoiTraServices,
            IChinhSachHoanTraServices chinhSachHoanTraServices)
        {
            _quanLySevices = quanLySevices;
            _giaoDichThanhToanServices = giaoDichThanhToanServices;
            _phieuDoiTraServices = phieuDoiTraServices;
            _chinhSachHoanTraServices = chinhSachHoanTraServices;
        }
        
        [Route("/GiaoDichHoanTra/ChinhSachDoiTra")]
        public IActionResult ChinhSachDoiTra()
        {
            var lstChinhSachDoiTra = _quanLySevices.GetList<ChinhSachHoanTra>();
            
            ViewData["lstChinhSachDoiTra"] = lstChinhSachDoiTra;
            
            return View();
        }
        
        [HttpPost]
        [Route("/GiaoDichHoanTra/DeleteChinhSach/{id}")]
        public IActionResult DeleteChinhSach(string id)
        {
            try
            {
                var result = _chinhSachHoanTraServices.DeleteChinhSach(id);
                
                if (result)
                {
                    return Json(new { success = true, message = "Xóa chính sách thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy chính sách!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        
        [Route("/GiaoDichHoanTra/GiaoDichThanhToan")]
        public IActionResult GiaoDichThanhToan()
        {
            var lstGiaoDichThanhToan = _quanLySevices.GetList<GiaoDichThanhToan>();

            ViewData["lstGiaoDichThanhToan"] = lstGiaoDichThanhToan;
            
            return View();
        }
        
        [Route("/GiaoDichHoanTra/PhieuDoiTra")]
        public IActionResult PhieuDoiTra()
        {
            var lstPhieuDoiTra = _quanLySevices.GetList<PhieuDoiTra>();

            ViewData["lstPhieuDoiTra"] = lstPhieuDoiTra;

            return View();
        }

        [Route("/GiaoDichHoanTra/XemChiTietGiaoDich/{id}")]
        public IActionResult XemChiTietGiaoDich(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("GiaoDichThanhToan");
            }

            var giaoDich = _giaoDichThanhToanServices.GetGiaoDichThanhToanById(id);
            if (giaoDich == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy giao dịch thanh toán";
                return RedirectToAction("GiaoDichThanhToan");
            }

            var hoaDon = _giaoDichThanhToanServices.GetHoaDonById(giaoDich.HoaDonId);
            if (hoaDon == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hóa đơn";
                return RedirectToAction("GiaoDichThanhToan");
            }

            var chiTietList = _giaoDichThanhToanServices.GetChiTietHoaDon(giaoDich.HoaDonId);

            ViewData["GiaoDich"] = giaoDich;
            ViewData["HoaDon"] = hoaDon;
            ViewData["ChiTietHoaDon"] = chiTietList;

            return View();
        }

        [Route("/GiaoDichHoanTra/XemChiTietPhieuDoiTra/{id}")]
        public IActionResult XemChiTietPhieuDoiTra(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("PhieuDoiTra");
            }

            var phieuDoiTra = _phieuDoiTraServices.GetPhieuDoiTraById(id);
            if (phieuDoiTra == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phiếu đổi trả";
                return RedirectToAction("PhieuDoiTra");
            }

            var hoaDon = _phieuDoiTraServices.GetHoaDonById(phieuDoiTra.HoaDonId);
            var chinhSach = _phieuDoiTraServices.GetChinhSachById(phieuDoiTra.ChinhSachId);

            ViewData["PhieuDoiTra"] = phieuDoiTra;
            ViewData["HoaDon"] = hoaDon;
            ViewData["ChinhSach"] = chinhSach;

            return View();
        }
    }
}

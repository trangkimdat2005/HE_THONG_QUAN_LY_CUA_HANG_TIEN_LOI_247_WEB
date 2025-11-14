using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GiaoDichHoanTraController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;
        
        public GiaoDichHoanTraController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }
        
        [Route("/GiaoDichHoanTra/ChinhSachDoiTra")]
        public IActionResult ChinhSachDoiTra()
        {
            var lstChinhSachDoiTra = _quanLySevices.GetList<ChinhSachHoanTra>();
            
            ViewData["lstChinhSachDoiTra"] = lstChinhSachDoiTra;
            
            return View();
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
    }
}

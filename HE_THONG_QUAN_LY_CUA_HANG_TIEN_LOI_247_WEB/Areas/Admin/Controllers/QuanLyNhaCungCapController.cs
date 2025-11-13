using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyNhaCungCapController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyNhaCungCapController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/QuanLyNhaCungCap/DanhSachNhaCungCap")]
        public IActionResult DanhSachNhaCungCap()
        {
            var lstNCC = _quanLySevices.GetList<NhaCungCap>();
            ViewData["lstNCC"] = lstNCC;
            return View();
        }
        [Route("/QuanLyNhaCungCap/LichSuGiaoDich")]
        public IActionResult LichSuGiaoDich()
        {
            var lstLichSuGD = _quanLySevices.GetList<LichSuGiaoDich>();
            ViewData["lstLichSuGD"] = lstLichSuGD;
            return View();
        }
    }
}

using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThanhToanController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public ThanhToanController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/ThanhToan/ThanhToan")]
        public IActionResult ThanhToan()
        {
            var lstKenhThanhToan = _quanLySevices.GetList<KenhThanhToan>();

            ViewData["lstKenhThanhToan"] = lstKenhThanhToan;

            return View();
        }
    }
}

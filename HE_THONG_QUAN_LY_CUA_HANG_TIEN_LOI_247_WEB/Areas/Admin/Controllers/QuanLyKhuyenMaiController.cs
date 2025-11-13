using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyKhuyenMaiController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyKhuyenMaiController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/QuanLyKhuyenMai/KhuyenMai")]
        public IActionResult KhuyenMai()
        {
            var lstKhuyenMai = _quanLySevices.GetList<MaKhuyenMai>();

            ViewData["lstKhuyenMai"] = lstKhuyenMai;

            return View();
        }
    }
}

using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyNhanSuController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyNhanSuController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/QuanLyNhanSu/DanhSachNhanVien")]
        public IActionResult DanhSachNhanVien()
        {
            var lstNhanVien = _quanLySevices.GetList<NhanVien>();
            ViewData["lstNhanVien"] = lstNhanVien;
            return View();
        }
        [Route("/QuanLyNhanSu/LichLamViec")]
        public IActionResult LichLamViec()
        {
            var lstChamCong = _quanLySevices.GetList<ChamCong>();
            ViewData["lstChamCong"] = lstChamCong;
            return View();
        }
        [Route("/QuanLyNhanSu/PhanCongCaLamViec")]
        public IActionResult PhanCongCaLamViec()
        {
            var lstPhanCong = _quanLySevices.GetList<PhanCongCaLamViec>();
            ViewData["lstPhanCong"] = lstPhanCong;
            var lstCaLamViec = _quanLySevices.GetList<CaLamViec>();
            ViewData["lstCaLamViec"] = lstCaLamViec;
            return View();
        }
    }
}

using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyBarCodeController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyBarCodeController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/QuanLyBarCode/DanhSachTemNhan")]
        public IActionResult DanhSachTemNhan()
        {
            var lstTemNhan = _quanLySevices.GetList<TemNhan>();
            var lstMaDinhDanh = _quanLySevices.GetList<MaDinhDanhSanPham>();

            ViewData["lstTemNhan"] = lstTemNhan;
            ViewData["lstMaDinhDanh"] = lstMaDinhDanh;

            return View();
        }

        [Route("/QuanLyBarCode/DinhDanhSanPham")]
        public IActionResult DinhDanhSanPham()
        {
            var lstMaDinhDanh = _quanLySevices.GetList<MaDinhDanhSanPham>();
            var lstSanPhamDonVi = _quanLySevices.GetList<SanPhamDonVi>();

            ViewData["lstMaDinhDanh"] = lstMaDinhDanh;
            ViewData["lstSanPhamDonVi"] = lstSanPhamDonVi;

            return View();
        }
    }
}

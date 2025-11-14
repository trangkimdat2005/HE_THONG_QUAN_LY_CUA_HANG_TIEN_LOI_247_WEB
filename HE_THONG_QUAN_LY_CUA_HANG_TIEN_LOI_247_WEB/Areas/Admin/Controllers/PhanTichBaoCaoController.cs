using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhanTichBaoCaoController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;
        
        public PhanTichBaoCaoController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/PhanTichBaoCao/BaoCaoBanChay")]
        public IActionResult BaoCaoBanChay()
        {
            var lstBaoCaoBanChay = _quanLySevices.GetList<BaoCaoBanChay>();
            
            ViewData["lstBaoCaoBanChay"] = lstBaoCaoBanChay;

            return View();
        }

        [Route("/PhanTichBaoCao/BaoCaoDoanhThu")]
        public IActionResult BaoCaoDoanhThu()
        {
            var lstBaoCaoDoanhThu = _quanLySevices.GetList<BaoCaoDoanhThu>();

            ViewData["lstBaoCaoDoanhThu"] = lstBaoCaoDoanhThu;

            return View();
        }

        [Route("/PhanTichBaoCao/BaoCaoTonKho")]
        public IActionResult BaoCaoTonKho()
        {
            var lstBaoCaoTonKho = _quanLySevices.GetList<BaoCaoTonKho>();

            ViewData["lstBaoCaoTonKho"] = lstBaoCaoTonKho;

            return View();
        }
    }
}

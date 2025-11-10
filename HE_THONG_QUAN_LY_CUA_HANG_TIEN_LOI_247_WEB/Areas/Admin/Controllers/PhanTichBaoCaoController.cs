using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhanTichBaoCaoController : Controller
    {
        [Route("/PhanTichBaoCao/BaoCaoBanChay")]
        public IActionResult BaoCaoBanChay()
        {
            return View();
        }
        [Route("/PhanTichBaoCao/BaoCaoDoanhThu")]
        public IActionResult BaoCaoDoanhThu()
        {
            return View();
        }
        [Route("/PhanTichBaoCao/BaoCaoTonKho")]
        public IActionResult BaoCaoTonKho()
        {
            return View();
        }
    }
}

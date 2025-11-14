using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SuaController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public SuaController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/Sua/SuaHoaDon")] 
        public IActionResult SuaHoaDon()
        {
            return View();
        }
        [Route("/Sua/SuaHoanTra")]
        public IActionResult SuaHoanTra()
        {
            return View();
        }
        [Route("/Sua/SuaKhachHang")]
        public IActionResult SuaKhachHang(string id)
        {
            var lstTTV = _quanLySevices.GetList<TheThanhVien>();
            ViewData["lstTTV"] = lstTTV;
            // Lấy thông tin khách hàng từ cơ sở dữ liệu bằng Id
            var khachHang = _quanLySevices.GetList<KhachHang>().FirstOrDefault(kh => kh.Id == $"{id}");
            // Nếu không tìm thấy khách hàng, trả về lỗi
            if (khachHang == null)
            {
                return NotFound();
            }
            // Trả về view sửa thông tin khách hàng với dữ liệu
            return View(khachHang);
        }
        [Route("/Sua/SuaMaKhuyenMai")]
        public IActionResult SuaMaKhuyenMai()
        {
            return View();
        }
        [Route("/Sua/SuaNCC")]
        public IActionResult SuaNCC()
        {
            return View();
        }
        [Route("/Sua/SuaNhanSu")]
        public IActionResult SuaNhanSu()
        {
            return View();
        }
        [Route("/Sua/SuaPhanCongCaLamViec")]
        public IActionResult SuaPhanCongCaLamViec()
        {
            return View();
        }
        [Route("/Sua/SuaTaiKhoan")]
        public IActionResult SuaTaiKhoan()
        {
            return View();
        }
    }
}

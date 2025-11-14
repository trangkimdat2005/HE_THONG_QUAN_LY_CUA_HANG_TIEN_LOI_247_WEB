using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [ApiController]
    [Route("API")]
    [Area("Admin")]
    public class QuanLyHangHoaController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyHangHoaController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/QuanLyHangHoa/DanhSachDanhMuc")]
        public IActionResult DanhSachDanhMuc()
        {
            var lstDanhMuc = _quanLySevices.GetList<DanhMuc>();

            ViewData["lstDanhMuc"] = lstDanhMuc;
            return View();
        }
        [Route("/QuanLyHangHoa/DanhSachDonVi")]
        public IActionResult DanhSachDonVi()
        {
            var lstDonViDoLuong = _quanLySevices.GetList<DonViDoLuong>();
            
            ViewData["lstDonViDoLuong"] = lstDonViDoLuong;
            
            return View();
        }
        
        [Route("/QuanLyHangHoa/DanhSachNhanHieu")]
        public IActionResult DanhSachNhanHieu()
        {
            var lstNhanHieu = _quanLySevices.GetList<NhanHieu>();
            
            ViewData["lstNhanHieu"] = lstNhanHieu;
            
            return View();
        }
        
        [Route("/QuanLyHangHoa/DanhSachSanPham")]
        public IActionResult DanhSachSanPham()
        {

            var lstSanPham = _quanLySevices.GetList<SanPham>();

            ViewData["lstSanPham"] = lstSanPham;

            return View();
        }
        [Route("/QuanLyHangHoa/LichSuGiaBan")]
        public IActionResult LichSuGiaBan()
        {
            var lstLichSuGiaBan = _quanLySevices.GetList<LichSuGiaBan>();
            
            ViewData["lstLichSuGiaBan"] = lstLichSuGiaBan;
            
            return View();
        }

        [HttpPost("get-next-id-SP")]
        public Task<IActionResult> GetNextId([FromBody] GetNextIdSPRequest request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLySevices.GenerateNewId<SanPham>(request.prefix, request.totalLength) }));
        }
    }
    public class GetNextIdSPRequest
    {
        public string prefix { get; set; }
        public int totalLength { get; set; }
    }

}

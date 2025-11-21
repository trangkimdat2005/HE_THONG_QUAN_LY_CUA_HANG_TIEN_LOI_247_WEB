using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Authorize]
    [ApiController]
    [Route("API")]
    [Area("Admin")]
    public class QuanLyKhuyenMaiController : Controller
    {
        private readonly IQuanLyServices _quanLyServices;
        private readonly IRealtimeNotifier _notifier;

        public QuanLyKhuyenMaiController(IQuanLyServices quanLyServices, IRealtimeNotifier notifier)
        {
            _quanLyServices = quanLyServices;
            _notifier = notifier;
        }

        //=========================================LoadView=========================================================

        [Authorize(Roles = "ADMIN")]
        [Route("/QuanLyKhuyenMai/KhuyenMai")]
        public IActionResult KhuyenMai()
        {
            var lstChuongTrinhKhuyenMai = _quanLyServices.GetList<ChuongTrinhKhuyenMai>();
            ViewData["lstKhuyenMai"] = lstChuongTrinhKhuyenMai;
            return View();
        }

        //=========================================DeleteData=======================================================================
        [HttpDelete("delete-CTKM/{id}")]
        public async Task<IActionResult> DeleteChuongTrinhKhuyenMai(string id)
        {
            try
            {
                var chuongTrinh = _quanLyServices.GetById<ChuongTrinhKhuyenMai>(id);
                if (chuongTrinh == null)
                {
                    return NotFound(new { message = "Không tìm thấy chương trình khuyến mãi." });
                }

                if (_quanLyServices.SoftDelete<ChuongTrinhKhuyenMai>(chuongTrinh))
                {
                    await _notifier.NotifyReloadAsync("ChuongTrinhKhuyenMai");
                    return Ok(new { message = "Xóa chương trình khuyến mãi thành công!" });
                }

                return BadRequest(new { message = "Lỗi khi xóa chương trình khuyến mãi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi xóa chương trình khuyến mãi: {ex.Message}" });
            }
        }

        //=========================================GetDataById=======================================================================
        [HttpGet("get-CTKM-by-id")]
        public async Task<IActionResult> GetChuongTrinhKhuyenMaiById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { message = "Id không hợp lệ" });

            var chuongTrinh = _quanLyServices.GetById<ChuongTrinhKhuyenMai>(id);

            if (chuongTrinh == null)
                return NotFound(new { message = "Không tìm thấy chương trình khuyến mãi" });

            return Ok(new
            {
                id = chuongTrinh.Id,
                ten = chuongTrinh.Ten,
                loai = chuongTrinh.Loai,
                ngayBatDau = chuongTrinh.NgayBatDau,
                ngayKetThuc = chuongTrinh.NgayKetThuc,
                moTa = chuongTrinh.MoTa
            });
        }

        //=========================================GetAllData=======================================================================
        [HttpGet("get-all-CTKM")]
        public async Task<IActionResult> GetAllChuongTrinhKhuyenMai()
        {
            var lstChuongTrinh = _quanLyServices.GetList<ChuongTrinhKhuyenMai>().Where(ct => ct.IsDelete == false).Select(ct => new
            {
                id = ct.Id,
                ten = ct.Ten,
                loai = ct.Loai,
                ngayBatDau = ct.NgayBatDau,
                ngayKetThuc = ct.NgayKetThuc,
                moTa = ct.MoTa,
                soMaKhuyenMai = ct.MaKhuyenMais.Count(m => m.IsDelete == false)
            }).ToList();

            return Ok(lstChuongTrinh);
        }
    }
}

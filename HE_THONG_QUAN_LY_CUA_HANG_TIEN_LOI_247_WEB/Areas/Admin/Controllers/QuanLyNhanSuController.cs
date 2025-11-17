using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels;
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
        [HttpDelete]
        [Route("/API/NhanVien/Delete/{id}")]
        public IActionResult DeleteNhanVien([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "ID nhân viên không hợp lệ." });
            }

            var nhanVien = _quanLySevices.GetById<NhanVien>(id);

            if (nhanVien == null)
            {
                return NotFound(new { message = $"Không tìm thấy nhân viên (ID: {id}) hoặc đã bị xóa." });
            }

            if (_quanLySevices.HardDelete<NhanVien>(nhanVien))
            {
                return Ok(new { message = $"Đã xoá vĩnh viễn nhân viên '{nhanVien.HoTen}'." });
            }

            if (_quanLySevices.SoftDelete<NhanVien>(nhanVien))
            {
                return Ok(new
                {
                    message = $"Đã xoá mềm nhân viên '{nhanVien.HoTen}'."
                });
            }

            return BadRequest(new { message = "Lỗi: Không thể thực hiện xóa." });
        }
        [HttpDelete]
        [Route("/API/PhanCong/Delete/{id}")]
        public IActionResult DeletePhanCongCaLamViec([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "ID ca phân công không hợp lệ." });
            }

            try
            {
                var phanCong = _quanLySevices.GetList<PhanCongCaLamViec>()
                                    .FirstOrDefault(x => x.Id == id);

                if (phanCong == null)
                {
                    return NotFound(new { message = $"Không tìm thấy ca phân công (ID: {id}) hoặc đã bị xóa." });
                }

                if (_quanLySevices.HardDelete<PhanCongCaLamViec>(phanCong))
                {
                    return Ok(new { message = "Đã xoá vĩnh viễn ca phân công." });
                }

                if (_quanLySevices.SoftDelete<PhanCongCaLamViec>(phanCong))
                {
                    return Ok(new
                    {
                        message = "Đã xoá mềm ca phân công."
                    });
                }

                return BadRequest(new { message = "Lỗi: Không thể thực hiện xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
    }
}

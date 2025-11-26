using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class QuanLyNhanSuController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyNhanSuController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/QuanLyNhanSu/DanhSachNhanVien")]
        public IActionResult DanhSachNhanVien()
        {
            var lstNhanVien = _quanLySevices.GetList<NhanVien>();
            ViewData["lstNhanVien"] = lstNhanVien;
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/QuanLyNhanSu/LichLamViec")]
        public IActionResult LichLamViec()
        {
            var lstChamCong = _quanLySevices.GetList<ChamCong>();
            ViewData["lstChamCong"] = lstChamCong;
            return View();
        }

        [Authorize(Roles = "ADMIN")]
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
        public async Task<IActionResult> DeleteNhanVien([FromRoute] string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(new { message = "ID nhân viên không hợp lệ." });
                }

                await _quanLySevices.BeginTransactionAsync();

                var nhanVien = _quanLySevices.GetById<NhanVien>(id);

                if (nhanVien == null)
                {
                    await _quanLySevices.RollbackAsync();
                    return NotFound(new { message = $"Không tìm thấy nhân viên (ID: {id}) hoặc đã bị xóa." });
                }

                _quanLySevices.HardDelete<NhanVien>(nhanVien);

                if (!await _quanLySevices.CommitAsync())
                {
                    await _quanLySevices.BeginTransactionAsync();
                    _quanLySevices.SoftDelete<NhanVien>(nhanVien);
                    if (!await _quanLySevices.CommitAsync())
                    {
                        return BadRequest(new { message = "Lỗi: Không thể thực hiện xóa." });
                    }
                    else
                    {
                        return Ok(new
                        {
                            message = $"Đã xoá mềm nhân viên '{nhanVien.HoTen}'."
                        });
                    }
                }
                else
                {
                    return Ok(new { message = $"Đã xoá vĩnh viễn nhân viên '{nhanVien.HoTen}'." });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }

        }
        [HttpDelete]
        [Route("/API/PhanCong/Delete/{id}")]
        public async Task<IActionResult> DeletePhanCongCaLamViec([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "ID ca phân công không hợp lệ." });
            }

            try
            {
                await _quanLySevices.BeginTransactionAsync();
                var phanCong = _quanLySevices.GetList<PhanCongCaLamViec>()
                                    .FirstOrDefault(x => x.Id == id);

                if (phanCong == null)
                {
                    await _quanLySevices.RollbackAsync();
                    return NotFound(new { message = $"Không tìm thấy ca phân công (ID: {id}) hoặc đã bị xóa." });
                }

                _quanLySevices.HardDelete<PhanCongCaLamViec>(phanCong);

                if (!await _quanLySevices.CommitAsync())
                {
                    await _quanLySevices.BeginTransactionAsync();
                    _quanLySevices.SoftDelete<PhanCongCaLamViec>(phanCong);
                    if (!await _quanLySevices.CommitAsync())
                    {
                        return BadRequest(new { message = "Lỗi: Không thể thực hiện xóa." });
                    }
                    else
                    {
                        return Ok(new
                        {
                            message = "Đã xoá mềm ca phân công."
                        });
                    }
                }
                else
                {
                    return Ok(new { message = "Đã xoá vĩnh viễn ca phân công." });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
    }
}

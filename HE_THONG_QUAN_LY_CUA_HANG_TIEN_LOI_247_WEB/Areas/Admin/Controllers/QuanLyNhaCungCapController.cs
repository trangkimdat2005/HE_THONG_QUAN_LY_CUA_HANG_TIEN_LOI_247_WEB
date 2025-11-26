using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Authorize]
    [ApiController]
    [Route("API")]
    [Area("Admin")]
    public class QuanLyNhaCungCapController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyNhaCungCapController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/QuanLyNhaCungCap/DanhSachNhaCungCap")]
        public IActionResult DanhSachNhaCungCap()
        {
            var lstNCC = _quanLySevices.GetList<NhaCungCap>();
            ViewData["lstNCC"] = lstNCC;
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/QuanLyNhaCungCap/LichSuGiaoDich")]
        public IActionResult LichSuGiaoDich()
        {
            var lstLichSuGD = _quanLySevices.GetList<LichSuGiaoDich>();
            ViewData["lstLichSuGD"] = lstLichSuGD;
            return View();
        }

        //Do Minh Khoi lam
        [HttpDelete] // Dùng động từ DELETE
        [Route("/API/NhaCungCap/Delete/{id}")] // Route để nhận ID
        public async Task<IActionResult> DeleteNhaCungCap([FromRoute] string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(new { message = "ID nhà cung cấp không hợp lệ." });
                }
                await _quanLySevices.BeginTransactionAsync();
                // 1. Lấy entity (Service 'GetById' của bạn đã kiểm tra IsDelete)
                var nhaCungCap = _quanLySevices.GetById<NhaCungCap>(id);

                if (nhaCungCap == null)
                {
                    await _quanLySevices.RollbackAsync();
                    // Service GetById trả về null nếu không tìm thấy hoặc đã IsDelete = true
                    return NotFound(new { message = $"Không tìm thấy nhà cung cấp (ID: {id}) hoặc đã bị xóa." });
                }

                _quanLySevices.HardDelete<NhaCungCap>(nhaCungCap);
                if (!await _quanLySevices.CommitAsync())
                {
                    await _quanLySevices.BeginTransactionAsync();
                    _quanLySevices.SoftDelete<NhaCungCap>(nhaCungCap);
                    if (!await _quanLySevices.CommitAsync())
                    {
                        return BadRequest(new { message = "Lỗi: Không thể thực hiện xóa cứng hoặc xóa mềm." });
                    }
                    else
                    {
                        return Ok(new
                        {
                            message = $"Đã xoá mềm nhà cung cấp '{nhaCungCap.Ten}'."
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        message = $"Đã xoá cứng nhà cung cấp '{nhaCungCap.Ten}'."
                    });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = "Lỗi máy chủ: " + ex.Message });
            }
        }
        [HttpGet]
        [Route("GetGoodsReceiptById")]
        public IActionResult GetGoodsReceiptById(string id)
        {
            var lichSuGiaoDich = _quanLySevices.GetById<LichSuGiaoDich>(id);

            var result = new
            {
                lichSuGiaoDich.Id,
                lichSuGiaoDich.NgayGd,
                lichSuGiaoDich.TongTien,
                lichSuGiaoDich.NhaCungCap.Ten,
                lichSuGiaoDich.NhaCungCap.SoDienThoai,
                lichSuGiaoDich.NhaCungCap.Email,
                lichSuGiaoDich.NhaCungCap.DiaChi,
                details = lichSuGiaoDich.ChiTietGiaoDichNccs.Select(ctdd => new
                {
                    tenSanPham = ctdd.SanPhamDonVi.SanPham.Ten,
                    tenDonVi = ctdd.SanPhamDonVi.DonVi.Ten,
                    ctdd.SoLuong,
                    ctdd.DonGia,
                    ctdd.ThanhTien
                }).ToArray()
            };

            return Json(result);
        }
    }
}

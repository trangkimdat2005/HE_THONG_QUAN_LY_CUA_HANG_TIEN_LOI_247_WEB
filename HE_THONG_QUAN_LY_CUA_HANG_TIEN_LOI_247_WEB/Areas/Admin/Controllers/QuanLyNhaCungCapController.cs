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
        public IActionResult DeleteNhaCungCap([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "ID nhà cung cấp không hợp lệ." });
            }

            // 1. Lấy entity (Service 'GetById' của bạn đã kiểm tra IsDelete)
            var nhaCungCap = _quanLySevices.GetById<NhaCungCap>(id);

            if (nhaCungCap == null)
            {
                // Service GetById trả về null nếu không tìm thấy hoặc đã IsDelete = true
                return NotFound(new { message = $"Không tìm thấy nhà cung cấp (ID: {id}) hoặc đã bị xóa." });
            }

            // 2. Thử XÓA CỨNG
            // Service 'HardDelete' của bạn return false nếu có lỗi (đã try-catch)
            if (_quanLySevices.HardDelete<NhaCungCap>(nhaCungCap))
            {
                // Xóa cứng thành công!
                return Ok(new { message = $"Đã xoá cứng nhà cung cấp '{nhaCungCap.Ten}'." });
            }

            // 3. XÓA CỨNG THẤT BẠI (do lỗi ràng buộc khóa ngoại, v.v...)
            // >> Chuyển sang thử XÓA MỀM
            if (_quanLySevices.SoftDelete<NhaCungCap>(nhaCungCap))
            {
                return Ok(new
                {
                    message = $"Đã xoá mềm nhà cung cấp '{nhaCungCap.Ten}'."
                });
            }

            // 4. Cả hai đều thất bại (hiếm, nhưng có thể xảy ra)
            return BadRequest(new { message = "Lỗi: Không thể thực hiện xóa cứng hoặc xóa mềm." });
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

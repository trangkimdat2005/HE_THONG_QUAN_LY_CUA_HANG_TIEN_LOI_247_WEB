using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    public class ShareController : ControllerBase
    {

        private readonly IQuanLyServices _quanLyServices;

        public ShareController(IQuanLyServices quanLyServices)
        {
            _quanLyServices = quanLyServices;
        }

        [HttpGet]
        public IActionResult GetInFUser()
        {
            try
            {
                var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                TaiKhoan taiKhoan = _quanLyServices.GetById<TaiKhoan>(Id);

                var anh = _quanLyServices.ConvertToBase64Image(taiKhoan.TaiKhoanNhanVien.NhanVien.Anh.Anh, taiKhoan.TaiKhoanNhanVien.NhanVien.Anh.TenAnh);

                return Ok(new
                {
                    taiKhoan.TenDangNhap,
                    taiKhoan.TaiKhoanNhanVien.NhanVien.ChucVu,
                    anh
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }

    }
}

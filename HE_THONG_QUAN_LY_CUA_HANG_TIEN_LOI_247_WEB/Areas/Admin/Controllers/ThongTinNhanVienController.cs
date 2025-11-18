using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiController]
    [Route("API")]
    public class ThongTinNhanVienController : Controller
    {
        private readonly IQuanLyServices _quanLyServices;

        public ThongTinNhanVienController(IQuanLyServices quanLyServices)
        {
            _quanLyServices = quanLyServices;
        }

        [Route("/ThongTinNhanVien")]
        public IActionResult ThongTinNhanVien()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = _quanLyServices.GetById<TaiKhoan>(userId);

            ViewData["user"] = user;

            return View();
        }



        [HttpGet("getThongTinNhanVien")]
        public IActionResult GetThongTinNhanVien()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var tk = _quanLyServices.GetById<TaiKhoan>(userId);

                if (tk == null)
                {
                    return NotFound(new { message = "Không tìm thấy thông tin nhân viên." });
                }

                var nhanVien = new
                {
                    tk.Id,
                    tk.TenDangNhap,
                    trangThaiTaiKhoan = tk.TrangThai,
                    tk.TaiKhoanNhanVien.NhanVienId,
                    tk.TaiKhoanNhanVien.NhanVien.HoTen,
                    tk.TaiKhoanNhanVien.NhanVien.ChucVu,
                    tk.TaiKhoanNhanVien.NhanVien.LuongCoBan,
                    tk.TaiKhoanNhanVien.NhanVien.SoDienThoai,
                    tk.TaiKhoanNhanVien.NhanVien.Email,
                    tk.TaiKhoanNhanVien.NhanVien.DiaChi,
                    tk.TaiKhoanNhanVien.NhanVien.NgayVaoLam,
                    trangThaiNhanVien = tk.TaiKhoanNhanVien.NhanVien.TrangThai,
                    tk.TaiKhoanNhanVien.NhanVien.GioiTinh,
                    anh = tk.TaiKhoanNhanVien.NhanVien.Anh != null
                ? _quanLyServices.ConvertToBase64Image(tk.TaiKhoanNhanVien.NhanVien.Anh.Anh, tk.TaiKhoanNhanVien.NhanVien.Anh.TenAnh)
                : null // Nếu không có ảnh, trả về null

                };

                return Ok(nhanVien);
            }
            catch (System.Exception ex)
            {
                // Log lỗi (nếu cần) và trả về lỗi
                return StatusCode(500, new { message = "Đã có lỗi xảy ra khi lấy thông tin.", error = ex.Message });
            }
        }
    }
}

using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Authorize]
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

        [Authorize(Roles = "ADMIN")]
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
                    anhId = tk.TaiKhoanNhanVien.NhanVien.AnhId
                };

                return Ok(nhanVien);
            }
            catch (System.Exception ex)
            {
                // Log lỗi (nếu cần) và trả về lỗi
                return StatusCode(500, new { message = "Đã có lỗi xảy ra khi lấy thông tin.", error = ex.Message });
            }
        }

        [HttpPut("updateThongTinNhanVien")]
        public IActionResult UpdateThongTinNhanVien([FromBody] UpdateNhanVienRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var tk = _quanLyServices.GetById<TaiKhoan>(userId);

                if (tk == null || tk.TaiKhoanNhanVien == null)
                {
                    return NotFound(new { message = "Không tìm thấy thông tin nhân viên." });
                }

                var nhanVien = tk.TaiKhoanNhanVien.NhanVien;


                nhanVien.HoTen = request.HoTen;
                nhanVien.Email = request.Email;
                nhanVien.SoDienThoai = request.SoDienThoai;
                nhanVien.DiaChi = request.DiaChi;
                nhanVien.GioiTinh = request.GioiTinh;

                _quanLyServices.Update(nhanVien);

                return Ok(new { message = "Cập nhật thông tin thành công!" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Đã có lỗi xảy ra khi cập nhật.", error = ex.Message });
            }
        }

        [HttpPost("changePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Không xác định được người dùng." });
                }

                // Validate input
                if (string.IsNullOrEmpty(request.CurrentPassword))
                {
                    return BadRequest(new { message = "Vui lòng nhập mật khẩu hiện tại." });
                }

                if (string.IsNullOrEmpty(request.NewPassword))
                {
                    return BadRequest(new { message = "Vui lòng nhập mật khẩu mới." });
                }

                if (request.NewPassword.Length < 6)
                {
                    return BadRequest(new { message = "Mật khẩu mới phải có ít nhất 6 ký tự." });
                }

                if (request.NewPassword != request.ConfirmPassword)
                {
                    return BadRequest(new { message = "Mật khẩu xác nhận không khớp." });
                }

                // Call service to change password
                _quanLyServices.ChangePassword(userId, request.CurrentPassword, request.NewPassword);

                return Ok(new { message = "Đổi mật khẩu thành công!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("getHinhAnh/{id}")]
        public IActionResult GetHinhAnh(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var hinhAnh = _quanLyServices.GetById<HinhAnh>(id);

                if (hinhAnh == null || hinhAnh.Anh == null || hinhAnh.Anh.Length == 0)
                {
                    return NotFound();
                }

                string contentType = "image/jpeg";
                if (hinhAnh.TenAnh != null)
                {
                    if (hinhAnh.TenAnh.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        contentType = "image/png";
                    }
                    else if (hinhAnh.TenAnh.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                    {
                        contentType = "image/gif";
                    }
                    else if (hinhAnh.TenAnh.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                             hinhAnh.TenAnh.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                    {
                        contentType = "image/jpeg";
                    }
                }

                return File(hinhAnh.Anh, contentType);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("updateAvatar")]
        public async Task<IActionResult> UpdateAvatar([FromForm] IFormFile avatar)
        {
            try
            {

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Không xác định được người dùng." });
                }

                if (avatar == null || avatar.Length == 0)
                {
                    return BadRequest(new { message = "Vui lòng chọn ảnh để upload." });
                }

                if (avatar.Length > 5 * 1024 * 1024)
                {
                    return BadRequest(new { message = "Kích thước ảnh không được vượt quá 5MB." });
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(avatar.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest(new { message = "Chỉ chấp nhận file ảnh định dạng JPG, JPEG, PNG hoặc GIF." });
                }

                var tk = _quanLyServices.GetById<TaiKhoan>(userId);

                if (tk == null || tk.TaiKhoanNhanVien == null)
                {
                    return NotFound(new { message = "Không tìm thấy thông tin nhân viên." });
                }

                var nhanVien = tk.TaiKhoanNhanVien.NhanVien;

                // Convert image to byte array
                var anhData = await _quanLyServices.ConvertImageToByteArray(avatar);

                string oldAnhId = nhanVien.AnhId;
                string newAnhId = null;

                await _quanLyServices.BeginTransactionAsync();

                if (string.IsNullOrEmpty(nhanVien.AnhId))
                {
                    // Tạo mới hình ảnh
                    var newHinhAnh = new HinhAnh
                    {
                        Id = _quanLyServices.GenerateNewId<HinhAnh>("ANH", 7),
                        Anh = anhData,
                        TenAnh = avatar.FileName
                    };

                    _quanLyServices.Add<HinhAnh>(newHinhAnh);


                    newAnhId = newHinhAnh.Id;
                    nhanVien.AnhId = newAnhId;
                    _quanLyServices.Update(nhanVien);
                    if (await _quanLyServices.CommitAsync())
                    {
                        return Ok(new
                        {
                            message = "Cập nhật ảnh đại diện thành công!",
                            anhId = newAnhId,
                            imageUrl = $"/API/getHinhAnh/{newAnhId}"
                        });
                    }
                }
                else
                {
                    // Cập nhật ảnh hiện tại
                    var existingAnh = _quanLyServices.GetById<HinhAnh>(nhanVien.AnhId);
                    if (existingAnh != null)
                    {
                        existingAnh.Anh = anhData;
                        existingAnh.TenAnh = avatar.FileName;
                        _quanLyServices.Update(existingAnh);
                        
                        if (await _quanLyServices.CommitAsync())
                        {
                            return Ok(new
                            {
                                message = "Cập nhật ảnh đại diện thành công!",
                                anhId = nhanVien.AnhId,
                                imageUrl = $"/API/getHinhAnh/{nhanVien.AnhId}?t={DateTime.Now.Ticks}"
                            });
                        }
                    }
                }

                return BadRequest(new { message = "Không thể cập nhật ảnh đại diện." });
            }
            catch (Exception ex)
            {
                await _quanLyServices.RollbackAsync();
                return StatusCode(500, new { message = "Lỗi khi cập nhật ảnh: " + ex.Message });
            }
        }
    }

    public class UpdateNhanVienRequest
    {
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public bool GioiTinh { get; set; }
        public string? AnhBase64 { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels
{
    public class TaiKhoanCreateDto
    {
        [Required]
        public string AccountType { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nhân viên hoặc khách hàng")]
        public string SelectedUserId { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string TenDangNhap { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string XacNhanMatKhau { get; set; }

        [Required]
        public string TrangThai { get; set; }

        public List<string> RoleIds { get; set; } = new List<string>();
    }
}

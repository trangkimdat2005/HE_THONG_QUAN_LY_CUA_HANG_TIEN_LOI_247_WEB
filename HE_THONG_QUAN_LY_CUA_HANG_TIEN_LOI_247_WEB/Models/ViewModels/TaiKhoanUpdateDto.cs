using System.ComponentModel.DataAnnotations;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels
{
    public class TaiKhoanUpdateDto
    {
        [Required(ErrorMessage = "Phải có ID của Tài Khoản để cập nhật")]
        public string TaiKhoanId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public string TrangThai { get; set; }

        public List<string> RoleIds { get; set; } = new List<string>();
    }
}

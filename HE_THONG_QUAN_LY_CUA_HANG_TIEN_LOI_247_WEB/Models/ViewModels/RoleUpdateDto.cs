using System.ComponentModel.DataAnnotations;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels
{
    public class RoleUpdateDto
    {
        [Required(ErrorMessage = "Phải có ID của Role để cập nhật")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Mã vai trò không được để trống")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Tên vai trò không được để trống")]
        public string Ten { get; set; }

        public string MoTa { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public string TrangThai { get; set; }

        public List<string> PermissionIds { get; set; } = new List<string>();
    }
}

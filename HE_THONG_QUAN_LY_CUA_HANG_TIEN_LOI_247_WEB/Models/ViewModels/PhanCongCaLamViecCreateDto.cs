using System.ComponentModel.DataAnnotations;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels
{
    public class PhanCongCaLamViecCreateDto
    {
        [Required(ErrorMessage = "Vui lòng chọn nhân viên")]
        public string NhanVienId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ca làm việc")]
        public string CaLamViecId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày phân công")]
        public DateTime? Ngay { get; set; } // Dùng ? để bắt lỗi "The value '' is invalid"
    }
}

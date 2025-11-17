using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels
{
    public class NhaCungCapDto
    {
        [Required(ErrorMessage = "ID nhà cung cấp không được để trống")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string SoDienThoai { get; set; }

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")] // Kiểm tra định dạng nếu có
        public string Email { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Mã số thuế không được để trống")]
        public string MaSoThue { get; set; }
    }
}

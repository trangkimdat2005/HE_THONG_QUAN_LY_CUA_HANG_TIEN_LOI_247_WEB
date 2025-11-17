using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
public class NhanVienDto
{
    //[Required(ErrorMessage = "Phải có ID để cập nhật")]
    //public string Id { get; set; }

    [Required(ErrorMessage = "Họ tên không được để trống")]
    public string HoTen { get; set; }

    [Required(ErrorMessage = "Chức vụ không được để trống")]
    public string ChucVu { get; set; }

    [Required(ErrorMessage = "Lương cơ bản là bắt buộc")]
    [Range(1.0, double.MaxValue, ErrorMessage = "Lương cơ bản phải lớn hơn 0")]
    public decimal? LuongCoBan{ get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
    public string SoDienThoai { get; set; }

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Địa chỉ không được để trống")]
    public string DiaChi { get; set; }

    [Required(ErrorMessage = "Ngày vào làm không được để trống")]
    public DateTime NgayVaoLam { get; set; }

    [Required(ErrorMessage = "Trạng thái không được để trống")]
    public string TrangThai { get; set; }

    public bool GioiTinh { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn ảnh đại diện")]
    public IFormFile? AnhDaiDien { get; set; }
}
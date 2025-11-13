using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class TaiKhoan
{
    public string Id { get; set; } = null!;

    public string TenDangNhap { get; set; } = null!;

    public string MatKhauHash { get; set; } = null!;

    public string? Email { get; set; }

    public string TrangThai { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual ICollection<NhatKyHoatDong> NhatKyHoatDongs { get; set; } = new List<NhatKyHoatDong>();

    public virtual TaiKhoanKhachHang? TaiKhoanKhachHang { get; set; }

    public virtual TaiKhoanNhanVien? TaiKhoanNhanVien { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

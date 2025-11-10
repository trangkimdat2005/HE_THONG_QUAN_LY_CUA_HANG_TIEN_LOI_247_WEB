using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class KhachHang
{
    public string Id { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string? Email { get; set; }

    public string DiaChi { get; set; } = null!;

    public DateOnly NgayDangKy { get; set; }

    public string TrangThai { get; set; } = null!;

    public bool IsDelete { get; set; }

    public bool GioiTinh { get; set; }

    public string AnhId { get; set; } = null!;

    public virtual HinhAnh Anh { get; set; } = null!;

    public virtual ICollection<DonHangOnline> DonHangOnlines { get; set; } = new List<DonHangOnline>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<LichSuMuaHang> LichSuMuaHangs { get; set; } = new List<LichSuMuaHang>();

    public virtual ICollection<PhieuXuat> PhieuXuats { get; set; } = new List<PhieuXuat>();

    public virtual ICollection<TaiKhoanKhachHang> TaiKhoanKhachHangs { get; set; } = new List<TaiKhoanKhachHang>();

    public virtual ICollection<TheThanhVien> TheThanhViens { get; set; } = new List<TheThanhVien>();
}

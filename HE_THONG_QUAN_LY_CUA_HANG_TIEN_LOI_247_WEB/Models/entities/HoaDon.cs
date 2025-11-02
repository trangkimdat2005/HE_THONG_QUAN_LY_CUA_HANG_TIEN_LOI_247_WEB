using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.entities;

public partial class HoaDon
{
    public string Id { get; set; } = null!;

    public DateTime NgayLap { get; set; }

    public decimal? TongTien { get; set; }

    public string NhanVienId { get; set; } = null!;

    public string KhachHangId { get; set; } = null!;

    public string TrangThai { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ICollection<ChiTietHoaDonKhuyenMai> ChiTietHoaDonKhuyenMais { get; set; } = new List<ChiTietHoaDonKhuyenMai>();

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual ICollection<DonGiaoHang> DonGiaoHangs { get; set; } = new List<DonGiaoHang>();

    public virtual DonHangOnline? DonHangOnline { get; set; }

    public virtual ICollection<GiaoDichThanhToan> GiaoDichThanhToans { get; set; } = new List<GiaoDichThanhToan>();

    public virtual KhachHang KhachHang { get; set; } = null!;

    public virtual ICollection<LichSuMuaHang> LichSuMuaHangs { get; set; } = new List<LichSuMuaHang>();

    public virtual NhanVien NhanVien { get; set; } = null!;

    public virtual ICollection<PhieuDoiTra> PhieuDoiTras { get; set; } = new List<PhieuDoiTra>();
}

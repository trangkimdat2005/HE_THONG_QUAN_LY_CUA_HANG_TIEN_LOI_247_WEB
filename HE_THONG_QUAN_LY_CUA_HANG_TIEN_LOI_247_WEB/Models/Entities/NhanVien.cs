using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class NhanVien
{
    public string Id { get; set; }

    public string HoTen { get; set; }

    public string ChucVu { get; set; }

    public decimal LuongCoBan { get; set; }

    public string SoDienThoai { get; set; }

    public string Email { get; set; }

    public string DiaChi { get; set; }

    public DateTime NgayVaoLam { get; set; }

    public string TrangThai { get; set; }

    public bool IsDelete { get; set; }

    public bool GioiTinh { get; set; }

    public string AnhId { get; set; }

    public virtual HinhAnh Anh { get; set; }

    public virtual ICollection<ChamCong> ChamCongs { get; set; } = new List<ChamCong>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<KiemKe> KiemKes { get; set; } = new List<KiemKe>();

    public virtual ICollection<PhanCongCaLamViec> PhanCongCaLamViecs { get; set; } = new List<PhanCongCaLamViec>();

    public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();

    public virtual ICollection<PhieuXuat> PhieuXuats { get; set; } = new List<PhieuXuat>();

    public virtual ICollection<TaiKhoanNhanVien> TaiKhoanNhanViens { get; set; } = new List<TaiKhoanNhanVien>();
}

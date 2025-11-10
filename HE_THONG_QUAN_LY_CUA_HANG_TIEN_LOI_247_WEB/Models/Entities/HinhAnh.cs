using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class HinhAnh
{
    public string Id { get; set; } = null!;

    public string TenAnh { get; set; } = null!;

    public byte[] Anh { get; set; } = null!;

    public virtual ICollection<AnhSanPhamDonVi> AnhSanPhamDonVis { get; set; } = new List<AnhSanPhamDonVi>();

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();

    public virtual ICollection<Qrcode> Qrcodes { get; set; } = new List<Qrcode>();

    public virtual ICollection<TemNhan> TemNhans { get; set; } = new List<TemNhan>();
}

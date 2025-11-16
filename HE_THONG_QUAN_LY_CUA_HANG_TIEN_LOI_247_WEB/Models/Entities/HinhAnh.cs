using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class HinhAnh
{
    public string Id { get; set; }

    public string TenAnh { get; set; }

    public byte[] Anh { get; set; }

    public virtual ICollection<AnhSanPhamDonVi> AnhSanPhamDonVis { get; set; } = new List<AnhSanPhamDonVi>();

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();

    public virtual ICollection<MaDinhDanhSanPham> MaDinhDanhSanPhams { get; set; } = new List<MaDinhDanhSanPham>();

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();

    public virtual ICollection<TemNhan> TemNhans { get; set; } = new List<TemNhan>();
}

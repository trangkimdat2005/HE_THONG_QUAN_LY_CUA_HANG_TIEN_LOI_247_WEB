using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class SanPhamDonVi
{
    public string SanPhamId { get; set; } = null!;

    public string DonViId { get; set; } = null!;

    public string Id { get; set; } = null!;

    public decimal HeSoQuyDoi { get; set; }

    public decimal GiaBan { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<BaoCaoTonKho> BaoCaoTonKhos { get; set; } = new List<BaoCaoTonKho>();

    public virtual ICollection<ChiTietDonOnline> ChiTietDonOnlines { get; set; } = new List<ChiTietDonOnline>();

    public virtual ICollection<ChiTietGiaoDichNcc> ChiTietGiaoDichNccs { get; set; } = new List<ChiTietGiaoDichNcc>();

    public virtual ICollection<ChiTietHoaDonKhuyenMai> ChiTietHoaDonKhuyenMais { get; set; } = new List<ChiTietHoaDonKhuyenMai>();

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; } = new List<ChiTietPhieuXuat>();

    public virtual DonViDoLuong DonVi { get; set; } = null!;

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual ICollection<KiemKe> KiemKes { get; set; } = new List<KiemKe>();

    public virtual ICollection<MaDinhDanhSanPham> MaDinhDanhSanPhams { get; set; } = new List<MaDinhDanhSanPham>();

    public virtual ICollection<PhieuDoiTra> PhieuDoiTras { get; set; } = new List<PhieuDoiTra>();

    public virtual SanPham SanPham { get; set; } = null!;

    public virtual ICollection<SanPhamViTri> SanPhamViTris { get; set; } = new List<SanPhamViTri>();

    public virtual ICollection<TonKho> TonKhos { get; set; } = new List<TonKho>();
}

using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.entities;

public partial class SanPham
{
    public string Id { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public string NhanHieuId { get; set; } = null!;

    public string MoTa { get; set; } = null!;

    public string TrangThai { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ICollection<BaoCaoBanChay> BaoCaoBanChays { get; set; } = new List<BaoCaoBanChay>();

    public virtual ICollection<DieuKienApDungSanPham> DieuKienApDungSanPhams { get; set; } = new List<DieuKienApDungSanPham>();

    public virtual ICollection<LichSuGiaBan> LichSuGiaBans { get; set; } = new List<LichSuGiaBan>();

    public virtual NhanHieu NhanHieu { get; set; } = null!;

    public virtual ICollection<SanPhamDanhMuc> SanPhamDanhMucs { get; set; } = new List<SanPhamDanhMuc>();

    public virtual ICollection<SanPhamDonVi> SanPhamDonVis { get; set; } = new List<SanPhamDonVi>();
}

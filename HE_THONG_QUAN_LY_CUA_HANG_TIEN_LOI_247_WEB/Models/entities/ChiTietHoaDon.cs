using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.entities;

public partial class ChiTietHoaDon
{
    public string HoaDonId { get; set; } = null!;

    public string SanPhamDonViId { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public decimal? GiamGia { get; set; }

    public bool IsDelete { get; set; }

    public virtual HoaDon HoaDon { get; set; } = null!;

    public virtual SanPhamDonVi SanPhamDonVi { get; set; } = null!;
}

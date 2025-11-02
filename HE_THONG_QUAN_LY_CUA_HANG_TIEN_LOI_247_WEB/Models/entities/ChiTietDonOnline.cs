using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.entities;

public partial class ChiTietDonOnline
{
    public string DonHangId { get; set; } = null!;

    public string SanPhamDonViId { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public string? Id { get; set; }

    public bool IsDelete { get; set; }

    public virtual DonHangOnline DonHang { get; set; } = null!;

    public virtual SanPhamDonVi SanPhamDonVi { get; set; } = null!;
}

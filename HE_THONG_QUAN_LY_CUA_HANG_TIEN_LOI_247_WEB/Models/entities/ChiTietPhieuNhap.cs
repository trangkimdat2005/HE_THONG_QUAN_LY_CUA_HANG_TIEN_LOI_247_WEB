using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class ChiTietPhieuNhap
{
    public string PhieuNhapId { get; set; } = null!;

    public string SanPhamDonViId { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public DateOnly HanSuDung { get; set; }

    public bool IsDelete { get; set; }

    public virtual PhieuNhap PhieuNhap { get; set; } = null!;

    public virtual SanPhamDonVi SanPhamDonVi { get; set; } = null!;
}

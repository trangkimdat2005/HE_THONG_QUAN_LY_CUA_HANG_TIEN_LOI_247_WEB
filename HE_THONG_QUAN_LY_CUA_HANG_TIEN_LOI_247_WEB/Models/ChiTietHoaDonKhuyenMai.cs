using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class ChiTietHoaDonKhuyenMai
{
    public string HoaDonId { get; set; } = null!;

    public string SanPhamDonViId { get; set; } = null!;

    public string MaKhuyenMaiId { get; set; } = null!;

    public decimal GiaTriApDung { get; set; }

    public string Id { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual HoaDon HoaDon { get; set; } = null!;

    public virtual MaKhuyenMai MaKhuyenMai { get; set; } = null!;

    public virtual SanPhamDonVi SanPhamDonVi { get; set; } = null!;
}

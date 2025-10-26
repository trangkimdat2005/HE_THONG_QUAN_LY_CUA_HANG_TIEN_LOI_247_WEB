using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class PhieuDoiTra
{
    public string Id { get; set; } = null!;

    public string HoaDonId { get; set; } = null!;

    public string SanPhamDonViId { get; set; } = null!;

    public DateOnly NgayDoiTra { get; set; }

    public string LyDo { get; set; } = null!;

    public decimal SoTienHoan { get; set; }

    public string ChinhSachId { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ChinhSachHoanTra ChinhSach { get; set; } = null!;

    public virtual HoaDon HoaDon { get; set; } = null!;

    public virtual SanPhamDonVi SanPhamDonVi { get; set; } = null!;
}

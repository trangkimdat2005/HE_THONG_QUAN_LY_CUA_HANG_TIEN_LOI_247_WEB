using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class GioHang
{
    public string TaiKhoanId { get; set; } = null!;

    public string SanPhamDonViId { get; set; } = null!;

    public int SoLuong { get; set; }

    public bool IsDelete { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual SanPhamDonVi SanPhamDonVi { get; set; } = null!;

    public virtual TaiKhoan TaiKhoan { get; set; } = null!;
}

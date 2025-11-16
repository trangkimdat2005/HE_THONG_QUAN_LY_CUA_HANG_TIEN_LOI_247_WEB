using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class ChiTietHoaDonKhuyenMai
{
    public string HoaDonId { get; set; }

    public string SanPhamDonViId { get; set; }

    public string MaKhuyenMaiId { get; set; }

    public decimal GiaTriApDung { get; set; }

    public string Id { get; set; }

    public bool IsDelete { get; set; }

    public virtual HoaDon HoaDon { get; set; }

    public virtual MaKhuyenMai MaKhuyenMai { get; set; }

    public virtual SanPhamDonVi SanPhamDonVi { get; set; }
}

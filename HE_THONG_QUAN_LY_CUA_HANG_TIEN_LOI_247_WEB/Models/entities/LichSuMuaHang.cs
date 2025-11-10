using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class LichSuMuaHang
{
    public string KhachHangId { get; set; } = null!;

    public string HoaDonId { get; set; } = null!;

    public decimal TongTien { get; set; }

    public DateOnly NgayMua { get; set; }

    public bool IsDelete { get; set; }

    public virtual HoaDon HoaDon { get; set; } = null!;

    public virtual KhachHang KhachHang { get; set; } = null!;
}

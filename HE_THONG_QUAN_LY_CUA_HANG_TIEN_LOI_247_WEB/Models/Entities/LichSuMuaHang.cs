using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class LichSuMuaHang
{
    public string KhachHangId { get; set; }

    public string HoaDonId { get; set; }

    public decimal TongTien { get; set; }

    public DateTime NgayMua { get; set; }

    public bool IsDelete { get; set; }

    public virtual HoaDon HoaDon { get; set; }

    public virtual KhachHang KhachHang { get; set; }
}

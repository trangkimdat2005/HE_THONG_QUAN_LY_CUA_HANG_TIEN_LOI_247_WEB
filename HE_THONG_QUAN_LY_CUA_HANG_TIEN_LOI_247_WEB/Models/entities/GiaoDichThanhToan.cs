using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class GiaoDichThanhToan
{
    public string Id { get; set; } = null!;

    public string HoaDonId { get; set; } = null!;

    public decimal SoTien { get; set; }

    public DateOnly NgayGd { get; set; }

    public string KenhThanhToanId { get; set; } = null!;

    public string? MoTa { get; set; }

    public bool IsDelete { get; set; }

    public virtual HoaDon HoaDon { get; set; } = null!;

    public virtual KenhThanhToan KenhThanhToan { get; set; } = null!;
}

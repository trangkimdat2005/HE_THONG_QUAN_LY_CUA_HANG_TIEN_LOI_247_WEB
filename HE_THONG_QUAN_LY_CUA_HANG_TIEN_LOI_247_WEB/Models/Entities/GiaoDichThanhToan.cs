using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class GiaoDichThanhToan
{
    public string Id { get; set; }

    public string HoaDonId { get; set; }

    public decimal SoTien { get; set; }

    public DateTime NgayGd { get; set; }

    public string KenhThanhToanId { get; set; }

    public string MoTa { get; set; }

    public bool IsDelete { get; set; }

    public virtual HoaDon HoaDon { get; set; }

    public virtual KenhThanhToan KenhThanhToan { get; set; }
}

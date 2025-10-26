using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class CaLamViec
{
    public string Id { get; set; } = null!;

    public string TenCa { get; set; } = null!;

    public TimeOnly ThoiGianBatDau { get; set; }

    public TimeOnly ThoiGianKetThuc { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<PhanCongCaLamViec> PhanCongCaLamViecs { get; set; } = new List<PhanCongCaLamViec>();
}

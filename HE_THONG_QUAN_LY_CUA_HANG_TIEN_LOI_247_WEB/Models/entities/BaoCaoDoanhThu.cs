using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class BaoCaoDoanhThu
{
    public string Id { get; set; } = null!;

    public string BaoCaoId { get; set; } = null!;

    public decimal TongDoanhThu { get; set; }

    public string KyBaoCao { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual BaoCao BaoCao { get; set; } = null!;
}

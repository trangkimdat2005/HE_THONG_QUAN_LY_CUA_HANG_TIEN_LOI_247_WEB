using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class BaoCaoBanChay
{
    public string BaoCaoId { get; set; } = null!;

    public string SanPhamId { get; set; } = null!;

    public int SoLuongBan { get; set; }

    public string? Id { get; set; }

    public bool IsDelete { get; set; }

    public virtual BaoCao BaoCao { get; set; } = null!;

    public virtual SanPham SanPham { get; set; } = null!;
}

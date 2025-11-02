using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.entities;

public partial class TonKho
{
    public string Id { get; set; } = null!;

    public string SanPhamDonViId { get; set; } = null!;

    public int SoLuongTon { get; set; }

    public bool IsDelete { get; set; }

    public virtual SanPhamDonVi SanPhamDonVi { get; set; } = null!;
}

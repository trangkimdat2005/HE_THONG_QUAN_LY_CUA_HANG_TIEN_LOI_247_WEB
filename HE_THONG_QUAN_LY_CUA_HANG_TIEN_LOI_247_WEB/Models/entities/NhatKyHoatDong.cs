using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.entities;

public partial class NhatKyHoatDong
{
    public string Id { get; set; } = null!;

    public string TaiKhoanId { get; set; } = null!;

    public DateTime ThoiGian { get; set; }

    public string HanhDong { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual TaiKhoan TaiKhoan { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class NhatKyHoatDong
{
    public string Id { get; set; }

    public string TaiKhoanId { get; set; }

    public DateTime ThoiGian { get; set; }

    public string HanhDong { get; set; }

    public bool IsDelete { get; set; }

    public virtual TaiKhoan TaiKhoan { get; set; }
}

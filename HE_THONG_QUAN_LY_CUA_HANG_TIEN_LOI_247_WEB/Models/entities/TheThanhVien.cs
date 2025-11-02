using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.entities;

public partial class TheThanhVien
{
    public string Id { get; set; } = null!;

    public string KhachHangId { get; set; } = null!;

    public string Hang { get; set; } = null!;

    public int DiemTichLuy { get; set; }

    public DateOnly NgayCap { get; set; }

    public bool IsDelete { get; set; }

    public virtual KhachHang KhachHang { get; set; } = null!;
}

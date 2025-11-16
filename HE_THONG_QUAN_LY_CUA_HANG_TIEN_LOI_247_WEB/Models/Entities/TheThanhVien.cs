using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class TheThanhVien
{
    public string Id { get; set; }

    public string KhachHangId { get; set; }

    public string Hang { get; set; }

    public int DiemTichLuy { get; set; }

    public DateTime NgayCap { get; set; }

    public bool IsDelete { get; set; }

    public virtual KhachHang KhachHang { get; set; }
}

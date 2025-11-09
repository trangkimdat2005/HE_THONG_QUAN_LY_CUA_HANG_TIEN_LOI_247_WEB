using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class LichSuGiaBan
{
    public string Id { get; set; } = null!;

    public string SanPhamId { get; set; } = null!;

    public string DonViId { get; set; } = null!;

    public decimal GiaBan { get; set; }

    public DateOnly NgayBatDau { get; set; }

    public DateOnly NgayKetThuc { get; set; }

    public bool IsDelete { get; set; }

    public virtual DonViDoLuong DonVi { get; set; } = null!;

    public virtual SanPham SanPham { get; set; } = null!;
}

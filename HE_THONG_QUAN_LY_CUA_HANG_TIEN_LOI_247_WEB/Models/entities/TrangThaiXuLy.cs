using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class TrangThaiXuLy
{
    public string Id { get; set; } = null!;

    public string DonHangId { get; set; } = null!;

    public string TrangThai { get; set; } = null!;

    public DateTime NgayCapNhat { get; set; }

    public bool IsDelete { get; set; }

    public virtual DonHangOnline DonHang { get; set; } = null!;
}

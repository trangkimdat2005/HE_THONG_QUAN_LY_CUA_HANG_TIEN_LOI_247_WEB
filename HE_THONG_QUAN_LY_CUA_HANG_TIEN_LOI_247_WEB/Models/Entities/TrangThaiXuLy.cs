using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class TrangThaiXuLy
{
    public string Id { get; set; }

    public string DonHangId { get; set; }

    public string TrangThai { get; set; }

    public DateTime NgayCapNhat { get; set; }

    public bool IsDelete { get; set; }

    public virtual DonHangOnline DonHang { get; set; }
}

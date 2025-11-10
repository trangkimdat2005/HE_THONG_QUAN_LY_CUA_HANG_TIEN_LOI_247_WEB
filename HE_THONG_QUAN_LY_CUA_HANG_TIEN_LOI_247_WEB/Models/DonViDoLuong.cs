using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class DonViDoLuong
{
    public string Id { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public string KyHieu { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ICollection<LichSuGiaBan> LichSuGiaBans { get; set; } = new List<LichSuGiaBan>();

    public virtual ICollection<SanPhamDonVi> SanPhamDonVis { get; set; } = new List<SanPhamDonVi>();
}

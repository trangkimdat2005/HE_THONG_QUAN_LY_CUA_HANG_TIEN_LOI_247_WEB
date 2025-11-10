using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class PhiVanChuyen
{
    public string Id { get; set; } = null!;

    public decimal SoTien { get; set; }

    public string PhuongThucTinh { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ICollection<DonGiaoHang> DonGiaoHangs { get; set; } = new List<DonGiaoHang>();
}

using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class Shipper
{
    public string Id { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ICollection<DonGiaoHang> DonGiaoHangs { get; set; } = new List<DonGiaoHang>();
}

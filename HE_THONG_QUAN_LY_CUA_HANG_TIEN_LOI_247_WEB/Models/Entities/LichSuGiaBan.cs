using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class LichSuGiaBan
{
    public string Id { get; set; }

    public string SanPhamId { get; set; }

    public string DonViId { get; set; }

    public decimal GiaBan { get; set; }

    public DateTime NgayBatDau { get; set; }

    public DateTime NgayKetThuc { get; set; }

    public bool IsDelete { get; set; }

    public virtual DonViDoLuong DonVi { get; set; }

    public virtual SanPham SanPham { get; set; }
}

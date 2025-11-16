using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class TrangThaiGiaoHang
{
    public string Id { get; set; }

    public string DonGiaoHangId { get; set; }

    public string TrangThai { get; set; }

    public DateTime NgayCapNhat { get; set; }

    public string GhiChu { get; set; }

    public bool IsDelete { get; set; }

    public virtual DonGiaoHang DonGiaoHang { get; set; }
}

using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class PhieuDoiTra
{
    public string Id { get; set; }

    public string HoaDonId { get; set; }

    public string SanPhamDonViId { get; set; }

    public DateTime NgayDoiTra { get; set; }

    public string LyDo { get; set; }

    public decimal SoTienHoan { get; set; }

    public string ChinhSachId { get; set; }

    public bool IsDelete { get; set; }

    public virtual ChinhSachHoanTra ChinhSach { get; set; }

    public virtual HoaDon HoaDon { get; set; }

    public virtual SanPhamDonVi SanPhamDonVi { get; set; }
}

using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class BaoCaoTonKho
{
    public string Id { get; set; }

    public string BaoCaoId { get; set; }

    public bool IsDelete { get; set; }

    public string SanPhamDonViId { get; set; }

    public int TonDauKy { get; set; }

    public int NhapTrongKy { get; set; }

    public int XuatTrongKy { get; set; }

    public int TonCuoiKy { get; set; }

    public virtual BaoCao BaoCao { get; set; }

    public virtual SanPhamDonVi SanPhamDonVi { get; set; }
}

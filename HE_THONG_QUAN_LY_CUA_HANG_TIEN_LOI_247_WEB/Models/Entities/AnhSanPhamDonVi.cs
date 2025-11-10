using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class AnhSanPhamDonVi
{
    public string SanPhamDonViId { get; set; } = null!;

    public string AnhId { get; set; } = null!;

    public bool? IsDelete { get; set; }

    public virtual HinhAnh Anh { get; set; } = null!;

    public virtual SanPhamDonVi SanPhamDonVi { get; set; } = null!;
}

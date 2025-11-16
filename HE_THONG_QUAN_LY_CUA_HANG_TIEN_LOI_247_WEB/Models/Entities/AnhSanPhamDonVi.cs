using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class AnhSanPhamDonVi
{
    public string SanPhamDonViId { get; set; }

    public string AnhId { get; set; }

    public bool? IsDelete { get; set; }

    public virtual HinhAnh Anh { get; set; }

    public virtual SanPhamDonVi SanPhamDonVi { get; set; }
}

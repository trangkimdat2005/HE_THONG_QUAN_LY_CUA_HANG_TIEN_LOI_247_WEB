using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class SanPhamViTri
{
    public string SanPhamDonViId { get; set; } = null!;

    public string ViTriId { get; set; } = null!;

    public int SoLuong { get; set; }

    public string? Id { get; set; }

    public bool IsDelete { get; set; }

    public virtual SanPhamDonVi SanPhamDonVi { get; set; } = null!;

    public virtual ViTri ViTri { get; set; } = null!;
}

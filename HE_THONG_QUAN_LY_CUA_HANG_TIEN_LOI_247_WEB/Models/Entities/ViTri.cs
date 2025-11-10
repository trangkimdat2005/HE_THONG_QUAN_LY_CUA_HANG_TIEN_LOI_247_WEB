using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class ViTri
{
    public string Id { get; set; } = null!;

    public string MaViTri { get; set; } = null!;

    public string LoaiViTri { get; set; } = null!;

    public string? MoTa { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<SanPhamViTri> SanPhamViTris { get; set; } = new List<SanPhamViTri>();
}

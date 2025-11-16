using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class ViTri
{
    public string Id { get; set; }

    public string MaViTri { get; set; }

    public string LoaiViTri { get; set; }

    public string MoTa { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<SanPhamViTri> SanPhamViTris { get; set; } = new List<SanPhamViTri>();
}

using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class DieuKienApDungSanPham
{
    public string Id { get; set; } = null!;

    public string DieuKienId { get; set; } = null!;

    public string SanPhamId { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual DieuKienApDung DieuKien { get; set; } = null!;

    public virtual SanPham SanPham { get; set; } = null!;
}

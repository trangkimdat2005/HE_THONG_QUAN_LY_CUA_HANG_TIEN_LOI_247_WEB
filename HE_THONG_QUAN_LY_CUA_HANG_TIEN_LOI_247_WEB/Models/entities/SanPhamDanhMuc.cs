using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.entities;

public partial class SanPhamDanhMuc
{
    public string SanPhamId { get; set; } = null!;

    public string DanhMucId { get; set; } = null!;

    public string Id { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual DanhMuc DanhMuc { get; set; } = null!;

    public virtual SanPham SanPham { get; set; } = null!;
}

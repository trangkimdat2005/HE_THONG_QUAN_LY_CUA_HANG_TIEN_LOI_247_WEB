using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class SanPhamDanhMuc
{
    public string SanPhamId { get; set; }

    public string DanhMucId { get; set; }

    public string Id { get; set; }

    public bool IsDelete { get; set; }

    public virtual DanhMuc DanhMuc { get; set; }

    public virtual SanPham SanPham { get; set; }
}

using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class DieuKienApDungSanPham
{
    public string Id { get; set; }

    public string DieuKienId { get; set; }

    public string SanPhamId { get; set; }

    public bool IsDelete { get; set; }

    public virtual DieuKienApDung DieuKien { get; set; }

    public virtual SanPham SanPham { get; set; }
}

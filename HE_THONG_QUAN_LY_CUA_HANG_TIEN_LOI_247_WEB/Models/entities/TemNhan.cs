using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class TemNhan
{
    public string Id { get; set; } = null!;

    public string MaDinhDanhId { get; set; } = null!;

    public string NoiDungTem { get; set; } = null!;

    public DateOnly NgayIn { get; set; }

    public bool IsDelete { get; set; }

    public virtual MaDinhDanhSanPham MaDinhDanh { get; set; } = null!;
}

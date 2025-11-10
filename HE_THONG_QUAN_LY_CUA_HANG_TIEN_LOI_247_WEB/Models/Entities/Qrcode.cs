using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class Qrcode
{
    public string Id { get; set; } = null!;

    public string MaDinhDanhId { get; set; } = null!;

    public bool IsDelete { get; set; }

    public string AnhId { get; set; } = null!;

    public virtual HinhAnh Anh { get; set; } = null!;

    public virtual MaDinhDanhSanPham MaDinhDanh { get; set; } = null!;
}

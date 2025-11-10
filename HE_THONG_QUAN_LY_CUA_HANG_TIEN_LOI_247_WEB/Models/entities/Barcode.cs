using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class Barcode
{
    public string Id { get; set; } = null!;

    public string MaDinhDanhId { get; set; } = null!;

    public string BarcodeImage { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual MaDinhDanhSanPham MaDinhDanh { get; set; } = null!;
}

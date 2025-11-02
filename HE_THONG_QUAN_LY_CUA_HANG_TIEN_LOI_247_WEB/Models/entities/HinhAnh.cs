using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.entities;

public partial class HinhAnh
{
    public string Id { get; set; } = null!;

    public string TenAnh { get; set; } = null!;

    public byte[] Anh { get; set; } = null!;
}

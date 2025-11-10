using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class ChinhSachHoanTraDanhMuc
{
    public string ChinhSachId { get; set; } = null!;

    public string DanhMucId { get; set; } = null!;

    public string? Id { get; set; }

    public bool IsDelete { get; set; }

    public virtual ChinhSachHoanTra ChinhSach { get; set; } = null!;

    public virtual DanhMuc DanhMuc { get; set; } = null!;
}

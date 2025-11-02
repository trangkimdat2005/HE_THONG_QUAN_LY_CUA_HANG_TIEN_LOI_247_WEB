using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.entities;

public partial class ChamCong
{
    public string Id { get; set; } = null!;

    public string NhanVienId { get; set; } = null!;

    public DateOnly Ngay { get; set; }

    public DateTime GioVao { get; set; }

    public DateTime GioRa { get; set; }

    public string GhiChu { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual NhanVien NhanVien { get; set; } = null!;
}

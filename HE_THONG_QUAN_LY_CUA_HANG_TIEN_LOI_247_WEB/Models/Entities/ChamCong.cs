using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class ChamCong
{
    public string Id { get; set; }

    public string NhanVienId { get; set; }

    public DateTime Ngay { get; set; }

    public TimeOnly GioVao { get; set; }

    public TimeOnly GioRa { get; set; }

    public string GhiChu { get; set; }

    public bool IsDelete { get; set; }

    public virtual NhanVien NhanVien { get; set; }
}

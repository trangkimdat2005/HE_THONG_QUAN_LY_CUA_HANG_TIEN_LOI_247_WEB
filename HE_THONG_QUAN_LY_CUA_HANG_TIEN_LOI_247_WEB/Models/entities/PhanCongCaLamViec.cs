using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class PhanCongCaLamViec
{
    public string NhanVienId { get; set; } = null!;

    public string CaLamViecId { get; set; } = null!;

    public DateOnly Ngay { get; set; }

    public string? Id { get; set; }

    public bool IsDelete { get; set; }

    public virtual CaLamViec CaLamViec { get; set; } = null!;

    public virtual NhanVien NhanVien { get; set; } = null!;
}

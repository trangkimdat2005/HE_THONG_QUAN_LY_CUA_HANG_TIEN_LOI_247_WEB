using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class KiemKe
{
    public string Id { get; set; } = null!;

    public DateTime NgayKiemKe { get; set; }

    public string KetQua { get; set; } = null!;

    public string NhanVienId { get; set; } = null!;

    public bool IsDelete { get; set; }

    public string? SanPhamDonViId { get; set; }

    public virtual NhanVien NhanVien { get; set; } = null!;

    public virtual SanPhamDonVi? SanPhamDonVi { get; set; }
}

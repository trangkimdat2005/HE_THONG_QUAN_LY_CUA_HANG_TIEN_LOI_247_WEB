using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class KiemKe
{
    public string Id { get; set; }

    public DateTime NgayKiemKe { get; set; }

    public string KetQua { get; set; }

    public string NhanVienId { get; set; }

    public bool IsDelete { get; set; }

    public string SanPhamDonViId { get; set; }

    public virtual NhanVien NhanVien { get; set; }

    public virtual SanPhamDonVi SanPhamDonVi { get; set; }
}

using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class TaiKhoanNhanVien
{
    public string NhanVienId { get; set; } = null!;

    public string TaiKhoanId { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual NhanVien NhanVien { get; set; } = null!;

    public virtual TaiKhoan TaiKhoan { get; set; } = null!;
}

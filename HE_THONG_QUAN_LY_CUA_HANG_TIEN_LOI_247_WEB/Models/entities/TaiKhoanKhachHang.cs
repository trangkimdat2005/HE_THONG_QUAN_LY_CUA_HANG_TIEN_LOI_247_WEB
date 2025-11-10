using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class TaiKhoanKhachHang
{
    public string KhachHangId { get; set; } = null!;

    public string TaiKhoanid { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual KhachHang KhachHang { get; set; } = null!;

    public virtual TaiKhoan TaiKhoan { get; set; } = null!;
}

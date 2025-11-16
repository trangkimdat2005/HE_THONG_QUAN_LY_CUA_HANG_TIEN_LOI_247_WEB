using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class UserRole
{
    public string TaiKhoanId { get; set; }

    public string RoleId { get; set; }

    public DateTime? HieuLucTu { get; set; }

    public DateTime? HieuLucDen { get; set; }

    public string Id { get; set; }

    public bool IsDelete { get; set; }

    public virtual Role Role { get; set; }

    public virtual TaiKhoan TaiKhoan { get; set; }
}

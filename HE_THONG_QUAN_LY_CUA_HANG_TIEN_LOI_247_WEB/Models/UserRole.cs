﻿using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class UserRole
{
    public string TaiKhoanId { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public DateOnly? HieuLucTu { get; set; }

    public DateOnly? HieuLucDen { get; set; }

    public string? Id { get; set; }

    public bool IsDelete { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual TaiKhoan TaiKhoan { get; set; } = null!;
}

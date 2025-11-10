using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class Role
{
    public string Id { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public string? MoTa { get; set; }

    public string TrangThai { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class Permission
{
    public string Id { get; set; }

    public string Code { get; set; }

    public string Ten { get; set; }

    public string MoTa { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

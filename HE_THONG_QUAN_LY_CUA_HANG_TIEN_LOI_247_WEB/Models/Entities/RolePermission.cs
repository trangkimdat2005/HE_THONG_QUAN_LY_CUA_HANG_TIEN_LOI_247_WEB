using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class RolePermission
{
    public string RoleId { get; set; }

    public string PermissionId { get; set; }

    public string Id { get; set; }

    public bool IsDelete { get; set; }

    public virtual Permission Permission { get; set; }

    public virtual Role Role { get; set; }
}

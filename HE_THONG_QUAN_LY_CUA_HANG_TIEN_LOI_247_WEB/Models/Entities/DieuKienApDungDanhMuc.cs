using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class DieuKienApDungDanhMuc
{
    public string Id { get; set; }

    public string DieuKienId { get; set; }

    public string DanhMucId { get; set; }

    public bool IsDelete { get; set; }

    public virtual DanhMuc DanhMuc { get; set; }

    public virtual DieuKienApDung DieuKien { get; set; }
}

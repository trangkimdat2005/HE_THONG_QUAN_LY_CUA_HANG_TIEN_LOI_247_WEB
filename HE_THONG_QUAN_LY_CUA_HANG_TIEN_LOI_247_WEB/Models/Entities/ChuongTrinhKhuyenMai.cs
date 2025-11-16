using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class ChuongTrinhKhuyenMai
{
    public string Id { get; set; }

    public string Ten { get; set; }

    public string Loai { get; set; }

    public DateTime NgayBatDau { get; set; }

    public DateTime NgayKetThuc { get; set; }

    public string MoTa { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<DieuKienApDung> DieuKienApDungs { get; set; } = new List<DieuKienApDung>();

    public virtual ICollection<MaKhuyenMai> MaKhuyenMais { get; set; } = new List<MaKhuyenMai>();
}

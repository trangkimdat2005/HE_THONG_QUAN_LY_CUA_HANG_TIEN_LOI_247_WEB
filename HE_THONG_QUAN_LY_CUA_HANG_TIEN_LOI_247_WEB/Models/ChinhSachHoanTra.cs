using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class ChinhSachHoanTra
{
    public string Id { get; set; } = null!;

    public string TenChinhSach { get; set; } = null!;

    public int ThoiHan { get; set; }

    public string DieuKien { get; set; } = null!;

    public bool ApDungToanBo { get; set; }

    public DateOnly ApDungTuNgay { get; set; }

    public DateOnly ApDungDenNgay { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<ChinhSachHoanTraDanhMuc> ChinhSachHoanTraDanhMucs { get; set; } = new List<ChinhSachHoanTraDanhMuc>();

    public virtual ICollection<PhieuDoiTra> PhieuDoiTras { get; set; } = new List<PhieuDoiTra>();
}

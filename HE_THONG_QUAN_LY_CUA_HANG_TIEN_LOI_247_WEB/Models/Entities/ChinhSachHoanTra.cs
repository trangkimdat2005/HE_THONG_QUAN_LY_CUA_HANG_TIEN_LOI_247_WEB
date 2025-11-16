using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class ChinhSachHoanTra
{
    public string Id { get; set; }

    public string TenChinhSach { get; set; }

    public int? ThoiHan { get; set; }

    public string DieuKien { get; set; }

    public bool? ApDungToanBo { get; set; }

    public DateTime ApDungTuNgay { get; set; }

    public DateTime ApDungDenNgay { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<ChinhSachHoanTraDanhMuc> ChinhSachHoanTraDanhMucs { get; set; } = new List<ChinhSachHoanTraDanhMuc>();

    public virtual ICollection<PhieuDoiTra> PhieuDoiTras { get; set; } = new List<PhieuDoiTra>();
}

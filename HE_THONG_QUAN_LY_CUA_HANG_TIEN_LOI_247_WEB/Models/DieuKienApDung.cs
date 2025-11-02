using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class DieuKienApDung
{
    public string Id { get; set; } = null!;

    public string ChuongTrinhId { get; set; } = null!;

    public string DieuKien { get; set; } = null!;

    public decimal GiaTriToiThieu { get; set; }

    public string GiamTheo { get; set; } = null!;

    public decimal GiaTriToiDa { get; set; }

    public bool IsDelete { get; set; }

    public virtual ChuongTrinhKhuyenMai ChuongTrinh { get; set; } = null!;

    public virtual ICollection<DieuKienApDungDanhMuc> DieuKienApDungDanhMucs { get; set; } = new List<DieuKienApDungDanhMuc>();

    public virtual ICollection<DieuKienApDungSanPham> DieuKienApDungSanPhams { get; set; } = new List<DieuKienApDungSanPham>();

    public virtual ICollection<DieuKienApDungToanBo> DieuKienApDungToanBos { get; set; } = new List<DieuKienApDungToanBo>();
}

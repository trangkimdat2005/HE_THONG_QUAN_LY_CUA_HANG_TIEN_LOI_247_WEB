using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class DieuKienApDung
{
    public string Id { get; set; }

    public string ChuongTrinhId { get; set; }

    public string DieuKien { get; set; }

    public decimal GiaTriToiThieu { get; set; }

    public string GiamTheo { get; set; }

    public decimal GiaTriToiDa { get; set; }

    public bool IsDelete { get; set; }

    public virtual ChuongTrinhKhuyenMai ChuongTrinh { get; set; }

    public virtual ICollection<DieuKienApDungDanhMuc> DieuKienApDungDanhMucs { get; set; } = new List<DieuKienApDungDanhMuc>();

    public virtual ICollection<DieuKienApDungSanPham> DieuKienApDungSanPhams { get; set; } = new List<DieuKienApDungSanPham>();

    public virtual ICollection<DieuKienApDungToanBo> DieuKienApDungToanBos { get; set; } = new List<DieuKienApDungToanBo>();
}

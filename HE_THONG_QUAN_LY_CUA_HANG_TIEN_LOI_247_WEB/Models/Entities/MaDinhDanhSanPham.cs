using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class MaDinhDanhSanPham
{
    public string Id { get; set; }

    public string SanPhamDonViId { get; set; }

    public string LoaiMa { get; set; }

    public string MaCode { get; set; }

    public string DuongDan { get; set; }

    public bool IsDelete { get; set; }

    public string AnhId { get; set; }

    public virtual HinhAnh Anh { get; set; }

    public virtual SanPhamDonVi SanPhamDonVi { get; set; }

    public virtual ICollection<TemNhan> TemNhans { get; set; } = new List<TemNhan>();
}

using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class TemNhan
{
    public string Id { get; set; }

    public string MaDinhDanhId { get; set; }

    public string NoiDungTem { get; set; }

    public DateTime NgayIn { get; set; }

    public bool IsDelete { get; set; }

    public string AnhId { get; set; }

    public virtual HinhAnh Anh { get; set; }

    public virtual MaDinhDanhSanPham MaDinhDanh { get; set; }
}

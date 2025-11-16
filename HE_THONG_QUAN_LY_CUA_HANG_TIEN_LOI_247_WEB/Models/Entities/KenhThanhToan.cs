using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class KenhThanhToan
{
    public string Id { get; set; }

    public string TenKenh { get; set; }

    public string LoaiKenh { get; set; }

    public decimal PhiGiaoDich { get; set; }

    public string TrangThai { get; set; }

    public string CauHinh { get; set; }

    public DateTime NgayTao { get; set; }

    public DateTime NgayCapNhat { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<GiaoDichThanhToan> GiaoDichThanhToans { get; set; } = new List<GiaoDichThanhToan>();
}

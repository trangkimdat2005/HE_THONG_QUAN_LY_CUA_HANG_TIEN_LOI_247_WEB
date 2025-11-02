using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.entities;

public partial class KenhThanhToan
{
    public string Id { get; set; } = null!;

    public string TenKenh { get; set; } = null!;

    public string LoaiKenh { get; set; } = null!;

    public decimal PhiGiaoDich { get; set; }

    public string TrangThai { get; set; } = null!;

    public string? CauHinh { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<GiaoDichThanhToan> GiaoDichThanhToans { get; set; } = new List<GiaoDichThanhToan>();
}

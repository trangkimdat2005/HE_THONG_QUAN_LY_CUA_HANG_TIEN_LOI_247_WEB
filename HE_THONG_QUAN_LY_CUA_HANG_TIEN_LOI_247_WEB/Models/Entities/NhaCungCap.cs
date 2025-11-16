using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class NhaCungCap
{
    public string Id { get; set; }

    public string Ten { get; set; }

    public string SoDienThoai { get; set; }

    public string Email { get; set; }

    public string DiaChi { get; set; }

    public string MaSoThue { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<LichSuGiaoDich> LichSuGiaoDiches { get; set; } = new List<LichSuGiaoDich>();

    public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();
}

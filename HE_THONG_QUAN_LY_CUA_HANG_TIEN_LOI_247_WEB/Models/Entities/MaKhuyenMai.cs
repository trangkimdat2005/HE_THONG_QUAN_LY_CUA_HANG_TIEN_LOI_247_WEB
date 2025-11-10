using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class MaKhuyenMai
{
    public string Id { get; set; } = null!;

    public string ChuongTrinhId { get; set; } = null!;

    public string Code { get; set; } = null!;

    public decimal GiaTri { get; set; }

    public int SoLanSuDung { get; set; }

    public string TrangThai { get; set; } = null!;

    public DateOnly NgayBatDau { get; set; }

    public DateOnly NgayKetThuc { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<ChiTietHoaDonKhuyenMai> ChiTietHoaDonKhuyenMais { get; set; } = new List<ChiTietHoaDonKhuyenMai>();

    public virtual ChuongTrinhKhuyenMai ChuongTrinh { get; set; } = null!;
}

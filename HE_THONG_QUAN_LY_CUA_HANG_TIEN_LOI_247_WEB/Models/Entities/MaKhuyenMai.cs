using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class MaKhuyenMai
{
    public string Id { get; set; }

    public string ChuongTrinhId { get; set; }

    public string Code { get; set; }

    public decimal GiaTri { get; set; }

    public int SoLanSuDung { get; set; }

    public string TrangThai { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<ChiTietHoaDonKhuyenMai> ChiTietHoaDonKhuyenMais { get; set; } = new List<ChiTietHoaDonKhuyenMai>();

    public virtual ChuongTrinhKhuyenMai ChuongTrinh { get; set; }
}

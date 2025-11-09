using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class LichSuGiaoDich
{
    public string Id { get; set; } = null!;

    public string NhaCungCapId { get; set; } = null!;

    public DateOnly NgayGd { get; set; }

    public decimal TongTien { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<ChiTietGiaoDichNcc> ChiTietGiaoDichNccs { get; set; } = new List<ChiTietGiaoDichNcc>();

    public virtual NhaCungCap NhaCungCap { get; set; } = null!;
}

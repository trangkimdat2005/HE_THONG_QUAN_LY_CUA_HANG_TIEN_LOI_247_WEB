using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class DonHangOnline
{
    public string Id { get; set; } = null!;

    public string HoaDonId { get; set; } = null!;

    public string KhachHangId { get; set; } = null!;

    public string KenhDat { get; set; } = null!;

    public DateOnly NgayDat { get; set; }

    public decimal TongTien { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<ChiTietDonOnline> ChiTietDonOnlines { get; set; } = new List<ChiTietDonOnline>();

    public virtual HoaDon HoaDon { get; set; } = null!;

    public virtual KhachHang KhachHang { get; set; } = null!;

    public virtual ICollection<TrangThaiXuLy> TrangThaiXuLies { get; set; } = new List<TrangThaiXuLy>();
}

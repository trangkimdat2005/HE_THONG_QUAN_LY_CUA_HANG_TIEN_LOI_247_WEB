using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class DonGiaoHang
{
    public string Id { get; set; } = null!;

    public string HoaDonId { get; set; } = null!;

    public string ShipperId { get; set; } = null!;

    public string PhiVanChuyenId { get; set; } = null!;

    public DateOnly NgayGiao { get; set; }

    public string TrangThaiHienTai { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual HoaDon HoaDon { get; set; } = null!;

    public virtual PhiVanChuyen PhiVanChuyen { get; set; } = null!;

    public virtual Shipper Shipper { get; set; } = null!;

    public virtual ICollection<TrangThaiGiaoHang> TrangThaiGiaoHangs { get; set; } = new List<TrangThaiGiaoHang>();
}

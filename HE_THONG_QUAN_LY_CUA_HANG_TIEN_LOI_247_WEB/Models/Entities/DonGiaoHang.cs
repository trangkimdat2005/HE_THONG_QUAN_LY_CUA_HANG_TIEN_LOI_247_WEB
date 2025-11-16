using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class DonGiaoHang
{
    public string Id { get; set; }

    public string HoaDonId { get; set; }

    public string ShipperId { get; set; }

    public string PhiVanChuyenId { get; set; }

    public DateTime NgayGiao { get; set; }

    public string TrangThaiHienTai { get; set; }

    public bool IsDelete { get; set; }

    public virtual HoaDon HoaDon { get; set; }

    public virtual PhiVanChuyen PhiVanChuyen { get; set; }

    public virtual Shipper Shipper { get; set; }

    public virtual ICollection<TrangThaiGiaoHang> TrangThaiGiaoHangs { get; set; } = new List<TrangThaiGiaoHang>();
}

using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class ChiTietGiaoDichNcc
{
    public string GiaoDichId { get; set; }

    public string SanPhamDonViId { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public decimal? ThanhTien { get; set; }

    public bool IsDelete { get; set; }

    public virtual LichSuGiaoDich GiaoDich { get; set; }

    public virtual SanPhamDonVi SanPhamDonVi { get; set; }
}

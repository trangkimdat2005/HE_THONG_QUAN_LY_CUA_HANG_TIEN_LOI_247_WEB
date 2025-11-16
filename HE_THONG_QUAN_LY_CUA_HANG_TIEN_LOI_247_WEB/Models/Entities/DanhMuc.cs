using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class DanhMuc
{
    public string Id { get; set; }

    public string Ten { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<ChinhSachHoanTraDanhMuc> ChinhSachHoanTraDanhMucs { get; set; } = new List<ChinhSachHoanTraDanhMuc>();

    public virtual ICollection<DieuKienApDungDanhMuc> DieuKienApDungDanhMucs { get; set; } = new List<DieuKienApDungDanhMuc>();

    public virtual ICollection<SanPhamDanhMuc> SanPhamDanhMucs { get; set; } = new List<SanPhamDanhMuc>();
}

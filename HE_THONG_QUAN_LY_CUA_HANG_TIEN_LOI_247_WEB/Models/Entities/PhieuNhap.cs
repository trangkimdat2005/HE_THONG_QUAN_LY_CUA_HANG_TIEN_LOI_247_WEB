using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class PhieuNhap
{
    public string Id { get; set; }

    public string NhaCungCapId { get; set; }

    public DateTime NgayNhap { get; set; }

    public decimal TongTien { get; set; }

    public bool IsDelete { get; set; }

    public string NhanVienId { get; set; }

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual NhaCungCap NhaCungCap { get; set; }

    public virtual NhanVien NhanVien { get; set; }
}

using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.entities;

public partial class PhieuNhap
{
    public string Id { get; set; } = null!;

    public string NhaCungCapId { get; set; } = null!;

    public DateOnly NgayNhap { get; set; }

    public decimal TongTien { get; set; }

    public bool IsDelete { get; set; }

    public string NhanVienId { get; set; } = null!;

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual NhaCungCap NhaCungCap { get; set; } = null!;

    public virtual NhanVien NhanVien { get; set; } = null!;
}

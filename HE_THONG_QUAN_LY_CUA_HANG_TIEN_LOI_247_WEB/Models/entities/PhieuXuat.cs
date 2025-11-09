using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;

public partial class PhieuXuat
{
    public string Id { get; set; } = null!;

    public string KhachHangId { get; set; } = null!;

    public DateOnly NgayXuat { get; set; }

    public decimal TongTien { get; set; }

    public bool IsDelete { get; set; }

    public string NhanVienId { get; set; } = null!;

    public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; } = new List<ChiTietPhieuXuat>();

    public virtual KhachHang KhachHang { get; set; } = null!;

    public virtual NhanVien NhanVien { get; set; } = null!;
}

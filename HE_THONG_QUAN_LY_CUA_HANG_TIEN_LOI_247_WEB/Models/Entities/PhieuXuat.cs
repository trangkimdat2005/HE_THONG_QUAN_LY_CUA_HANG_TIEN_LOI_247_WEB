using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class PhieuXuat
{
    public string Id { get; set; }

    public string KhachHangId { get; set; }

    public DateTime NgayXuat { get; set; }

    public decimal TongTien { get; set; }

    public bool IsDelete { get; set; }

    public string NhanVienId { get; set; }

    public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; } = new List<ChiTietPhieuXuat>();

    public virtual KhachHang KhachHang { get; set; }

    public virtual NhanVien NhanVien { get; set; }
}

using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class MaDinhDanhSanPham
{
    public string Id { get; set; } = null!;

    public string SanPhamDonViId { get; set; } = null!;

    public string LoaiMa { get; set; } = null!;

    public string MaCode { get; set; } = null!;

    public string DuongDan { get; set; } = null!;

    public bool IsDelete { get; set; }

    public virtual ICollection<Barcode> Barcodes { get; set; } = new List<Barcode>();

    public virtual ICollection<Qrcode> Qrcodes { get; set; } = new List<Qrcode>();

    public virtual SanPhamDonVi SanPhamDonVi { get; set; } = null!;

    public virtual ICollection<TemNhan> TemNhans { get; set; } = new List<TemNhan>();
}

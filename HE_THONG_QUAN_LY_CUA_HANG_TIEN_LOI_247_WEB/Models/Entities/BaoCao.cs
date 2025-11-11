using System;
using System.Collections.Generic;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

public partial class BaoCao
{
    public string Id { get; set; } = null!;

    public string LoaiBaoCao { get; set; } = null!;

    public DateOnly NgayLap { get; set; }

    public bool IsDelete { get; set; }

    public DateOnly TuNgay { get; set; }

    public DateOnly DenNgay { get; set; }

    public virtual ICollection<BaoCaoBanChay> BaoCaoBanChays { get; set; } = new List<BaoCaoBanChay>();

    public virtual ICollection<BaoCaoDoanhThu> BaoCaoDoanhThus { get; set; } = new List<BaoCaoDoanhThu>();

    public virtual ICollection<BaoCaoTonKho> BaoCaoTonKhos { get; set; } = new List<BaoCaoTonKho>();
}

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels
{
    /// <summary>
    /// ViewModel cho th?ng kê t?ng quan Dashboard
    /// </summary>
    public class DashboardStatisticsViewModel
    {
        public int TongKhachHang { get; set; }
        public int TongNhanVien { get; set; }
        public int TongSanPham { get; set; }
        public int TongHoaDon { get; set; }
        public decimal TongDoanhThu { get; set; }
        public int DonChoXuLy { get; set; }
    }
    
    /// <summary>
    /// ViewModel cho s?n ph?m s?p h?t hàng
    /// </summary>
    public class LowStockProductViewModel
    {
        public string SanPhamDonViId { get; set; } = string.Empty;
        public string TenSanPham { get; set; } = string.Empty;
        public string DonVi { get; set; } = string.Empty;
        public int SoLuongTon { get; set; }
    }
    
    /// <summary>
    /// ViewModel cho doanh thu theo ngày
    /// </summary>
    public class DailyRevenueViewModel
    {
        public string Ngay { get; set; } = string.Empty;
        public decimal DoanhThu { get; set; }
    }
    
    /// <summary>
    /// ViewModel cho top s?n ph?m bán ch?y
    /// </summary>
    public class TopProductViewModel
    {
        public string TenSanPham { get; set; } = string.Empty;
        public string DonVi { get; set; } = string.Empty;
        public int SoLuongBan { get; set; }
        public decimal DoanhThu { get; set; }
    }
}

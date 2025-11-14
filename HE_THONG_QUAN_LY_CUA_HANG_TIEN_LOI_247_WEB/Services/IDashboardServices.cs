using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public interface IDashboardServices
    {
        /// <summary>
        /// L?y th?ng kê t?ng quan cho Dashboard
        /// </summary>
        DashboardStatisticsViewModel GetDashboardStatistics();
        
        /// <summary>
        /// L?y danh sách s?n ph?m s?p h?t hàng (t?n kho < ng??ng)
        /// </summary>
        List<LowStockProductViewModel> GetLowStockProducts(int threshold = 10, int limit = 5);
        
        /// <summary>
        /// L?y doanh thu theo kho?ng th?i gian
        /// </summary>
        List<DailyRevenueViewModel> GetDailyRevenue(int days = 7);
        
        /// <summary>
        /// L?y top s?n ph?m bán ch?y
        /// </summary>
        List<TopProductViewModel> GetTopSellingProducts(int days = 7, int limit = 5);
    }
}

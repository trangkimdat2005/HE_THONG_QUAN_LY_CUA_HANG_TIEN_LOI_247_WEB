using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public interface IGiaoDichThanhToanServices
    {
        GiaoDichThanhToan GetGiaoDichThanhToanById(string id);
        HoaDon GetHoaDonById(string id);
        List<ChiTietHoaDon> GetChiTietHoaDon(string hoaDonId);
    }
}

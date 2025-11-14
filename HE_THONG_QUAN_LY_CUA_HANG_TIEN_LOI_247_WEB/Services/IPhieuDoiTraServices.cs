using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public interface IPhieuDoiTraServices
    {
        PhieuDoiTra GetPhieuDoiTraById(string id);
        HoaDon GetHoaDonById(string hoaDonId);
        ChinhSachHoanTra GetChinhSachById(string chinhSachId);
        List<HoaDon> GetAllHoaDons();
        List<ChinhSachHoanTra> GetAllChinhSachs();
        List<SanPhamDonVi> GetSanPhamDonVisByHoaDon(string hoaDonId);
        bool CreatePhieuDoiTra(PhieuDoiTra phieuDoiTra);
        string GeneratePhieuDoiTraId();
    }
}

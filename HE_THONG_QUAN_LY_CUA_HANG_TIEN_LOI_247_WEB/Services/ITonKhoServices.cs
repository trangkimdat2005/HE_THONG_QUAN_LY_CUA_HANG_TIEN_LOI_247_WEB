using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public interface ITonKhoServices
    {
        PhieuNhap GetPhieuNhapById(string id);
        List<ChiTietPhieuNhap> GetChiTietPhieuNhap(string phieuNhapId);
        
        PhieuXuat GetPhieuXuatById(string id);
        List<ChiTietPhieuXuat> GetChiTietPhieuXuat(string phieuXuatId);

        KiemKe GetKiemKeById(string id);
    }
}

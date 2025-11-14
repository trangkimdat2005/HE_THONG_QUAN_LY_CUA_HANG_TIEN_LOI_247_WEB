using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public interface IChinhSachHoanTraServices
    {
        List<ChinhSachHoanTra> GetAllChinhSach();
        ChinhSachHoanTra GetChinhSachById(string id);
        bool CreateChinhSach(ChinhSachHoanTra chinhSach);
        bool UpdateChinhSach(ChinhSachHoanTra chinhSach);
        bool DeleteChinhSach(string id);
        string GenerateChinhSachId();
        List<DanhMuc> GetAllDanhMuc();
        bool AddDanhMucToChinhSach(string chinhSachId, List<string> danhMucIds);
    }
}

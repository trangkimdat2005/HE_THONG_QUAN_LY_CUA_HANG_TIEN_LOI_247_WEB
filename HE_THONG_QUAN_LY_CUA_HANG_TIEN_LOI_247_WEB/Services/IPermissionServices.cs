using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public interface IPermissionServices
    {
        public List<Permission> GetPermissionsForUser(string taiKhoanId);
    }
}

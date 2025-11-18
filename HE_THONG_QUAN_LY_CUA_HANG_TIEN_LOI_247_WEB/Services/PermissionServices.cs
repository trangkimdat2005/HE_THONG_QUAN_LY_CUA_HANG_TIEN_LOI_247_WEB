using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public class PermissionServices : IPermissionServices
    {
        private readonly ApplicationDbContext _context;

        public PermissionServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Permission> GetPermissionsForUser(string taiKhoanId)
        {
            try
            {
                var permissions = (from p in _context.Permissions
                                   join rp in _context.RolePermissions on p.Id equals rp.PermissionId
                                   join ur in _context.UserRoles on rp.RoleId equals ur.RoleId
                                   where ur.TaiKhoanId == taiKhoanId && !p.IsDelete
                                   select p).Distinct().ToList();
                return permissions;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use any logging framework)
                Console.WriteLine($"An error occurred while retrieving permissions: {ex.Message}");
                return new List<Permission>();
            }
        }
    }
}

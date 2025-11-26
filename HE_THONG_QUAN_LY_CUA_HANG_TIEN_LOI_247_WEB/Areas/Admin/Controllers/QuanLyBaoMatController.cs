using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class QuanLyBaoMatController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyBaoMatController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/QuanLyBaoMat/DanhSachTaiKhoan")]
        public IActionResult DanhSachTaiKhoan()
        {
            var lstRole= _quanLySevices.GetList<Role>();
            ViewData["lstRole"] = lstRole;
            var lstPer = _quanLySevices.GetList<Permission>();
            ViewData["lstPermission"] = lstPer;
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/QuanLyBaoMat/DanhSachTaiKhoanKhachHang")]
        public IActionResult DanhSachTaiKhoanKhachHang()
        {
            var lstTKKH = _quanLySevices.GetList<TaiKhoanKhachHang>();
            ViewData["lstTKKH"] = lstTKKH;
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/QuanLyBaoMat/DanhSachTaiKhoanNhanVien")]
        public IActionResult DanhSachTaiKhoanNhanVien()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var lstTKNV = _quanLySevices.GetList<TaiKhoanNhanVien>().Where(t=>t.TaiKhoanId!= userId).ToList();
            ViewData["lstTKNV"] = lstTKNV;
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/QuanLyBaoMat/LichSuMuaHang")]
        public IActionResult LichSuMuaHang(string KhachHangId,string TaiKhoanid)
        {
            var lstLSMH = _quanLySevices.GetList<LichSuMuaHang>().Where(x => x.KhachHangId == KhachHangId).ToList();
            ViewData["lstLSMH"] = lstLSMH;

            var lstHD = _quanLySevices.GetList<HoaDon>().Where(x => x.KhachHangId == KhachHangId).ToList();
            ViewData["lstHD"] = lstHD;

            var lstCTHD = _quanLySevices.GetList<ChiTietHoaDon>().ToList();
            ViewData["lstCTHD"] = lstCTHD;

            var lstTTV = _quanLySevices.GetList<TheThanhVien>().Where(x => x.KhachHangId == KhachHangId).ToList();
            ViewData["lstTTV"] = lstTTV;

            var lstSPDV = _quanLySevices.GetList<SanPhamDonVi>().ToList();
            ViewData["lstSPDV"] = lstSPDV;

            var tkkh = _quanLySevices.GetList<TaiKhoanKhachHang>()
                        .Where(x => x.KhachHangId == KhachHangId && x.TaiKhoanid == TaiKhoanid)
                        .FirstOrDefault();

            if (tkkh == null)
            {
                return NotFound();
            }
            return View(tkkh);
        }
        [HttpPost]
        [Route("/API/TaiKhoan/ToggleLock/{id}")]
        public async Task<IActionResult> ToggleLockState(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "ID tài khoản không hợp lệ." });
            }

            try
            {
                await _quanLySevices.BeginTransactionAsync();
                var taiKhoan = _quanLySevices.GetList<TaiKhoan>()
                                .FirstOrDefault(tk => tk.Id == id && !tk.IsDelete);

                if (taiKhoan == null)
                {
                    await _quanLySevices.RollbackAsync();
                    return NotFound(new { message = "Không tìm thấy tài khoản này." });
                }

                string newStatus;
                if (taiKhoan.TrangThai == "Hoạt động" || taiKhoan.TrangThai == "Active")
                {
                    newStatus = "Đã khoá";
                }
                else
                {
                    newStatus = "Hoạt động";
                }

                taiKhoan.TrangThai = newStatus;
                _quanLySevices.Update<TaiKhoan>(taiKhoan);

                if (await _quanLySevices.CommitAsync())
                {
                    return Ok(new { message = $"Đã cập nhật trạng thái tài khoản '{taiKhoan.TenDangNhap}' thành '{newStatus}'." });
                }
                else
                {
                    await _quanLySevices.RollbackAsync();
                    return BadRequest(new { message = "Lỗi khi cập nhật trạng thái." });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
        [HttpPost]
        [Route("/API/PhanQuyen/ThemRole")]
        public async Task<IActionResult> AddRole([FromBody] RoleCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _quanLySevices.BeginTransactionAsync();
                var existingRole = _quanLySevices.GetList<Role>()
                                    .FirstOrDefault(r => r.Code == dto.Code && !r.IsDelete);

                if (existingRole != null)
                {
                    await _quanLySevices.RollbackAsync();
                    return BadRequest(new { message = $"Mã vai trò '{dto.Code}' đã tồn tại." });
                }

                var newRole = new Role
                {
                    Id = _quanLySevices.GenerateNewId<Role>("ROLE", 5),
                    Code = dto.Code,
                    Ten = dto.Ten,
                    MoTa = dto.MoTa,
                    TrangThai = dto.TrangThai,
                    IsDelete = false
                };

                _quanLySevices.Add<Role>(newRole);

                if (dto.PermissionIds != null && dto.PermissionIds.Any())
                {
                    foreach (var permId in dto.PermissionIds)
                    {
                        var newRolePerm = new RolePermission
                        {
                            Id = _quanLySevices.GenerateNewId<RolePermission>("RP", 7),
                            RoleId = newRole.Id,
                            PermissionId = permId,
                            IsDelete = false
                        };
                        _quanLySevices.Add<RolePermission>(newRolePerm);
                    }
                }

                if (!await _quanLySevices.CommitAsync())
                {
                    return BadRequest(new { message = "Lỗi: Không thể lưu quyền cho vai trò." });
                }

                return Ok(new { message = $"Thêm vai trò '{newRole.Ten}' thành công!" });
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("/API/PhanQuyen/GetPermissionsForRole/{id}")]
        public IActionResult GetPermissionsForRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var permissionIds = _quanLySevices.GetList<RolePermission>()
                .Where(rp => rp.RoleId == id && !rp.IsDelete)
                .Select(rp => rp.PermissionId)
                .ToList();

            return Ok(permissionIds);
        }

        [HttpPut]
        [Route("/API/PhanQuyen/UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _quanLySevices.BeginTransactionAsync();

                var role = _quanLySevices.GetList<Role>()
                                .FirstOrDefault(r => r.Id == dto.Id && !r.IsDelete);

                if (role == null)
                {
                    await _quanLySevices.RollbackAsync();
                    return NotFound(new { message = "Không tìm thấy vai trò để cập nhật." });
                }

                role.Ten = dto.Ten;
                role.MoTa = dto.MoTa;
                role.TrangThai = dto.TrangThai;

                var oldPerms = _quanLySevices.GetList<RolePermission>()
                                    .Where(p => p.RoleId == dto.Id && !p.IsDelete).ToList();

                var newPermIds = dto.PermissionIds ?? new List<string>();

                var permsToRemove = oldPerms.Where(old => !newPermIds.Contains(old.PermissionId)).ToList();
                var permIdsToAdd = newPermIds.Where(newId => !oldPerms.Any(old => old.PermissionId == newId)).ToList();

                foreach (var perm in permsToRemove)
                {
                    _quanLySevices.SoftDelete<RolePermission>(perm);
                }

                foreach (var permId in permIdsToAdd)
                {
                    var newRolePerm = new RolePermission
                    {
                        Id = _quanLySevices.GenerateNewId<RolePermission>("RP", 7),
                        RoleId = role.Id,
                        PermissionId = permId,
                        IsDelete = false
                    };
                    _quanLySevices.Add<RolePermission>(newRolePerm);
                }

                _quanLySevices.Update<Role>(role);

                if (await _quanLySevices.CommitAsync())
                {
                    return Ok(new { message = $"Cập nhật vai trò '{role.Ten}' thành công!" });
                }
                else
                {
                    await _quanLySevices.RollbackAsync();
                    return BadRequest(new { message = "Lỗi khi cập nhật vai trò." });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
        [HttpDelete]
        [Route("/API/PhanQuyen/DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "ID vai trò không hợp lệ." });
            }

            try
            {
                await _quanLySevices.BeginTransactionAsync();
                var role = _quanLySevices.GetList<Role>()
                                .FirstOrDefault(r => r.Id == id && !r.IsDelete);

                if (role == null)
                {
                    await _quanLySevices.RollbackAsync();
                    return NotFound(new { message = $"Không tìm thấy vai trò (ID: {id}) hoặc đã bị xóa." });
                }

                var userRoles = _quanLySevices.GetList<UserRole>()
                                .Where(ur => ur.RoleId == id && !ur.IsDelete).ToList();
                foreach (var ur in userRoles)
                {
                    _quanLySevices.SoftDelete<UserRole>(ur);
                }

                var rolePerms = _quanLySevices.GetList<RolePermission>()
                                .Where(rp => rp.RoleId == id && !rp.IsDelete).ToList();
                foreach (var rp in rolePerms)
                {
                    _quanLySevices.SoftDelete<RolePermission>(rp);
                }

                _quanLySevices.SoftDelete<Role>(role);

                if (await _quanLySevices.CommitAsync())
                {
                    return Ok(new { message = $"Đã xóa vai trò '{role.Ten}' và các liên kết." });
                }
                else
                {
                    await _quanLySevices.RollbackAsync();
                    return BadRequest(new { message = "Lỗi khi xóa vai trò." });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
    }
}
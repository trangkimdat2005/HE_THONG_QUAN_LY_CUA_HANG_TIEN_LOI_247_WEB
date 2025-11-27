using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Authorize]
    [ApiController]
    [Route("API")]
    [Area("Admin")]
    public class ThemController : Controller
    {
        private readonly IPhieuDoiTraServices _phieuDoiTraServices;
        private readonly IChinhSachHoanTraServices _chinhSachHoanTraServices;
        private readonly IQuanLyServices _quanLyServices;
        private readonly IEmailService _emailService;

        public ThemController(
            IPhieuDoiTraServices phieuDoiTraServices,
            IChinhSachHoanTraServices chinhSachHoanTraServices,
            IQuanLyServices quanLyServices,
            IEmailService emailService)
        {
            _phieuDoiTraServices = phieuDoiTraServices;
            _chinhSachHoanTraServices = chinhSachHoanTraServices;
            _quanLyServices = quanLyServices;
            _emailService = emailService;
        }

        [Authorize(Roles = "ADMIN,NV_KHO")]
        [Route("/Them/ThemDanhMucViTRi")]
        public IActionResult ThemDanhMucViTRi()
        {
            ViewData["DanhSachViTri"] = _quanLyServices.GetList<ViTri>();
            return View();
        }

        [Authorize(Roles = "ADMIN,NV_BANHANG")]
        [Route("/Them/ThemHoaDon")]
        public IActionResult ThemHoaDon()
        {
            var lstKhachHang = _quanLyServices.GetList<KhachHang>()
                .Where(kh => !kh.IsDelete && kh.TrangThai == "Hoạt động")
                .OrderBy(kh => kh.HoTen)
                .ToList();

            var lstSanPhamDonVi = _quanLyServices.GetList<SanPhamDonVi>()
                .Where(sp => !sp.IsDelete)
                .ToList();

            foreach (var spDonVi in lstSanPhamDonVi)
            {
                // Load thông tin sản phẩm
                if (!string.IsNullOrEmpty(spDonVi.SanPhamId))
                {
                    spDonVi.SanPham = _quanLyServices.GetById<SanPham>(spDonVi.SanPhamId);

                    // Load danh mục của sản phẩm
                    if (spDonVi.SanPham != null)
                    {
                        var sanPhamDanhMuc = _quanLyServices.GetList<SanPhamDanhMuc>()
                            .Where(spdm => spdm.SanPhamId == spDonVi.SanPhamId && !spdm.IsDelete)
                            .ToList();

                        spDonVi.SanPham.SanPhamDanhMucs = sanPhamDanhMuc;

                        // Load thông tin danh mục
                        foreach (var spdm in sanPhamDanhMuc)
                        {
                            if (!string.IsNullOrEmpty(spdm.DanhMucId))
                            {
                                spdm.DanhMuc = _quanLyServices.GetById<DanhMuc>(spdm.DanhMucId);
                            }
                        }
                    }
                }

                // Load thông tin đơn vị đo lường
                if (!string.IsNullOrEmpty(spDonVi.DonViId))
                {
                    spDonVi.DonVi = _quanLyServices.GetById<DonViDoLuong>(spDonVi.DonViId);
                }
            }

            // Load danh sách danh mục
            var lstDanhMuc = _quanLyServices.GetList<DanhMuc>()
                .Where(dm => !dm.IsDelete)
                .OrderBy(dm => dm.Ten)
                .ToList();

            // Load danh sách mã khuyến mãi đang active
            var lstMaKhuyenMai = _quanLyServices.GetList<MaKhuyenMai>()
                .Where(mkm => !mkm.IsDelete && mkm.TrangThai == "Hoạt động")
                .ToList();

            ViewData["DanhSachKhachHang"] = lstKhachHang;
            ViewData["DanhSachSanPhamDonVi"] = lstSanPhamDonVi;
            ViewData["DanhSachDanhMuc"] = lstDanhMuc;
            ViewData["DanhSachMaKhuyenMai"] = lstMaKhuyenMai;

            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/Them/ThemHoanTra")]
        public IActionResult ThemHoanTra()
        {
            return View();
        }

        [Authorize(Roles = "ADMIN,NV_KHO,NV_BANHANG")]
        [Route("/Them/ThemKiemKe")]
        public IActionResult ThemKiemKe()
        {
            // Load danh sách nhân viên và sản phẩm cho phiếu kiểm kê
            ViewData["DanhSachNhanVien"] = _quanLyServices.GetList<NhanVien>();
            ViewData["DanhSachSanPhamDonVi"] = _quanLyServices.GetList<SanPhamDonVi>();

            return View();
        }

        [Authorize(Roles = "ADMIN,NV_BANHANG")]
        [Route("/Them/ThemKhachHang")]
        public IActionResult ThemKhachHang()
        {
            // Load danh sách hình ảnh (nếu cần)
            ViewData["DanhSachHinhAnh"] = _quanLyServices.GetList<HinhAnh>();

            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/Them/ThemMaKhuyenMai")]
        public IActionResult ThemMaKhuyenMai()
        {
            // Load danh sách danh mục và sản phẩm
            ViewData["DanhMucs"] = _quanLyServices.GetList<DanhMuc>();
            ViewData["SanPhams"] = _quanLyServices.GetList<SanPham>();

            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/Them/ThemNCC")]
        public IActionResult ThemNCC()
        {
            return View();
        }

        [HttpPost]
        [Route("/API/ThemNCC/Add")]
        public async Task<IActionResult> AddNhaCungCap([FromBody] NhaCungCapDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _quanLyServices.BeginTransactionAsync();

                var newId = _quanLyServices.GenerateNewId<NhaCungCap>("NCC", 7);

                if (string.IsNullOrEmpty(newId))
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Không thể tạo ID mới cho nhà cung cấp." });
                }

                var nhaCungCapEntity = new NhaCungCap
                {
                    Id = newId,
                    Ten = data.Ten,
                    SoDienThoai = data.SoDienThoai,
                    Email = data.Email.ToLower(),
                    DiaChi = data.DiaChi,
                    MaSoThue = data.MaSoThue,
                    IsDelete = false
                };

                _quanLyServices.Add<NhaCungCap>(nhaCungCapEntity);

                if (await _quanLyServices.CommitAsync())
                {
                    return Ok(new { message = $"Thêm nhà cung cấp '{data.Ten}' thành công!", newId = newId });
                }
                else
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Lỗi: Không thể lưu nhà cung cấp vào cơ sở dữ liệu." });
                }
            }
            catch (Exception ex)
            {
                await _quanLyServices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }

        [HttpPost("get-next-id-NCC")]
        public Task<IActionResult> GetNextIdNCC([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLyServices.GenerateNewId<NhaCungCap>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/Them/ThemNhanSu")]
        public IActionResult ThemNhanSu()
        {
            return View();
        }
        [HttpPost]
        [Route("/API/NhanVien/Them")]
        // Dùng [FromForm] để nhận DTO từ multipart/form-data
        public async Task<IActionResult> ThemNhanVien([FromForm] NhanVienDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto.AnhDaiDien == null || dto.AnhDaiDien.Length == 0)
            {
                ModelState.AddModelError("AnhDaiDien", "File ảnh không hợp lệ.");
                return BadRequest(ModelState);
            }

            HinhAnh newHinhAnh = null;
            try
            {
                await _quanLyServices.BeginTransactionAsync();
                byte[] anhBytes = await _quanLyServices.ConvertImageToByteArray(dto.AnhDaiDien);

                newHinhAnh = new HinhAnh
                {
                    Id = _quanLyServices.GenerateNewId<HinhAnh>("ANH", 7),
                    TenAnh = dto.AnhDaiDien.FileName,
                    Anh = anhBytes
                };

                _quanLyServices.Add<HinhAnh>(newHinhAnh);

                var nhanVien = new NhanVien
                {
                    Id = _quanLyServices.GenerateNewId<NhanVien>("NV", 6),
                    HoTen = dto.HoTen,
                    ChucVu = dto.ChucVu,
                    LuongCoBan = (decimal)dto.LuongCoBan,
                    SoDienThoai = dto.SoDienThoai,
                    Email = dto.Email.ToLower(),
                    DiaChi = dto.DiaChi,
                    NgayVaoLam = dto.NgayVaoLam,
                    TrangThai = dto.TrangThai,
                    GioiTinh = dto.GioiTinh,
                    AnhId = newHinhAnh.Id, // Gán ID của ảnh vừa lưu
                    IsDelete = false
                };

                _quanLyServices.Add<NhanVien>(nhanVien);

                if (await _quanLyServices.CommitAsync())
                {
                    return Ok(new { message = $"Thêm nhân viên {nhanVien.HoTen} thành công!", newId = nhanVien.Id });
                }
                else
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Lỗi khi lưu thông tin nhân viên." });
                }
            }
            catch (Exception ex)
            {
                await _quanLyServices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }

        [Authorize(Roles = "ADMIN,NV_KHO")]
        [Route("/Them/ThemNhapKho")]
        public IActionResult ThemNhapKho()
        {
            ViewData["DanhSachNhaCungCap"] = _quanLyServices.GetList<NhaCungCap>();
            ViewData["DanhSachNhanVien"] = _quanLyServices.GetList<NhanVien>();
            ViewData["DanhSachSanPhamDonVi"] = _quanLyServices.GetList<SanPhamDonVi>();

            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/Them/ThemPhanCongCaLamViec")]
        public IActionResult ThemPhanCongCaLamViec()
        {
            var lstNhanVien = _quanLyServices.GetList<NhanVien>();
            ViewData["lstNhanVien"] = lstNhanVien;

            var lstCaLamViec = _quanLyServices.GetList<CaLamViec>();
            ViewData["lstCaLamViec"] = lstCaLamViec;
            return View();
        }

        [Authorize(Roles = "ADMIN,NV_BANHANG")]
        [HttpGet]
        [Route("/Them/ThemPhieuDoiTra")]
        public IActionResult ThemPhieuDoiTra()
        {
            ViewData["DanhSachHoaDon"] = _phieuDoiTraServices.GetAllHoaDons();
            ViewData["DanhSachChinhSach"] = _phieuDoiTraServices.GetAllChinhSachs();

            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [Route("/Them/ThemPhieuDoiTra")]
        public IActionResult ThemPhieuDoiTra(PhieuDoiTra phieuDoiTra)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _phieuDoiTraServices.CreatePhieuDoiTra(phieuDoiTra);

                    if (result)
                    {
                        TempData["SuccessMessage"] = "Tạo phiếu đổi trả thành công!";
                        return RedirectToAction("PhieuDoiTra", "GiaoDichHoanTra");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo phiếu đổi trả!";
                    }
                }

                // Nếu validation fail, load lại data
                ViewData["DanhSachHoaDon"] = _phieuDoiTraServices.GetAllHoaDons();
                ViewData["DanhSachChinhSach"] = _phieuDoiTraServices.GetAllChinhSachs();

                return View(phieuDoiTra);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
                ViewData["DanhSachHoaDon"] = _phieuDoiTraServices.GetAllHoaDons();
                ViewData["DanhSachChinhSach"] = _phieuDoiTraServices.GetAllChinhSachs();
                return View(phieuDoiTra);
            }
        }

        [HttpGet]
        [Route("/Them/GetSanPhamByHoaDon/{hoaDonId}")]
        public IActionResult GetSanPhamByHoaDon(string hoaDonId)
        {
            try
            {
                var sanPhams = _phieuDoiTraServices.GetSanPhamDonVisByHoaDon(hoaDonId);
                return Json(sanPhams.Select(sp => new
                {
                    id = sp.Id,
                    ten = sp.SanPham?.Ten,
                    donVi = sp.DonVi?.Ten,
                    giaBan = sp.GiaBan
                }));
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // ==================== CHÍNH SÁCH HOÀN TRẢ ====================

        [Authorize(Roles = "ADMIN,NV_BANHANG")]
        [HttpGet]
        [Route("/Them/ThemChinhSachHoanTra")]
        public IActionResult ThemChinhSachHoanTra()
        {
            ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [Route("/Them/ThemChinhSachHoanTra")]
        public IActionResult ThemChinhSachHoanTra(
            string TenChinhSach,
            int? ThoiHan,
            string DieuKien,
            bool ApDungToanBo,
            DateTime ApDungTuNgay,
            DateTime ApDungDenNgay,
            List<string> DanhMucIds)
        {
            try
            {
                // Validate dữ liệu
                if (string.IsNullOrWhiteSpace(TenChinhSach))
                {
                    TempData["ErrorMessage"] = "Tên chính sách không được để trống!";
                    ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
                    return View();
                }

                if (string.IsNullOrWhiteSpace(DieuKien))
                {
                    TempData["ErrorMessage"] = "Điều kiện áp dụng không được để trống!";
                    ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
                    return View();
                }

                if (ApDungTuNgay >= ApDungDenNgay)
                {
                    TempData["ErrorMessage"] = "Ngày áp dụng đến phải sau ngày áp dụng từ!";
                    ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
                    return View();
                }

                if (!ApDungToanBo && (DanhMucIds == null || !DanhMucIds.Any()))
                {
                    TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một danh mục hoặc chọn 'Áp dụng toàn bộ'!";
                    ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
                    return View();
                }

                // Tạo object ChinhSachHoanTra
                var chinhSach = new ChinhSachHoanTra
                {
                    TenChinhSach = TenChinhSach,
                    ThoiHan = ThoiHan,
                    DieuKien = DieuKien,
                    ApDungToanBo = ApDungToanBo,
                    ApDungTuNgay = ApDungTuNgay,
                    ApDungDenNgay = ApDungDenNgay
                };

                var result = _chinhSachHoanTraServices.CreateChinhSach(chinhSach);

                if (result)
                {
                    // Nếu không áp dụng toàn bộ, tạo các bản ghi ChinhSachHoanTraDanhMuc
                    if (!ApDungToanBo && DanhMucIds != null && DanhMucIds.Any())
                    {
                        _chinhSachHoanTraServices.AddDanhMucToChinhSach(chinhSach.Id, DanhMucIds);
                    }

                    TempData["SuccessMessage"] = "Tạo chính sách hoàn trả thành công!";
                    return RedirectToAction("ChinhSachDoiTra", "GiaoDichHoanTra");
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi tạo chính sách!";
                }

                ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
                ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
                return View();
            }
        }
        [HttpPost]
        [Route("/API/TaiKhoan/Them")]
        public async Task<IActionResult> CreateTaiKhoan([FromBody] TaiKhoanCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _quanLyServices.BeginTransactionAsync();
                var existingUser = _quanLyServices.GetList<TaiKhoan>()
                                    .FirstOrDefault(t => t.TenDangNhap == dto.TenDangNhap && !t.IsDelete);
                if (existingUser != null)
                {
                    await _quanLyServices.RollbackAsync();
                    ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại.");
                    return BadRequest(ModelState);
                }

                var newTaiKhoan = new TaiKhoan
                {
                    Id = _quanLyServices.GenerateNewId<TaiKhoan>("TK", 6),
                    TenDangNhap = dto.TenDangNhap,
                    Email = dto.Email.ToLower(),
                    MatKhauHash = _quanLyServices.HashPassword(dto.MatKhau),
                    TrangThai = dto.TrangThai,
                    IsDelete = false
                };

                _quanLyServices.Add<TaiKhoan>(newTaiKhoan);

                if (dto.AccountType == "NhanVien")
                {
                    var tkNv = new TaiKhoanNhanVien
                    {
                        TaiKhoanId = newTaiKhoan.Id,
                        NhanVienId = dto.SelectedUserId,
                        IsDelete = false
                    };
                    _quanLyServices.Add<TaiKhoanNhanVien>(tkNv);
                }
                else
                {
                    var tkKh = new TaiKhoanKhachHang
                    {
                        TaiKhoanid = newTaiKhoan.Id,
                        KhachHangId = dto.SelectedUserId,
                        IsDelete = false
                    };
                    _quanLyServices.Add<TaiKhoanKhachHang>(tkKh);
                }

                if (dto.RoleIds != null)
                {
                    foreach (var roleId in dto.RoleIds)
                    {
                        var newUserRole = new UserRole
                        {
                            Id = _quanLyServices.GenerateNewId<UserRole>("UR", 7),
                            TaiKhoanId = newTaiKhoan.Id,
                            RoleId = roleId,
                            HieuLucTu = DateTime.Now,
                            IsDelete = false
                        };
                        _quanLyServices.Add<UserRole>(newUserRole);
                    }
                }

                if (!await _quanLyServices.CommitAsync())
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Lỗi khi tạo tài khoản." });
                }

                return Ok(new { message = "Tạo tài khoản thành công!" });
            }
            catch (Exception ex)
            {
                await _quanLyServices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
        [HttpPost]
        [Route("/API/TaiKhoan/ResetPassword/{id}")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "ID tài khoản không hợp lệ." });
            }

            try
            {
                var taiKhoan = _quanLyServices.GetList<TaiKhoan>()
                                .FirstOrDefault(tk => tk.Id == id && !tk.IsDelete);

                if (taiKhoan == null)
                {
                    return NotFound(new { message = "Không tìm thấy tài khoản này." });
                }

                if (string.IsNullOrEmpty(taiKhoan.Email))
                {
                    return BadRequest(new { message = "Tài khoản này chưa cập nhật Email, không thể gửi mật khẩu." });
                }

                string token = _quanLyServices.GenerateRecoveryToken(taiKhoan.Email);

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { message = "Lỗi hệ thống: Không thể tạo token khôi phục." });
                }

                string newPassword = _quanLyServices.GenerateRandomPassword();

                string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(newPassword));
                string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

                var callbackUrl = Url.Action("ConfirmReset", "Account",
                    new { area = "", email = taiKhoan.Email, newPassword = encodedPassword, token = encodedToken }, Request.Scheme);

                await _emailService.SendEmailAsync(taiKhoan.Email, "Admin đã cấp lại mật khẩu cho bạn",
                    $"<h3>Yêu cầu đặt lại mật khẩu từ Quản trị viên</h3>" +
                    $"<p>Tài khoản <b>{taiKhoan.TenDangNhap}</b> vừa được yêu cầu cấp lại mật khẩu.</p>" +
                    $"<p>Mật khẩu mới tạm thời: <strong style='color:red; font-size:18px'>{newPassword}</strong></p>" +
                    $"<p>Link này chỉ có hiệu lực trong vòng <b>5 phút</b> và chỉ dùng được <b>1 lần</b>.</p>" +
                    $"<p>Vui lòng <a href='{callbackUrl}'>BẤM VÀO ĐÂY</a> để kích hoạt mật khẩu này.</p>" +
                    $"<p><i>Lưu ý: Mật khẩu cũ vẫn có hiệu lực cho đến khi bạn bấm link trên.</i></p>");

                return Ok(new { message = $"Đã gửi email xác nhận và mật khẩu mới tới {taiKhoan.Email}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
        [Authorize(Roles = "ADMIN")]
        [Route("/Them/ThemTaiKhoan")]
        public IActionResult ThemTaiKhoan(string loai = "nhanvien")
        {
            var nvIdsDaCoTK = _quanLyServices.GetList<TaiKhoanNhanVien>()
                                .Where(tk => !tk.IsDelete)
                                .Select(tk => tk.NhanVienId);
            ViewData["lstNhanVien"] = _quanLyServices.GetList<NhanVien>()
                                        .Where(nv => !nvIdsDaCoTK.Contains(nv.Id)).ToList();

            var khIdsDaCoTK = _quanLyServices.GetList<TaiKhoanKhachHang>()
                                .Where(tk => !tk.IsDelete)
                                .Select(tk => tk.KhachHangId);
            ViewData["lstKhachHang"] = _quanLyServices.GetList<KhachHang>()
                                        .Where(kh => !khIdsDaCoTK.Contains(kh.Id)).ToList();

            ViewData["lstRole"] = _quanLyServices.GetList<Role>();

            ViewBag.LoaiTaiKhoan = loai.ToLower();

            return View();
        }

        [Authorize(Roles = "ADMIN,NV_KHO")]
        [Route("/Them/ThemViTriSanPham")]
        public IActionResult ThemViTriSanPham()
        {
            // Load danh sách nhân viên, sản phẩm, và vị trí
            ViewData["DanhSachNhanVien"] = _quanLyServices.GetList<NhanVien>();
            ViewData["DanhSachSanPhamDonVi"] = _quanLyServices.GetList<SanPhamDonVi>();
            ViewData["DanhSachViTri"] = _quanLyServices.GetList<ViTri>();

            return View();
        }


        //thêm phiếu nhập
        [Route("/add-PN")]
        public async Task<IActionResult> AddPhieuNhap([FromBody] PhieuNhapFormData request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate dữ liệu đầu vào
                if (string.IsNullOrEmpty(request.NhaCungCapId) ||
                    string.IsNullOrEmpty(request.NhanVienId) ||
                    request.NgayNhap == default(DateTime))
                {
                    return BadRequest(new { message = "Thiếu thông tin nhà cung cấp, nhân viên hoặc ngày nhập." });
                }

                if (request.ChiTietPhieuNhap == null || !request.ChiTietPhieuNhap.Any())
                {
                    return BadRequest(new { message = "Phiếu nhập phải có ít nhất một sản phẩm." });
                }

                // Tạo phiếu nhập
                PhieuNhap phieuNhap = new PhieuNhap
                {
                    Id = _quanLyServices.GenerateNewId<PhieuNhap>("PN", 6),
                    NhaCungCapId = request.NhaCungCapId,
                    NhanVienId = request.NhanVienId,
                    NgayNhap = request.NgayNhap,
                    TongTien = 0, // Sẽ được tính sau
                    IsDelete = false
                };

                await _quanLyServices.BeginTransactionAsync();

                _quanLyServices.Add<PhieuNhap>(phieuNhap);

                decimal tongTien = 0;

                // Thêm chi tiết phiếu nhập
                foreach (var chiTiet in request.ChiTietPhieuNhap)
                {
                    if (string.IsNullOrEmpty(chiTiet.SanPhamDonViId) ||
                        chiTiet.SoLuong <= 0 ||
                        chiTiet.DonGia <= 0)
                    {
                        continue; // Bỏ qua dòng không hợp lệ
                    }

                    decimal thanhTien = chiTiet.SoLuong * chiTiet.DonGia;
                    tongTien += thanhTien;

                    ChiTietPhieuNhap chiTietPhieuNhap = new ChiTietPhieuNhap
                    {
                        PhieuNhapId = phieuNhap.Id,
                        SanPhamDonViId = chiTiet.SanPhamDonViId,
                        SoLuong = chiTiet.SoLuong,
                        DonGia = chiTiet.DonGia,
                        TongTien = thanhTien,
                        HanSuDung = chiTiet.HanSuDung,
                        IsDelete = false
                    };

                    _quanLyServices.Add<ChiTietPhieuNhap>(chiTietPhieuNhap);
                }

                if(!await _quanLyServices.CommitAsync())
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Lỗi khi lưu phiếu nhập." });
                }

                return Ok(new { message = "Thêm phiếu nhập thành công!", phieuNhapId = phieuNhap.Id });
            }
            catch (Exception ex)
            {
                await _quanLyServices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi thêm phiếu nhập: {ex.Message}" });
            }
        }

        //gán vị trí sản phẩm
        [Route("/add-GanViTri")]
        public async Task<IActionResult> AddGanViTriSanPham([FromBody] GanViTriFormData request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate dữ liệu đầu vào
                if (string.IsNullOrEmpty(request.NhanVienId) ||
                    request.NgayThucHien == default(DateTime))
                {
                    return BadRequest(new { message = "Thiếu thông tin nhân viên hoặc ngày thực hiện." });
                }

                if (request.ChiTietGanViTri == null || !request.ChiTietGanViTri.Any())
                {
                    return BadRequest(new { message = "Phải có ít nhất một sản phẩm để gán vị trí." });
                }

                await _quanLyServices.BeginTransactionAsync();

                // Thêm từng chi tiết gán vị trí
                foreach (var chiTiet in request.ChiTietGanViTri)
                {
                    if (string.IsNullOrEmpty(chiTiet.SanPhamDonViId) ||
                        string.IsNullOrEmpty(chiTiet.ViTriId) ||
                        chiTiet.SoLuong <= 0)
                    {
                        continue; // Bỏ qua dòng không hợp lệ
                    }

                    // Kiểm tra xem sản phẩm đã có ở vị trí này chưa
                    var existingSPVT = _quanLyServices.GetList<SanPhamViTri>()
                        .FirstOrDefault(spvt => spvt.SanPhamDonViId == chiTiet.SanPhamDonViId &&
                                               spvt.ViTriId == chiTiet.ViTriId &&
                                               !spvt.IsDelete);

                    if (existingSPVT != null)
                    {
                        // Nếu đã tồn tại, cập nhật số lượng
                        existingSPVT.SoLuong += chiTiet.SoLuong;
                        _quanLyServices.Update<SanPhamViTri>(existingSPVT);
                    }
                    else
                    {
                        // Tạo mới bản ghi SanPhamViTri
                        SanPhamViTri sanPhamViTri = new SanPhamViTri
                        {
                            Id = _quanLyServices.GenerateNewId<SanPhamViTri>("SPVT", 8),
                            SanPhamDonViId = chiTiet.SanPhamDonViId,
                            ViTriId = chiTiet.ViTriId,
                            SoLuong = chiTiet.SoLuong,
                            IsDelete = false
                        };

                        _quanLyServices.Add<SanPhamViTri>(sanPhamViTri);
                    }
                }

                if (!await _quanLyServices.CommitAsync())
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Lỗi khi gán vị trí sản phẩm." });
                }

                return Ok(new { message = "Gán vị trí sản phẩm thành công!" });
            }
            catch (Exception ex)
            {
                await _quanLyServices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi gán vị trí sản phẩm: {ex.Message}" });
            }
        }

        //thêm vị trí
        [HttpPost]
        [Route("/add-ViTri")]
        public async Task<IActionResult> AddViTri([FromBody] ViTri viTri)
        {
            try
            {
                Console.WriteLine("=== ADD VITRI API CALLED ===");
                Console.WriteLine($"Received data: MaViTri={viTri?.MaViTri}, LoaiViTri={viTri?.LoaiViTri}, MoTa={viTri?.MoTa}");

                if (viTri == null)
                {
                    Console.WriteLine("ERROR: viTri is null");
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate dữ liệu
                if (string.IsNullOrWhiteSpace(viTri.MaViTri))
                {
                    Console.WriteLine("ERROR: MaViTri is empty");
                    return BadRequest(new { message = "Mã vị trí không được để trống." });
                }

                if (string.IsNullOrWhiteSpace(viTri.LoaiViTri))
                {
                    Console.WriteLine("ERROR: LoaiViTri is empty");
                    return BadRequest(new { message = "Loại vị trí không được để trống." });
                }

                // Kiểm tra mã vị trí đã tồn tại chưa
                Console.WriteLine("Checking if MaViTri exists...");
                var existingViTri = _quanLyServices.GetList<ViTri>()
                    .FirstOrDefault(vt => vt.MaViTri == viTri.MaViTri && !vt.IsDelete);

                if (existingViTri != null)
                {
                    await _quanLyServices.RollbackAsync();
                    Console.WriteLine($"ERROR: MaViTri '{viTri.MaViTri}' already exists (ID: {existingViTri.Id})");
                    return BadRequest(new { message = $"Mã vị trí '{viTri.MaViTri}' đã tồn tại trong hệ thống." });
                }

                // Tạo ID mới cho vị trí
                Console.WriteLine("Generating new ID...");
                viTri.Id = _quanLyServices.GenerateNewId<ViTri>("VT", 6);
                Console.WriteLine($"Generated ID: {viTri.Id}");

                viTri.IsDelete = false;

                Console.WriteLine("Calling Add service...");
                _quanLyServices.Add<ViTri>(viTri);

                if (!await _quanLyServices.CommitAsync())
                {
                    await _quanLyServices.RollbackAsync();
                    Console.WriteLine("ERROR: Add service returned false");
                    return BadRequest(new { message = "Không thể thêm vị trí mới. Vui lòng kiểm tra kết nối database." });
                }

                Console.WriteLine($"SUCCESS: Added ViTri with ID={viTri.Id}, MaViTri={viTri.MaViTri}");
                return Ok(new { message = "Thêm vị trí mới thành công!", viTriId = viTri.Id });
            }
            catch (Exception ex)
            {
                await _quanLyServices.RollbackAsync();
                Console.WriteLine($" EXCEPTION in AddViTri: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                return StatusCode(500, new { message = $"Lỗi khi thêm vị trí: {ex.Message}" });
            }
        }

        //=========================================API Thêm Chương Trình Khuyến Mãi=======================================================================

        [HttpPost]
        [Route("/API/add-CTKM")]
        public async Task<IActionResult> AddChuongTrinhKhuyenMai([FromBody] ChuongTrinhKhuyenMaiRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate
                if (string.IsNullOrWhiteSpace(request.Ten))
                {
                    return BadRequest(new { message = "Tên chương trình không được để trống." });
                }

                if (string.IsNullOrWhiteSpace(request.Loai))
                {
                    return BadRequest(new { message = "Loại khuyến mãi không được để trống." });
                }

                if (request.NgayKetThuc <= request.NgayBatDau)
                {
                    return BadRequest(new { message = "Ngày kết thúc phải sau ngày bắt đầu." });
                }

                // Tạo Chương trình khuyến mãi
                ChuongTrinhKhuyenMai chuongTrinh = new ChuongTrinhKhuyenMai
                {
                    Id = _quanLyServices.GenerateNewId<ChuongTrinhKhuyenMai>("CTKM", 8),
                    Ten = request.Ten,
                    Loai = request.Loai,
                    NgayBatDau = request.NgayBatDau,
                    NgayKetThuc = request.NgayKetThuc,
                    MoTa = request.MoTa,
                    IsDelete = false
                };

                await _quanLyServices.BeginTransactionAsync();

                _quanLyServices.Add<ChuongTrinhKhuyenMai>(chuongTrinh);

                // Thêm Điều kiện áp dụng
                if (request.DieuKienApDung != null)
                {
                    DieuKienApDung dieuKien = new DieuKienApDung
                    {
                        Id = _quanLyServices.GenerateNewId<DieuKienApDung>("DK", 6),
                        ChuongTrinhId = chuongTrinh.Id,
                        DieuKien = request.DieuKienApDung.DieuKien ?? "",
                        GiaTriToiThieu = request.DieuKienApDung.GiaTriToiThieu,
                        GiamTheo = request.DieuKienApDung.GiamTheo,
                        GiaTriToiDa = request.DieuKienApDung.GiaTriToiDa,
                        IsDelete = false
                    };

                    _quanLyServices.Add<DieuKienApDung>(dieuKien);

                    // Thêm áp dụng theo phạm vi
                    if (request.PhamViApDung == "DanhMuc" && request.DanhMucIds != null && request.DanhMucIds.Any())
                    {
                        foreach (var danhMucId in request.DanhMucIds)
                        {
                            DieuKienApDungDanhMuc dkadDanhMuc = new DieuKienApDungDanhMuc
                            {
                                Id = _quanLyServices.GenerateNewId<DieuKienApDungDanhMuc>("DKNV", 8),
                                DieuKienId = dieuKien.Id,
                                DanhMucId = danhMucId,
                                IsDelete = false
                            };

                            _quanLyServices.Add<DieuKienApDungDanhMuc>(dkadDanhMuc);
                        }
                    }
                    else if (request.PhamViApDung == "SanPham" && request.SanPhamIds != null && request.SanPhamIds.Any())
                    {
                        foreach (var sanPhamId in request.SanPhamIds)
                        {
                            DieuKienApDungSanPham dkadSanPham = new DieuKienApDungSanPham
                            {
                                Id = _quanLyServices.GenerateNewId<DieuKienApDungSanPham>("DKSP", 8),
                                DieuKienId = dieuKien.Id,
                                SanPhamId = sanPhamId,
                                IsDelete = false
                            };

                            _quanLyServices.Add<DieuKienApDungSanPham>(dkadSanPham);
                        }
                    }
                    else if (request.PhamViApDung == "ToanBo")
                    {
                        DieuKienApDungToanBo dkadToanBo = new DieuKienApDungToanBo
                        {
                            Id = _quanLyServices.GenerateNewId<DieuKienApDungToanBo>("DKTB", 8),
                            DieuKienId = dieuKien.Id,
                            GhiChu = "Áp dụng toàn bộ cửa hàng",
                            IsDelete = false
                        };

                        _quanLyServices.Add<DieuKienApDungToanBo>(dkadToanBo);
                    }
                }

                // Thêm Mã khuyến mãi
                if (request.MaKhuyenMai != null)
                {
                    MaKhuyenMai maKhuyenMai = new MaKhuyenMai
                    {
                        Id = _quanLyServices.GenerateNewId<MaKhuyenMai>("MKM", 7),
                        ChuongTrinhId = chuongTrinh.Id,
                        Code = request.MaKhuyenMai.Code,
                        GiaTri = request.MaKhuyenMai.GiaTri,
                        SoLanSuDung = request.MaKhuyenMai.SoLanSuDung,
                        TrangThai = request.MaKhuyenMai.TrangThai,
                        IsDelete = false
                    };

                    _quanLyServices.Add<MaKhuyenMai>(maKhuyenMai);
                }

                if(!await _quanLyServices.CommitAsync())
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Lỗi khi lưu chương trình khuyến mãi." });
                }

                return Ok(new { message = "Thêm chương trình khuyến mãi thành công!", chuongTrinhId = chuongTrinh.Id });
            }
            catch (Exception ex)
            {
                await _quanLyServices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi thêm chương trình khuyến mãi: {ex.Message}" });
            }
        }

        [Route("/API/get-next-id-CTKM")]
        public Task<IActionResult> GetNextIdCTKM([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLyServices.GenerateNewId<ChuongTrinhKhuyenMai>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }

        [HttpPost]
        [Route("/API/get-next-id-MKM")]
        public Task<IActionResult> GetNextIdMKM([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLyServices.GenerateNewId<MaKhuyenMai>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }

        [HttpPost]
        [Route("/API/get-next-id-DKAD")]
        public Task<IActionResult> GetNextIdDKAD([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLyServices.GenerateNewId<DieuKienApDung>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }

        //=========================================API Thêm Nhân Viên=======================================================================
        [HttpPost]
        [Route("/API/add-NhanVien")]
        public async Task<IActionResult> AddNhanVien([FromBody] NhanVienRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate
                if (string.IsNullOrWhiteSpace(request.HoTen))
                {
                    return BadRequest(new { message = "Họ tên không được để trống." });
                }

                if (string.IsNullOrWhiteSpace(request.SoDienThoai))
                {
                    return BadRequest(new { message = "Số điện thoại không được để trống." });
                }

                if (string.IsNullOrWhiteSpace(request.ChucVu))
                {
                    return BadRequest(new { message = "Chức vụ không được để trống." });
                }

                await _quanLyServices.BeginTransactionAsync();

                // Tạo Nhân viên
                NhanVien nhanVien = new NhanVien
                {
                    Id = _quanLyServices.GenerateNewId<NhanVien>("NV", 8),
                    HoTen = request.HoTen,
                    ChucVu = request.ChucVu,
                    LuongCoBan = request.LuongCoBan,
                    SoDienThoai = request.SoDienThoai,
                    Email = request.Email.ToLower(),
                    DiaChi = request.DiaChi ?? "",
                    NgayVaoLam = request.NgayVaoLam ?? DateTime.Now,
                    TrangThai = request.TrangThai ?? "HoatDong",
                    GioiTinh = request.GioiTinh,
                    AnhId = request.AnhId ?? "ANH_DEFAULT", // Cần tạo ảnh mặc định
                    IsDelete = false
                };

                _quanLyServices.Add<NhanVien>(nhanVien);

                if (!await _quanLyServices.CommitAsync())
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Không thể thêm nhân viên." });
                }

                return Ok(new { message = "Thêm nhân viên thành công!", nhanVienId = nhanVien.Id });
            }
            catch (Exception ex)
            {
                await _quanLyServices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi thêm nhân viên: {ex.Message}" });
            }
        }

        //=========================================API Thêm Phân Công Ca Làm Việc=======================================================================
        [HttpPost]
        [Route("/API/add-PhanCongCaLamViec")]
        public async Task<IActionResult> AddPhanCongCaLamViec([FromBody] PhanCongCaLamViecRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate
                if (string.IsNullOrWhiteSpace(request.NhanVienId))
                {
                    return BadRequest(new { message = "Vui lòng chọn nhân viên." });
                }

                if (string.IsNullOrWhiteSpace(request.CaLamViecId))
                {
                    return BadRequest(new { message = "Vui lòng chọn ca làm việc." });
                }

                if (request.Ngay == default(DateTime))
                {
                    return BadRequest(new { message = "Vui lòng chọn ngày phân công." });
                }

                await _quanLyServices.BeginTransactionAsync();

                // Kiểm tra xem nhân viên đã được phân công ca này trong ngày chưa
                var existingPhanCong = _quanLyServices.GetList<PhanCongCaLamViec>()
                    .FirstOrDefault(pc => pc.NhanVienId == request.NhanVienId &&
                                         pc.CaLamViecId == request.CaLamViecId &&
                                         pc.Ngay.Date == request.Ngay.Date &&
                                         !pc.IsDelete);

                if (existingPhanCong != null)
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Nhân viên đã được phân công ca này trong ngày đã chọn." });
                }

                // Tạo Phân công ca làm việc
                PhanCongCaLamViec phanCong = new PhanCongCaLamViec
                {
                    Id = _quanLyServices.GenerateNewId<PhanCongCaLamViec>("PCCLV", 10),
                    NhanVienId = request.NhanVienId,
                    CaLamViecId = request.CaLamViecId,
                    Ngay = request.Ngay,
                    IsDelete = false
                };
                _quanLyServices.Add<PhanCongCaLamViec>(phanCong);
                if (!await _quanLyServices.CommitAsync())
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Không thể thêm phân công ca làm việc." });
                }

                return Ok(new { message = "Phân công ca làm việc thành công!", phanCongId = phanCong.Id });
            }
            catch (Exception ex)
            {
                await _quanLyServices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi phân công ca làm việc: {ex.Message}" });
            }
        }


        [HttpPost]
        [Route("/API/get-next-id-NV")]
        public Task<IActionResult> GetNextIdNV([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLyServices.GenerateNewId<NhanVien>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }

        [HttpPost]
        [Route("/API/get-next-id-PCCLV")]
        public Task<IActionResult> GetNextIdPCCLV([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLyServices.GenerateNewId<PhanCongCaLamViec>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }
        [HttpPost]
        [Route("/API/PhanCong/Them")]
        public async Task<IActionResult> AddPhanCongCaLamViec([FromBody] PhanCongCaLamViecCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _quanLyServices.BeginTransactionAsync();
                // Kiểm tra xem nhân viên này đã bị phân công ca này trong ngày này chưa
                var existingPhanCong = _quanLyServices.GetList<PhanCongCaLamViec>()
                    .FirstOrDefault(p =>
                        p.NhanVienId == dto.NhanVienId &&
                        p.CaLamViecId == dto.CaLamViecId &&
                        p.Ngay.Date == dto.Ngay.Value.Date && // So sánh ngày
                        !p.IsDelete);

                if (existingPhanCong != null)
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Nhân viên này đã được phân công ca này trong ngày đã chọn." });
                }

                // Tạo Entity
                var phanCong = new PhanCongCaLamViec
                {
                    Id = _quanLyServices.GenerateNewId<PhanCongCaLamViec>("PCCLV", 9),
                    NhanVienId = dto.NhanVienId,
                    CaLamViecId = dto.CaLamViecId,
                    Ngay = dto.Ngay.Value,
                    IsDelete = false
                };

                // Lưu

                _quanLyServices.Add<PhanCongCaLamViec>(phanCong);

                if (await _quanLyServices.CommitAsync())
                {
                    return Ok(new { message = "Thêm phân công thành công!" });
                }
                else
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Lỗi khi lưu phân công vào cơ sở dữ liệu." });
                }
            }
            catch (Exception ex)
            {
                await _quanLyServices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }

        //=========================================API Thêm Khách Hàng=======================================================================
        [HttpPost]
        [Route("/API/add-KhachHang")]
        public async Task<IActionResult> AddKhachHang([FromBody] KhachHangRequest request)
        {
            try
            {
                Console.WriteLine("=== API ADD KHACH HANG CALLED ===");
                Console.WriteLine($"Request: {System.Text.Json.JsonSerializer.Serialize(request)}");

                if (request == null)
                {
                    Console.WriteLine("ERROR: Request is null");
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate
                if (string.IsNullOrWhiteSpace(request.HoTen))
                {
                    Console.WriteLine("ERROR: HoTen is empty");
                    return BadRequest(new { message = "Họ tên không được để trống." });
                }

                if (string.IsNullOrWhiteSpace(request.SoDienThoai))
                {
                    Console.WriteLine("ERROR: SoDienThoai is empty");
                    return BadRequest(new { message = "Số điện thoại không được để trống." });
                }

                await _quanLyServices.BeginTransactionAsync();

                // Kiểm tra số điện thoại đã tồn tại chưa
                Console.WriteLine("Checking if phone number exists...");
                var existingKH = _quanLyServices.GetList<KhachHang>()
                    .FirstOrDefault(kh => kh.SoDienThoai == request.SoDienThoai && !kh.IsDelete);

                if (existingKH != null)
                {
                    await _quanLyServices.RollbackAsync();
                    Console.WriteLine($"ERROR: Phone number '{request.SoDienThoai}' already exists");
                    return BadRequest(new { message = $"Số điện thoại '{request.SoDienThoai}' đã được sử dụng." });
                }

                // Lấy hoặc tạo ảnh mặc định
                Console.WriteLine("Getting or creating default image...");
                string anhId = request.AnhId;

                if (string.IsNullOrEmpty(anhId))
                {
                    // Kiểm tra ảnh mặc định có tồn tại không
                    var defaultImage = _quanLyServices.GetList<HinhAnh>()
                        .FirstOrDefault(ha => ha.Id == "ANH_DEFAULT");

                    if (defaultImage == null)
                    {
                        // Tạo ảnh mặc định (1x1 pixel transparent PNG)
                        byte[] defaultImageBytes = Convert.FromBase64String(
                            "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg=="
                        );

                        defaultImage = new HinhAnh
                        {
                            Id = "ANH_DEFAULT",
                            TenAnh = "Default Avatar",
                            Anh = defaultImageBytes
                        };

                        Console.WriteLine("Creating default image...");
                        _quanLyServices.Add<HinhAnh>(defaultImage);
                    }

                    anhId = "ANH_DEFAULT";
                }

                // Tạo Khách hàng
                Console.WriteLine("Creating KhachHang entity...");
                KhachHang khachHang = new KhachHang
                {
                    Id = _quanLyServices.GenerateNewId<KhachHang>("KH", 6),
                    HoTen = request.HoTen,
                    SoDienThoai = request.SoDienThoai,
                    Email = request.Email.ToLower(),
                    DiaChi = request.DiaChi ?? "",
                    NgayDangKy = request.NgayDangKy ?? DateTime.Now,
                    TrangThai = request.TrangThai ?? "Hoạt động",
                    GioiTinh = request.GioiTinh,
                    AnhId = anhId,
                    IsDelete = false
                };

                Console.WriteLine($"Generated KhachHang ID: {khachHang.Id}");
                Console.WriteLine("Adding KhachHang to database...");

                _quanLyServices.Add<KhachHang>(khachHang);


                // Nếu có thông tin thẻ thành viên, tạo thẻ
                if (request.CreateMemberCard && request.TheThanhVien != null)
                {
                    Console.WriteLine("Creating TheThanhVien...");

                    // Map từ tiếng Anh sang tiếng Việt
                    string hangVietnamese = request.TheThanhVien.Hang switch
                    {
                        "Bronze" => "Đồng",
                        "Silver" => "Bạc",
                        "Gold" => "Vàng",
                        "Platinum" => "Bạch Kim",
                        _ => "Đồng" // Default
                    };

                    TheThanhVien theThanhVien = new TheThanhVien
                    {
                        Id = _quanLyServices.GenerateNewId<TheThanhVien>("TTV", 7),
                        KhachHangId = khachHang.Id,
                        Hang = hangVietnamese,
                        DiemTichLuy = request.TheThanhVien.DiemTichLuy,
                        NgayCap = request.TheThanhVien.NgayCap ?? DateTime.Now,
                        IsDelete = false
                    };

                    Console.WriteLine($"Generated TheThanhVien ID: {theThanhVien.Id}");
                    Console.WriteLine("Adding TheThanhVien to database...");

                    _quanLyServices.Add<TheThanhVien>(theThanhVien);

                    Console.WriteLine("TheThanhVien added successfully");
                }

                if (!await _quanLyServices.CommitAsync())
                {
                    await _quanLyServices.RollbackAsync();
                    Console.WriteLine("ERROR: CommitAsync returned false");
                    return BadRequest(new { message = "Không thể thêm khách hàng." });
                }

                Console.WriteLine("=== SUCCESS: All operations completed ===");
                return Ok(new { message = "Thêm khách hàng thành công!", khachHangId = khachHang.Id });
            }
            catch (Exception ex)
            {
                await _quanLyServices.RollbackAsync();
                Console.WriteLine($"EXCEPTION: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, new { message = $"Lỗi khi thêm khách hàng: {ex.Message}" });
            }
        }

        //=========================================API Thêm Phiếu Kiểm Kê=======================================================================
        [HttpPost]
        [Route("/API/add-KiemKe")]
        public async Task<IActionResult> AddKiemKe([FromBody] KiemKeRequest request)
        {
            try
            {
                Console.WriteLine("=== API ADD KIEM KE CALLED ===");
                Console.WriteLine($"Request: {System.Text.Json.JsonSerializer.Serialize(request)}");

                if (request == null)
                {
                    Console.WriteLine("ERROR: Request is null");
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate
                if (string.IsNullOrWhiteSpace(request.NhanVienId))
                {
                    Console.WriteLine("ERROR: NhanVienId is empty");
                    return BadRequest(new { message = "Vui lòng chọn nhân viên thực hiện." });
                }

                if (request.NgayKiemKe == default(DateTime))
                {
                    Console.WriteLine("ERROR: NgayKiemKe is empty");
                    return BadRequest(new { message = "Vui lòng chọn ngày kiểm kê." });
                }

                if (request.ChiTietKiemKe == null || !request.ChiTietKiemKe.Any())
                {
                    Console.WriteLine("ERROR: ChiTietKiemKe is empty");
                    return BadRequest(new { message = "Vui lòng thêm ít nhất một sản phẩm để kiểm kê." });
                }

                Console.WriteLine($"Creating {request.ChiTietKiemKe.Count} KiemKe records...");

                int successCount = 0;
                int failCount = 0;

                // Tạo từng bản ghi kiểm kê cho mỗi sản phẩm
                foreach (var chiTiet in request.ChiTietKiemKe)
                {
                    if (string.IsNullOrEmpty(chiTiet.SanPhamDonViId))
                    {
                        Console.WriteLine($"WARNING: Skipping record with empty SanPhamDonViId");
                        failCount++;
                        continue;
                    }

                    // Tạo bản ghi KiemKe
                    KiemKe kiemKe = new KiemKe
                    {
                        Id = _quanLyServices.GenerateNewId<KiemKe>("KK", 6),
                        NgayKiemKe = request.NgayKiemKe,
                        NhanVienId = request.NhanVienId,
                        SanPhamDonViId = chiTiet.SanPhamDonViId,
                        KetQua = chiTiet.KetQua ?? "",
                        IsDelete = false
                    };

                    Console.WriteLine($"Adding KiemKe: ID={kiemKe.Id}, SanPham={chiTiet.SanPhamDonViId}");

                    await _quanLyServices.BeginTransactionAsync();

                    _quanLyServices.Add<KiemKe>(kiemKe);

                    
                }

                Console.WriteLine($"=== RESULT: {successCount} success, {failCount} failed ===");

                if (await _quanLyServices.CommitAsync())
                {
                    string message = failCount > 0
                        ? $"Tạo phiếu kiểm kê thành công {successCount} sản phẩm, {failCount} thất bại."
                        : $"Tạo phiếu kiểm kê thành công cho {successCount} sản phẩm!";

                    return Ok(new { message = message, successCount = successCount, failCount = failCount });
                }
                else
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Không thể tạo phiếu kiểm kê." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ EXCEPTION: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, new { message = $"Lỗi khi tạo phiếu kiểm kê: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("/API/get-next-id-KK")]
        public Task<IActionResult> GetNextIdKK([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLyServices.GenerateNewId<KiemKe>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }

        //=========================================API Áp Dụng Mã Khuyến Mãi=======================================================================
        [HttpPost]
        [Route("/API/apply-discount")]
        public IActionResult ApplyDiscount([FromBody] ApplyDiscountRequest request)
        {
            try
            {
                Console.WriteLine("=== API APPLY DISCOUNT CALLED ===");
                Console.WriteLine($"Code: {request.MaGiamGia}, TongTien: {request.TongTien}");

                if (string.IsNullOrWhiteSpace(request.MaGiamGia))
                {
                    return BadRequest(new { message = "Mã giảm giá không được để trống." });
                }

                // Tìm mã khuyến mãi
                var maKhuyenMai = _quanLyServices.GetList<MaKhuyenMai>()
                    .FirstOrDefault(mkm => mkm.Code == request.MaGiamGia &&
                                          !mkm.IsDelete &&
                                          mkm.TrangThai == "Hoạt động");

                if (maKhuyenMai == null)
                {
                    return BadRequest(new { message = "Mã giảm giá không tồn tại hoặc đã hết hạn." });
                }

                // Kiểm tra số lần sử dụng
                if (maKhuyenMai.SoLanSuDung <= 0)
                {
                    return BadRequest(new { message = "Mã giảm giá đã hết lượt sử dụng." });
                }

                // Load thông tin chương trình khuyến mãi
                if (!string.IsNullOrEmpty(maKhuyenMai.ChuongTrinhId))
                {
                    maKhuyenMai.ChuongTrinh = _quanLyServices.GetById<ChuongTrinhKhuyenMai>(maKhuyenMai.ChuongTrinhId);

                    // Kiểm tra thời hạn chương trình
                    if (maKhuyenMai.ChuongTrinh != null)
                    {
                        var now = DateTime.Now.Date;
                        if (now < maKhuyenMai.ChuongTrinh.NgayBatDau || now > maKhuyenMai.ChuongTrinh.NgayKetThuc)
                        {
                            return BadRequest(new { message = "Chương trình khuyến mãi chưa bắt đầu hoặc đã kết thúc." });
                        }
                    }
                }

                decimal giaTri = maKhuyenMai.GiaTri;

                Console.WriteLine($"✅ Mã hợp lệ. Giá trị giảm: {giaTri}");

                return Ok(new
                {
                    id = maKhuyenMai.Id,
                    code = maKhuyenMai.Code,
                    ten = maKhuyenMai.ChuongTrinh?.Ten ?? "Mã giảm giá",
                    giaTri = giaTri,
                    message = "Áp dụng mã giảm giá thành công!"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ EXCEPTION: {ex.Message}");
                return StatusCode(500, new { message = $"Lỗi khi áp dụng mã giảm giá: {ex.Message}" });
            }
        }

        public class ApplyDiscountRequest
        {
            public string MaGiamGia { get; set; }
            public decimal TongTien { get; set; }
        }

        //=========================================API Thêm Hóa Đơn=======================================================================
        [HttpPost]
        [Route("/API/add-HoaDon")]
        public async Task<IActionResult> AddHoaDon([FromBody] HoaDonRequest request)
        {
            try
            {
                Console.WriteLine("=== API ADD HÓA ĐƠN CALLED ===");
                Console.WriteLine($"Request: {System.Text.Json.JsonSerializer.Serialize(request)}");

                if (request == null)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate
                if (request.ChiTietHoaDon == null || !request.ChiTietHoaDon.Any())
                {
                    return BadRequest(new { message = "Hóa đơn phải có ít nhất một sản phẩm." });
                }

                // Xử lý KhachHangId - nếu null thì tạo hoặc dùng khách lẻ mặc định
                string khachHangId = request.KhachHangId;

                await _quanLyServices.BeginTransactionAsync();

                if (string.IsNullOrEmpty(khachHangId))
                {
                    Console.WriteLine("KhachHangId is null - creating/using default customer...");

                    // Kiểm tra khách hàng mặc định có tồn tại không
                    var khachLe = _quanLyServices.GetById<KhachHang>("KH_LE");

                    if (khachLe == null)
                    {
                        Console.WriteLine("Creating default customer KH_LE...");

                        // Tạo ảnh mặc định nếu chưa có
                        var defaultImage = _quanLyServices.GetList<HinhAnh>()
                            .FirstOrDefault(ha => ha.Id == "ANH_DEFAULT");

                        if (defaultImage == null)
                        {
                            byte[] defaultImageBytes = Convert.FromBase64String(
                                "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg=="
                            );

                            defaultImage = new HinhAnh
                            {
                                Id = "ANH_DEFAULT",
                                TenAnh = "Default Avatar",
                                Anh = defaultImageBytes
                            };

                            _quanLyServices.Add<HinhAnh>(defaultImage);
                        }

                        // Tạo khách hàng lẻ mặc định
                        khachLe = new KhachHang
                        {
                            Id = "KH_LE",
                            HoTen = "Khách lẻ",
                            SoDienThoai = "0000000000",
                            Email = null,
                            DiaChi = "",
                            NgayDangKy = DateTime.Now,
                            TrangThai = "Hoạt động",
                            GioiTinh = false,
                            AnhId = "ANH_DEFAULT",
                            IsDelete = false
                        };



                        _quanLyServices.Add<KhachHang>(khachLe);

                        Console.WriteLine("✅ Created default customer KH_LE");
                    }

                    khachHangId = "KH_LE";
                    Console.WriteLine($"Using KhachHangId: {khachHangId}");
                }

                var NhanVienId = _quanLyServices.GetById<TaiKhoan>(User.FindFirst(ClaimTypes.NameIdentifier)?.Value).TaiKhoanNhanVien.NhanVienId;


                // Tạo hóa đơn
                HoaDon hoaDon = new HoaDon
                {
                    Id = _quanLyServices.GenerateNewId<HoaDon>("HD", 6),
                    KhachHangId = khachHangId,
                    NhanVienId = NhanVienId,
                    NgayLap = request.NgayLap ?? DateTime.Now,
                    TrangThai = request.TrangThai ?? "Chưa thanh toán",
                    TongTien = 0, // Sẽ tính sau
                    IsDelete = false
                };

                Console.WriteLine($"Generated HoaDon ID: {hoaDon.Id}, TrangThai: {hoaDon.TrangThai}, KhachHangId: {hoaDon.KhachHangId}");

                _quanLyServices.Add<HoaDon>(hoaDon);

                decimal tongTien = 0;

                // Thêm chi tiết hóa đơn
                foreach (var chiTiet in request.ChiTietHoaDon)
                {
                    decimal thanhTien = chiTiet.SoLuong * chiTiet.DonGia;
                    tongTien += thanhTien;

                    ChiTietHoaDon ctHoaDon = new ChiTietHoaDon
                    {
                        HoaDonId = hoaDon.Id,
                        SanPhamDonViId = chiTiet.SanPhamDonViId,
                        SoLuong = chiTiet.SoLuong,
                        DonGia = chiTiet.DonGia,
                        GiamGia = chiTiet.GiamGia ?? 0,
                        TongTien = thanhTien,
                        IsDelete = false
                    };

                    Console.WriteLine($"Adding ChiTietHoaDon: SanPham={chiTiet.SanPhamDonViId}, SL={chiTiet.SoLuong}, DonGia={chiTiet.DonGia}");

                    _quanLyServices.Add<ChiTietHoaDon>(ctHoaDon);
                }



                if(await _quanLyServices.CommitAsync() == false)
                {
                    await _quanLyServices.RollbackAsync();
                    return BadRequest(new { message = "Không thể tạo hóa đơn." });
                }

                return Ok(new
                {
                    message = "Tạo hóa đơn thành công!",
                    hoaDonId = hoaDon.Id,
                    tongTien = tongTien
                });
            }
            catch (Exception ex)
            {
                await _quanLyServices.RollbackAsync();
                Console.WriteLine($"❌ EXCEPTION: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = $"Lỗi khi tạo hóa đơn: {ex.Message}" });
            }
        }










        public class PhieuNhapFormData
        {
            public string NhaCungCapId { get; set; }
            public string NhanVienId { get; set; }
            public DateTime NgayNhap { get; set; }
            public List<ChiTietPhieuNhapFormData> ChiTietPhieuNhap { get; set; }
        }

        public class ChiTietPhieuNhapFormData
        {
            public string SanPhamDonViId { get; set; }
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
            public DateTime HanSuDung { get; set; }
        }

        public class GanViTriFormData
        {
            public string NhanVienId { get; set; }
            public DateTime NgayThucHien { get; set; }
            public List<ChiTietGanViTriFormData> ChiTietGanViTri { get; set; }
        }

        public class ChiTietGanViTriFormData
        {
            public string SanPhamDonViId { get; set; }
            public string ViTriId { get; set; }
            public int SoLuong { get; set; }
        }

        public class ChuongTrinhKhuyenMaiRequest
        {
            public string Ten { get; set; }
            public string Loai { get; set; }
            public DateTime NgayBatDau { get; set; }
            public DateTime NgayKetThuc { get; set; }
            public string MoTa { get; set; }
            public DieuKienApDungRequest DieuKienApDung { get; set; }
            public MaKhuyenMaiRequest MaKhuyenMai { get; set; }
            public string PhamViApDung { get; set; }
            public List<string> DanhMucIds { get; set; }
            public List<string> SanPhamIds { get; set; }
        }

        public class DieuKienApDungRequest
        {
            public string DieuKien { get; set; }
            public decimal GiaTriToiThieu { get; set; }
            public string GiamTheo { get; set; }
            public decimal GiaTriToiDa { get; set; }
        }

        public class MaKhuyenMaiRequest
        {
            public string Code { get; set; }
            public decimal GiaTri { get; set; }
            public int SoLanSuDung { get; set; }
            public string TrangThai { get; set; }
        }

        public class HoaDonRequest
        {
            public string KhachHangId { get; set; }
            public string NhanVienId { get; set; }
            public DateTime? NgayLap { get; set; }
            public string TrangThai { get; set; }
            public string MaKhuyenMaiId { get; set; }
            public string KenhThanhToanId { get; set; }
            public string MoTaThanhToan { get; set; }
            public decimal? TongGiamGia { get; set; }
            public List<ChiTietHoaDonRequest> ChiTietHoaDon { get; set; }
        }

        public class ChiTietHoaDonRequest
        {
            public string SanPhamDonViId { get; set; }
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
            public decimal? GiamGia { get; set; }
        }
        public class NhanVienRequest
        {
            public string HoTen { get; set; }
            public string ChucVu { get; set; }
            public decimal LuongCoBan { get; set; }
            public string SoDienThoai { get; set; }
            public string Email { get; set; }
            public string DiaChi { get; set; }
            public DateTime? NgayVaoLam { get; set; }
            public string TrangThai { get; set; }
            public bool GioiTinh { get; set; }
            public string AnhId { get; set; }
        }
        public class PhanCongCaLamViecRequest
        {
            public string NhanVienId { get; set; }
            public string CaLamViecId { get; set; }
            public DateTime Ngay { get; set; }
        }

        public class KhachHangRequest
        {
            public string HoTen { get; set; }
            public string SoDienThoai { get; set; }
            public string Email { get; set; }
            public string DiaChi { get; set; }
            public DateTime? NgayDangKy { get; set; }
            public string TrangThai { get; set; }
            public bool GioiTinh { get; set; }
            public string AnhId { get; set; }
            public bool CreateMemberCard { get; set; }
            public TheThanhVienRequest TheThanhVien { get; set; }
        }

        public class TheThanhVienRequest
        {
            public string Hang { get; set; }
            public int DiemTichLuy { get; set; }
            public DateTime? NgayCap { get; set; }
        }

        public class KiemKeRequest
        {
            public string NhanVienId { get; set; }
            public DateTime NgayKiemKe { get; set; }
            public List<ChiTietKiemKeRequest> ChiTietKiemKe { get; set; }
        }

        public class ChiTietKiemKeRequest
        {
            public string SanPhamDonViId { get; set; }
            public string KetQua { get; set; }
        }

    }
}

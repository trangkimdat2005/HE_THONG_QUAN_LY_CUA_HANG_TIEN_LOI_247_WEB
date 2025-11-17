using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [ApiController]
    [Route("API")]
    [Area("Admin")]
    public class ThemController : Controller
    {
        private readonly IPhieuDoiTraServices _phieuDoiTraServices;
        private readonly IChinhSachHoanTraServices _chinhSachHoanTraServices;
        private readonly IQuanLyServices _quanLyServices;

        public ThemController(
            IPhieuDoiTraServices phieuDoiTraServices,
            IChinhSachHoanTraServices chinhSachHoanTraServices,
            IQuanLyServices quanLyServices)
        {
            _phieuDoiTraServices = phieuDoiTraServices;
            _chinhSachHoanTraServices = chinhSachHoanTraServices;
            _quanLyServices = quanLyServices;
        }

        [Route("/Them/ThemDanhMucViTRi")]
        public IActionResult ThemDanhMucViTRi()
        {
            // Load danh sách vị trí hiện có để gợi ý loại vị trí
            ViewData["DanhSachViTri"] = _quanLyServices.GetList<ViTri>();
            return View();
        }

        [Route("/Them/ThemHoaDon")]
        public IActionResult ThemHoaDon()
        {
            // Load danh sách khách hàng
            var lstKhachHang = _quanLyServices.GetList<KhachHang>()
                .Where(kh => !kh.IsDelete && kh.TrangThai == "Active")
                .OrderBy(kh => kh.HoTen)
                .ToList();

            // Load danh sách sản phẩm đơn vị
            var lstSanPhamDonVi = _quanLyServices.GetList<SanPhamDonVi>()
                .Where(sp => !sp.IsDelete)
                .ToList();

            // Load thông tin cho từng sản phẩm đơn vị
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
                .Where(mkm => !mkm.IsDelete && mkm.TrangThai == "Active")
                .ToList();

            ViewData["DanhSachKhachHang"] = lstKhachHang;
            ViewData["DanhSachSanPhamDonVi"] = lstSanPhamDonVi;
            ViewData["DanhSachDanhMuc"] = lstDanhMuc;
            ViewData["DanhSachMaKhuyenMai"] = lstMaKhuyenMai;

            return View();
        }

        [Route("/Them/ThemHoanTra")]
        public IActionResult ThemHoanTra()
        {
            return View();
        }

        [Route("/Them/ThemKiemKe")]
        public IActionResult ThemKiemKe()
        {
            // Load danh sách nhân viên và sản phẩm cho phiếu kiểm kê
            ViewData["DanhSachNhanVien"] = _quanLyServices.GetList<NhanVien>();
            ViewData["DanhSachSanPhamDonVi"] = _quanLyServices.GetList<SanPhamDonVi>();
            
            return View();
        }

        [Route("/Them/ThemKhachHang")]
        public IActionResult ThemKhachHang()
        {
            // Load danh sách hình ảnh (nếu cần)
            ViewData["DanhSachHinhAnh"] = _quanLyServices.GetList<HinhAnh>();

            return View();
        }

        [Route("/Them/ThemMaKhuyenMai")]
        public IActionResult ThemMaKhuyenMai()
        {
            // Load danh sách danh mục và sản phẩm
            ViewData["DanhMucs"] = _quanLyServices.GetList<DanhMuc>();
            ViewData["SanPhams"] = _quanLyServices.GetList<SanPham>();

            return View();
        }

        [Route("/Them/ThemNCC")]
        public IActionResult ThemNCC()
        {
            return View();
        }

        [HttpPost]
        [Route("/API/ThemNCC/Add")]
        public IActionResult AddNhaCungCap([FromBody] NhaCungCapDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newId = _quanLyServices.GenerateNewId<NhaCungCap>("NCC", 7);

                if (string.IsNullOrEmpty(newId))
                {
                    return BadRequest(new { message = "Không thể tạo ID mới cho nhà cung cấp." });
                }

                var nhaCungCapEntity = new NhaCungCap
                {
                    Id = newId,
                    Ten = data.Ten,
                    SoDienThoai = data.SoDienThoai,
                    Email = data.Email,
                    DiaChi = data.DiaChi,
                    MaSoThue = data.MaSoThue,
                    IsDelete = false
                };

                bool success = _quanLyServices.Add<NhaCungCap>(nhaCungCapEntity);

                if (success)
                {
                    return Ok(new { message = $"Thêm nhà cung cấp '{data.Ten}' thành công!", newId = newId });
                }
                else
                {
                    return BadRequest(new { message = "Lỗi: Không thể lưu nhà cung cấp vào cơ sở dữ liệu." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
        
        [HttpPost("get-next-id-NCC")]
        public Task<IActionResult> GetNextIdNCC([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLyServices.GenerateNewId<NhaCungCap>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }

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
                // 2. Xử lý lưu HinhAnh trước
                byte[] anhBytes = await _quanLyServices.ConvertImageToByteArray(dto.AnhDaiDien);

                newHinhAnh = new HinhAnh
                {
                    Id = _quanLyServices.GenerateNewId<HinhAnh>("ANH", 7),
                    TenAnh = dto.AnhDaiDien.FileName,
                    Anh = anhBytes
                };

                // Thêm ảnh vào DB
                if (!_quanLyServices.Add<HinhAnh>(newHinhAnh))
                {
                    return BadRequest(new { message = "Lỗi: Không thể lưu hình ảnh vào cơ sở dữ liệu." });
                }

                // 3. Tạo NhanVien entity
                var nhanVien = new NhanVien
                {
                    Id = _quanLyServices.GenerateNewId<NhanVien>("NV", 6),
                    HoTen = dto.HoTen,
                    ChucVu = dto.ChucVu,
                    LuongCoBan = (decimal)dto.LuongCoBan,
                    SoDienThoai = dto.SoDienThoai,
                    Email = dto.Email,
                    DiaChi = dto.DiaChi,
                    NgayVaoLam = dto.NgayVaoLam,
                    TrangThai = dto.TrangThai,
                    GioiTinh = dto.GioiTinh,
                    AnhId = newHinhAnh.Id, // Gán ID của ảnh vừa lưu
                    IsDelete = false
                };

                // 4. Lưu NhanVien vào DB
                if (_quanLyServices.Add<NhanVien>(nhanVien))
                {
                    return Ok(new { message = $"Thêm nhân viên {nhanVien.HoTen} thành công!", newId = nhanVien.Id });
                }
                else
                {
                    // Lỗi: Đã lỡ lưu ảnh. Phải xóa (rollback)
                    _quanLyServices.HardDelete<HinhAnh>(newHinhAnh); // Cố gắng dọn dẹp
                    return BadRequest(new { message = "Lỗi khi lưu thông tin nhân viên." });
                }
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, mà newHinhAnh đã được tạo và lưu, hãy xóa nó
                if (newHinhAnh != null && !string.IsNullOrEmpty(newHinhAnh.Id))
                {
                    _quanLyServices.HardDelete<HinhAnh>(newHinhAnh);
                }
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
        [Route("/Them/ThemNhapKho")]
        public IActionResult ThemNhapKho()
        {
            // Load danh sách nhà cung cấp, nhân viên, sản phẩm
            ViewData["DanhSachNhaCungCap"] = _quanLyServices.GetList<NhaCungCap>();
            ViewData["DanhSachNhanVien"] = _quanLyServices.GetList<NhanVien>();
            ViewData["DanhSachSanPhamDonVi"] = _quanLyServices.GetList<SanPhamDonVi>();

            return View();
        }

        [Route("/Them/ThemPhanCongCaLamViec")]
        public IActionResult ThemPhanCongCaLamViec()
        {
            var lstNhanVien = _quanLyServices.GetList<NhanVien>();
            ViewData["lstNhanVien"] = lstNhanVien;

            var lstCaLamViec = _quanLyServices.GetList<CaLamViec>();
            ViewData["lstCaLamViec"]= lstCaLamViec;
            return View();
        }

        [HttpGet]
        [Route("/Them/ThemPhieuDoiTra")]
        public IActionResult ThemPhieuDoiTra()
        {
            // Load danh sách hóa đơn và chính sách
            ViewData["DanhSachHoaDon"] = _phieuDoiTraServices.GetAllHoaDons();
            ViewData["DanhSachChinhSach"] = _phieuDoiTraServices.GetAllChinhSachs();

            return View();
        }

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

        // API để lấy danh sách sản phẩm theo hóa đơn
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

        [HttpGet]
        [Route("/Them/ThemChinhSachHoanTra")]
        public IActionResult ThemChinhSachHoanTra()
        {
            ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
            return View();
        }

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

        [Route("/Them/ThemTaiKhoan")]
        public IActionResult ThemTaiKhoan()
        {
            return View();
        }

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
                    Id = _quanLyServices.GenerateNewId<PhieuNhap>("PN", 5),
                    NhaCungCapId = request.NhaCungCapId,
                    NhanVienId = request.NhanVienId,
                    NgayNhap = request.NgayNhap,
                    TongTien = 0, // Sẽ được tính sau
                    IsDelete = false
                };

                if (!_quanLyServices.Add<PhieuNhap>(phieuNhap))
                {
                    return BadRequest(new { message = "Không thể thêm phiếu nhập" });
                }

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

                    if (!_quanLyServices.Add<ChiTietPhieuNhap>(chiTietPhieuNhap))
                    {
                        return BadRequest(new { message = $"Không thể thêm chi tiết phiếu nhập cho sản phẩm {chiTiet.SanPhamDonViId}" });
                    }
                }

                // Cập nhật tổng tiền cho phiếu nhập
                phieuNhap.TongTien = tongTien;
                if (!_quanLyServices.Update<PhieuNhap>(phieuNhap))
                {
                    return BadRequest(new { message = "Không thể cập nhật tổng tiền phiếu nhập" });
                }

                return Ok(new { message = "Thêm phiếu nhập thành công!", phieuNhapId = phieuNhap.Id });
            }
            catch (Exception ex)
            {
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
                        if (!_quanLyServices.Update<SanPhamViTri>(existingSPVT))
                        {
                            return BadRequest(new { message = $"Không thể cập nhật số lượng cho sản phẩm {chiTiet.SanPhamDonViId}" });
                        }
                    }
                    else
                    {
                        // Tạo mới bản ghi SanPhamViTri
                        SanPhamViTri sanPhamViTri = new SanPhamViTri
                        {
                            Id = _quanLyServices.GenerateNewId<SanPhamViTri>("SPVT", 7),
                            SanPhamDonViId = chiTiet.SanPhamDonViId,
                            ViTriId = chiTiet.ViTriId,
                            SoLuong = chiTiet.SoLuong,
                            IsDelete = false
                        };

                        if (!_quanLyServices.Add<SanPhamViTri>(sanPhamViTri))
                        {
                            return BadRequest(new { message = $"Không thể gán vị trí cho sản phẩm {chiTiet.SanPhamDonViId}" });
                        }
                    }
                }

                return Ok(new { message = "Gán vị trí sản phẩm thành công!" });
            }
            catch (Exception ex)
            {
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
                    Console.WriteLine($"ERROR: MaViTri '{viTri.MaViTri}' already exists (ID: {existingViTri.Id})");
                    return BadRequest(new { message = $"Mã vị trí '{viTri.MaViTri}' đã tồn tại trong hệ thống." });
                }

                // Tạo ID mới cho vị trí
                Console.WriteLine("Generating new ID...");
                viTri.Id = _quanLyServices.GenerateNewId<ViTri>("VT", 5);
                Console.WriteLine($"Generated ID: {viTri.Id}");

                viTri.IsDelete = false;

                Console.WriteLine("Calling Add service...");
                var addResult = _quanLyServices.Add<ViTri>(viTri);
                Console.WriteLine($"Add result: {addResult}");

                if (!addResult)
                {
                    Console.WriteLine("ERROR: Add service returned false");
                    return BadRequest(new { message = "Không thể thêm vị trí mới. Vui lòng kiểm tra kết nối database." });
                }

                Console.WriteLine($"SUCCESS: Added ViTri with ID={viTri.Id}, MaViTri={viTri.MaViTri}");
                return Ok(new { message = "Thêm vị trí mới thành công!", viTriId = viTri.Id });
            }
            catch (Exception ex)
            {
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
                    Id = _quanLyServices.GenerateNewId<ChuongTrinhKhuyenMai>("CTKM", 7),
                    Ten = request.Ten,
                    Loai = request.Loai,
                    NgayBatDau = request.NgayBatDau,
                    NgayKetThuc = request.NgayKetThuc,
                    MoTa = request.MoTa,
                    IsDelete = false
                };

                if (!_quanLyServices.Add<ChuongTrinhKhuyenMai>(chuongTrinh))
                {
                    return BadRequest(new { message = "Không thể thêm chương trình khuyến mãi." });
                }

                // Thêm Điều kiện áp dụng
                if (request.DieuKienApDung != null)
                {
                    DieuKienApDung dieuKien = new DieuKienApDung
                    {
                        Id = _quanLyServices.GenerateNewId<DieuKienApDung>("DK", 5),
                        ChuongTrinhId = chuongTrinh.Id,
                        DieuKien = request.DieuKienApDung.DieuKien ?? "",
                        GiaTriToiThieu = request.DieuKienApDung.GiaTriToiThieu,
                        GiamTheo = request.DieuKienApDung.GiamTheo,
                        GiaTriToiDa = request.DieuKienApDung.GiaTriToiDa,
                        IsDelete = false
                    };

                    if (!_quanLyServices.Add<DieuKienApDung>(dieuKien))
                    {
                        return BadRequest(new { message = "Không thể thêm điều kiện áp dụng." });
                    }

                    // Thêm áp dụng theo phạm vi
                    if (request.PhamViApDung == "DanhMuc" && request.DanhMucIds != null && request.DanhMucIds.Any())
                    {
                        foreach (var danhMucId in request.DanhMucIds)
                        {
                            DieuKienApDungDanhMuc dkadDanhMuc = new DieuKienApDungDanhMuc
                            {
                                Id = _quanLyServices.GenerateNewId<DieuKienApDungDanhMuc>("DKNV", 7),
                                DieuKienId = dieuKien.Id,
                                DanhMucId = danhMucId,
                                IsDelete = false
                            };

                            if (!_quanLyServices.Add<DieuKienApDungDanhMuc>(dkadDanhMuc))
                            {
                                return BadRequest(new { message = "Không thể thêm điều kiện áp dụng danh mục." });
                            }
                        }
                    }
                    else if (request.PhamViApDung == "SanPham" && request.SanPhamIds != null && request.SanPhamIds.Any())
                    {
                        foreach (var sanPhamId in request.SanPhamIds)
                        {
                            DieuKienApDungSanPham dkadSanPham = new DieuKienApDungSanPham
                            {
                                Id = _quanLyServices.GenerateNewId<DieuKienApDungSanPham>("DKSP", 7),
                                DieuKienId = dieuKien.Id,
                                SanPhamId = sanPhamId,
                                IsDelete = false
                            };

                            if (!_quanLyServices.Add<DieuKienApDungSanPham>(dkadSanPham))
                            {
                                return BadRequest(new { message = "Không thể thêm điều kiện áp dụng sản phẩm." });
                            }
                        }
                    }
                    else if (request.PhamViApDung == "ToanBo")
                    {
                        DieuKienApDungToanBo dkadToanBo = new DieuKienApDungToanBo
                        {
                            Id = _quanLyServices.GenerateNewId<DieuKienApDungToanBo>("DKTB", 7),
                            DieuKienId = dieuKien.Id,
                            GhiChu = "Áp dụng toàn bộ cửa hàng",
                            IsDelete = false
                        };

                        if (!_quanLyServices.Add<DieuKienApDungToanBo>(dkadToanBo))
                        {
                            return BadRequest(new { message = "Không thể thêm điều kiện áp dụng toàn bộ." });
                        }
                    }
                }

                // Thêm Mã khuyến mãi
                if (request.MaKhuyenMai != null)
                {
                    MaKhuyenMai maKhuyenMai = new MaKhuyenMai
                    {
                        Id = _quanLyServices.GenerateNewId<MaKhuyenMai>("MKM", 6),
                        ChuongTrinhId = chuongTrinh.Id,
                        Code = request.MaKhuyenMai.Code,
                        GiaTri = request.MaKhuyenMai.GiaTri,
                        SoLanSuDung = request.MaKhuyenMai.SoLanSuDung,
                        TrangThai = request.MaKhuyenMai.TrangThai,
                        IsDelete = false
                    };

                    if (!_quanLyServices.Add<MaKhuyenMai>(maKhuyenMai))
                    {
                        return BadRequest(new { message = "Không thể thêm mã khuyến mãi." });
                    }
                }

                return Ok(new { message = "Thêm chương trình khuyến mãi thành công!", chuongTrinhId = chuongTrinh.Id });
            }
            catch (Exception ex)
            {
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

                // Tạo Nhân viên
                NhanVien nhanVien = new NhanVien
                {
                    Id = _quanLyServices.GenerateNewId<NhanVien>("NV", 8),
                    HoTen = request.HoTen,
                    ChucVu = request.ChucVu,
                    LuongCoBan = request.LuongCoBan,
                    SoDienThoai = request.SoDienThoai,
                    Email = request.Email,
                    DiaChi = request.DiaChi ?? "",
                    NgayVaoLam = request.NgayVaoLam ?? DateTime.Now,
                    TrangThai = request.TrangThai ?? "HoatDong",
                    GioiTinh = request.GioiTinh,
                    AnhId = request.AnhId ?? "ANH_DEFAULT", // Cần tạo ảnh mặc định
                    IsDelete = false
                };

                if (!_quanLyServices.Add<NhanVien>(nhanVien))
                {
                    return BadRequest(new { message = "Không thể thêm nhân viên." });
                }

                return Ok(new { message = "Thêm nhân viên thành công!", nhanVienId = nhanVien.Id });
            }
            catch (Exception ex)
            {
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

                // Kiểm tra xem nhân viên đã được phân công ca này trong ngày chưa
                var existingPhanCong = _quanLyServices.GetList<PhanCongCaLamViec>()
                    .FirstOrDefault(pc => pc.NhanVienId == request.NhanVienId &&
                                         pc.CaLamViecId == request.CaLamViecId &&
                                         pc.Ngay.Date == request.Ngay.Date &&
                                         !pc.IsDelete);

                if (existingPhanCong != null)
                {
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

                if (!_quanLyServices.Add<PhanCongCaLamViec>(phanCong))
                {
                    return BadRequest(new { message = "Không thể thêm phân công ca làm việc." });
                }

                return Ok(new { message = "Phân công ca làm việc thành công!", phanCongId = phanCong.Id });
            }
            catch (Exception ex)
            {
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
        public IActionResult AddPhanCongCaLamViec([FromBody] PhanCongCaLamViecCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Kiểm tra xem nhân viên này đã bị phân công ca này trong ngày này chưa
                var existingPhanCong = _quanLyServices.GetList<PhanCongCaLamViec>()
                    .FirstOrDefault(p =>
                        p.NhanVienId == dto.NhanVienId &&
                        p.CaLamViecId == dto.CaLamViecId &&
                        p.Ngay.Date == dto.Ngay.Value.Date && // So sánh ngày
                        !p.IsDelete);

                if (existingPhanCong != null)
                {
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
                if (_quanLyServices.Add<PhanCongCaLamViec>(phanCong))
                {
                    return Ok(new { message = "Thêm phân công thành công!" });
                }
                else
                {
                    return BadRequest(new { message = "Lỗi khi lưu phân công vào cơ sở dữ liệu." });
                }
            }
            catch (Exception ex)
            {
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

                // Kiểm tra số điện thoại đã tồn tại chưa
                Console.WriteLine("Checking if phone number exists...");
                var existingKH = _quanLyServices.GetList<KhachHang>()
                    .FirstOrDefault(kh => kh.SoDienThoai == request.SoDienThoai && !kh.IsDelete);

                if (existingKH != null)
                {
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
                        if (!_quanLyServices.Add<HinhAnh>(defaultImage))
                        {
                            Console.WriteLine("ERROR: Cannot create default image");
                            return BadRequest(new { message = "Không thể tạo ảnh mặc định." });
                        }
                    }

                    anhId = "ANH_DEFAULT";
                }

                // Tạo Khách hàng
                Console.WriteLine("Creating KhachHang entity...");
                KhachHang khachHang = new KhachHang
                {
                    Id = _quanLyServices.GenerateNewId<KhachHang>("KH", 5),
                    HoTen = request.HoTen,
                    SoDienThoai = request.SoDienThoai,
                    Email = request.Email,
                    DiaChi = request.DiaChi ?? "",
                    NgayDangKy = request.NgayDangKy ?? DateTime.Now,
                    TrangThai = request.TrangThai ?? "Active",
                    GioiTinh = request.GioiTinh,
                    AnhId = anhId,
                    IsDelete = false
                };

                Console.WriteLine($"Generated KhachHang ID: {khachHang.Id}");
                Console.WriteLine("Adding KhachHang to database...");

                if (!_quanLyServices.Add<KhachHang>(khachHang))
                {
                    Console.WriteLine("ERROR: Cannot add KhachHang");
                    return BadRequest(new { message = "Không thể thêm khách hàng." });
                }

                Console.WriteLine("KhachHang added successfully");

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
                        Id = _quanLyServices.GenerateNewId<TheThanhVien>("TTV", 6),
                        KhachHangId = khachHang.Id,
                        Hang = hangVietnamese,
                        DiemTichLuy = request.TheThanhVien.DiemTichLuy,
                        NgayCap = request.TheThanhVien.NgayCap ?? DateTime.Now,
                        IsDelete = false
                    };

                    Console.WriteLine($"Generated TheThanhVien ID: {theThanhVien.Id}");
                    Console.WriteLine("Adding TheThanhVien to database...");

                    if (!_quanLyServices.Add<TheThanhVien>(theThanhVien))
                    {
                        Console.WriteLine("ERROR: Cannot add TheThanhVien");
                        return BadRequest(new { message = "Thêm khách hàng thành công nhưng không thể tạo thẻ thành viên." });
                    }

                    Console.WriteLine("TheThanhVien added successfully");
                }

                Console.WriteLine("=== SUCCESS: All operations completed ===");
                return Ok(new { message = "Thêm khách hàng thành công!", khachHangId = khachHang.Id });
            }
            catch (Exception ex)
            {
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
                        Id = _quanLyServices.GenerateNewId<KiemKe>("KK", 5),
                        NgayKiemKe = request.NgayKiemKe,
                        NhanVienId = request.NhanVienId,
                        SanPhamDonViId = chiTiet.SanPhamDonViId,
                        KetQua = chiTiet.KetQua ?? "",
                        IsDelete = false
                    };

                    Console.WriteLine($"Adding KiemKe: ID={kiemKe.Id}, SanPham={chiTiet.SanPhamDonViId}");

                    if (!_quanLyServices.Add<KiemKe>(kiemKe))
                    {
                        Console.WriteLine($"❌ ERROR: Cannot add KiemKe for {chiTiet.SanPhamDonViId}");
                        failCount++;
                    }
                    else
                    {
                        Console.WriteLine($"✅ KiemKe added successfully");
                        successCount++;
                    }
                }

                Console.WriteLine($"=== RESULT: {successCount} success, {failCount} failed ===");

                if (successCount > 0)
                {
                    string message = failCount > 0
                        ? $"Tạo phiếu kiểm kê thành công {successCount} sản phẩm, {failCount} thất bại."
                        : $"Tạo phiếu kiểm kê thành công cho {successCount} sản phẩm!";

                    return Ok(new { message = message, successCount = successCount, failCount = failCount });
                }
                else
                {
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
        public async Task<IActionResult> ApplyDiscount([FromBody] ApplyDiscountRequest request)
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
                                          mkm.TrangThai == "Active");

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
                
                if (string.IsNullOrEmpty(khachHangId))
                {
                    Console.WriteLine("KhachHangId is null - creating/using default customer...");
                    
                    // Kiểm tra khách hàng mặc định có tồn tại không
                    var khachLe = _quanLyServices.GetList<KhachHang>()
                        .FirstOrDefault(kh => kh.Id == "KH_LE" && !kh.IsDelete);
                    
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
                            TrangThai = "Active",
                            GioiTinh = false,
                            AnhId = "ANH_DEFAULT",
                            IsDelete = false
                        };
                        
                        if (!_quanLyServices.Add<KhachHang>(khachLe))
                        {
                            return BadRequest(new { message = "Không thể tạo khách hàng mặc định." });
                        }
                        
                        Console.WriteLine("✅ Created default customer KH_LE");
                    }
                    
                    khachHangId = "KH_LE";
                    Console.WriteLine($"Using KhachHangId: {khachHangId}");
                }

                // Tạo hóa đơn
                HoaDon hoaDon = new HoaDon
                {
                    Id = _quanLyServices.GenerateNewId<HoaDon>("HD", 5),
                    KhachHangId = khachHangId,
                    NhanVienId = request.NhanVienId,
                    NgayLap = request.NgayLap ?? DateTime.Now,
                    TrangThai = request.TrangThai ?? "Chưa thanh toán",
                    TongTien = 0, // Sẽ tính sau
                    IsDelete = false
                };

                Console.WriteLine($"Generated HoaDon ID: {hoaDon.Id}, TrangThai: {hoaDon.TrangThai}, KhachHangId: {hoaDon.KhachHangId}");

                if (!_quanLyServices.Add<HoaDon>(hoaDon))
                {
                    return BadRequest(new { message = "Không thể tạo hóa đơn." });
                }

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

                    if (!_quanLyServices.Add<ChiTietHoaDon>(ctHoaDon))
                    {
                        return BadRequest(new { message = $"Không thể thêm sản phẩm {chiTiet.SanPhamDonViId}" });
                    }
                }

                

                // Cập nhật tổng tiền
                // Cập nhật tổng tiền
                decimal tongGiamGia = request.TongGiamGia ?? 0;
                hoaDon.TongTien = tongTien - tongGiamGia; 
                if (!_quanLyServices.Update<HoaDon>(hoaDon))
                {
                    return BadRequest(new { message = "Không thể cập nhật tổng tiền hóa đơn." });
                }
                Console.WriteLine($"✅ SUCCESS: Created invoice {hoaDon.Id} with total {tongTien}");

                return Ok(new
                {
                    message = "Tạo hóa đơn thành công!",
                    hoaDonId = hoaDon.Id,
                    tongTien = tongTien
                });
            }
            catch (Exception ex)
            {
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

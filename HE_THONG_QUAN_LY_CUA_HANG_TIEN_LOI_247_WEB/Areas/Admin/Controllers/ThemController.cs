using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
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
            return View();
        }
        
        [Route("/Them/ThemKhachHang")]
        public IActionResult ThemKhachHang()
        {
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
        
        [Route("/Them/ThemNhanSu")]
        public IActionResult ThemNhanSu()
        {
            return View();
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
                    Id = _quanLyServices.GenerateNewId<PhieuNhap>("PN", 6),
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
                            Id = _quanLyServices.GenerateNewId<SanPhamViTri>("SPVT", 8),
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
                viTri.Id = _quanLyServices.GenerateNewId<ViTri>("VT", 6);
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

                Console.WriteLine($"✅ SUCCESS: Added ViTri with ID={viTri.Id}, MaViTri={viTri.MaViTri}");
                return Ok(new { message = "Thêm vị trí mới thành công!", viTriId = viTri.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ EXCEPTION in AddViTri: {ex.Message}");
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

                if (!_quanLyServices.Add<ChuongTrinhKhuyenMai>(chuongTrinh))
                {
                    return BadRequest(new { message = "Không thể thêm chương trình khuyến mãi." });
                }

                // Thêm Điều kiện áp dụng
                if (request.DieuKienApDung != null)
                {
                    DieuKienApDung dieuKien = new DieuKienApDung
                    {
                        Id = _quanLyServices.GenerateNewId<DieuKienApDung>("DKAD", 8),
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
                                Id = _quanLyServices.GenerateNewId<DieuKienApDungDanhMuc>("DKADDM", 10),
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
                                Id = _quanLyServices.GenerateNewId<DieuKienApDungSanPham>("DKADSP", 10),
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
                            Id = _quanLyServices.GenerateNewId<DieuKienApDungToanBo>("DKADTB", 10),
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
                        Id = _quanLyServices.GenerateNewId<MaKhuyenMai>("MKM", 8),
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

        //=========================================API Get Next ID=======================================================================
        [HttpPost]
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

    // Request classes cho Chương trình khuyến mãi
    public class ChuongTrinhKhuyenMaiRequest
    {
        public string Ten { get; set; }
        public string Loai { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string MoTa { get; set; }
        public DieuKienApDungRequest DieuKienApDung { get; set; }
        public MaKhuyenMaiRequest MaKhuyenMai { get; set; }
        public string PhamViApDung { get; set; } // "ToanBo", "DanhMuc", "SanPham"
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
}

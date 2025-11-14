using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThemController : Controller
    {
        private readonly IPhieuDoiTraServices _phieuDoiTraServices;
        private readonly IChinhSachHoanTraServices _chinhSachHoanTraServices;

        public ThemController(
            IPhieuDoiTraServices phieuDoiTraServices,
            IChinhSachHoanTraServices chinhSachHoanTraServices)
        {
            _phieuDoiTraServices = phieuDoiTraServices;
            _chinhSachHoanTraServices = chinhSachHoanTraServices;
        }

        [Route("/Them/ThemBanHang")]
        public IActionResult ThemDanhMucViTRi()
        {
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
            return View();
        }
    }
}

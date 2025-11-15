using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyKhoHangController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;
        private readonly ITonKhoServices _tonKhoServices;

        public QuanLyKhoHangController(IQuanLyServices quanLySevices, ITonKhoServices tonKhoServices)
        {
            _quanLySevices = quanLySevices;
            _tonKhoServices = tonKhoServices;
        }

        [Route("/QuanLyKhoHang/DanhSachHangTonKho")]
        public IActionResult DanhSachHangTonKho()
        {
            var lstHangTonKho = _quanLySevices.GetList<TonKho>();
            ViewData["lstHangTonKho"] = lstHangTonKho;
            return View();
        }

        [Route("/QuanLyKhoHang/DanhSachPhieuNhap")]
        public IActionResult DanhSachPhieuNhap()
        {
            var lstPhieuNhap = _quanLySevices.GetList<PhieuNhap>();
            ViewData["lstPhieuNhap"] = lstPhieuNhap;
            return View();
        }

        [Route("/QuanLyKhoHang/DanhSachPhieuXuat")]
        public IActionResult DanhSachPhieuXuat()
        {
            var lstPhieuXuat = _quanLySevices.GetList<PhieuXuat>();
            ViewData["lstPhieuXuat"] = lstPhieuXuat;
            return View();
        }

        [Route("/QuanLyKhoHang/DanhSachKiemKe")]
        public IActionResult KiemKeSanPham()
        {
            var lstKiemKe = _quanLySevices.GetList<KiemKe>();
            ViewData["lstKiemKe"] = lstKiemKe;
            return View();
        }

        [Route("/QuanLyKhoHang/ViTriSanPham")]
        public IActionResult ViTriSanPham()
        {
            var lstSanPhamViTri = _quanLySevices.GetList<SanPhamViTri>();
            ViewData["lstSanPhamViTri"] = lstSanPhamViTri;
            return View();
        }

        [Route("/QuanLyKhoHang/XemChiTietKiemKe/{id}")]
        public IActionResult XemChiTietKiemKe(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("KiemKeSanPham");
            }

            var phieuKiemKe = _tonKhoServices.GetKiemKeById(id);
            if (phieuKiemKe == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phiếu kiểm kê";
                return RedirectToAction("KiemKeSanPham");
            }

            ViewData["PhieuKiemKe"] = phieuKiemKe;
            return View();
        }

        [Route("/QuanLyKhoHang/XemChiTietPhieuXuat/{id}")]
        public IActionResult XemChiTietPhieuXuat(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("DanhSachPhieuXuat");
            }

            var phieuXuat = _tonKhoServices.GetPhieuXuatById(id);
            if (phieuXuat == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phiếu xuất";
                return RedirectToAction("DanhSachPhieuXuat");
            }

            var chiTietList = _tonKhoServices.GetChiTietPhieuXuat(id);
            ViewData["PhieuXuat"] = phieuXuat;
            ViewData["ChiTietPhieuXuat"] = chiTietList;
            return View();
        }

        [Route("/QuanLyKhoHang/XemNhapKho/{id}")]
        public IActionResult XemNhapKho(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("DanhSachPhieuNhap");
            }

            var phieuNhap = _tonKhoServices.GetPhieuNhapById(id);
            if (phieuNhap == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phiếu nhập";
                return RedirectToAction("DanhSachPhieuNhap");
            }

            var chiTietList = _tonKhoServices.GetChiTietPhieuNhap(id);
            ViewData["PhieuNhap"] = phieuNhap;
            ViewData["ChiTietPhieuNhap"] = chiTietList;
            return View();
        }

        [Route("/get-next-id-PN")]
        [HttpPost]
        public Task<IActionResult> GetNextIdPN([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLySevices.GenerateNewId<PhieuNhap>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }


        //cập nhật vị trí sản phẩm
        [HttpPost]
        [Route("/CapNhatSanPhamViTri")]
        public IActionResult CapNhatSanPhamViTri([FromBody] CapNhatSPVTRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.SanPhamDonViId) || string.IsNullOrEmpty(request.ViTriIdCu))
                {
                    return BadRequest(new { message = "Thiếu thông tin sản phẩm hoặc vị trí!" });
                }

                // Lấy SanPhamViTri hiện tại với composite key
                var sanPhamViTri = _quanLySevices.GetById<SanPhamViTri>(request.SanPhamDonViId, request.ViTriIdCu);
                
                if (sanPhamViTri == null)
                {
                    return NotFound(new { message = "Không tìm thấy sản phẩm tại vị trí này!" });
                }

                // Nếu chuyển vị trí
                if (request.ViTriIdMoi != request.ViTriIdCu)
                {
                    // Xóa mềm bản ghi cũ
                    _quanLySevices.SoftDelete(sanPhamViTri);
                    
                    // Kiểm tra xem sản phẩm đã có ở vị trí mới chưa
                    var existingAtNewLocation = _quanLySevices.GetById<SanPhamViTri>(request.SanPhamDonViId, request.ViTriIdMoi);
                    
                    if (existingAtNewLocation != null && !existingAtNewLocation.IsDelete)
                    {
                        // Nếu đã tồn tại ở vị trí mới, cộng dồn số lượng
                        existingAtNewLocation.SoLuong += request.SoLuong;
                        _quanLySevices.Update(existingAtNewLocation);
                    }
                    else
                    {
                        // Tạo bản ghi mới ở vị trí mới
                        var sanPhamViTriMoi = new SanPhamViTri
                        {
                            Id = _quanLySevices.GenerateNewId<SanPhamViTri>("SPVT", 8),
                            SanPhamDonViId = request.SanPhamDonViId,
                            ViTriId = request.ViTriIdMoi,
                            SoLuong = request.SoLuong,
                            IsDelete = false
                        };
                        
                        _quanLySevices.Add(sanPhamViTriMoi);
                    }
                }
                else
                {
                    // Chỉ cập nhật số lượng
                    sanPhamViTri.SoLuong = request.SoLuong;
                    _quanLySevices.Update(sanPhamViTri);
                }

                return Ok(new { message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CapNhatSanPhamViTri: {ex.Message}");
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }

        //xóa vị trí sane phẩm
        [HttpPost]
        [Route("/XoaSanPhamViTri")]
        public IActionResult XoaSanPhamViTri([FromBody] XoaSPVTRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.SanPhamDonViId) || string.IsNullOrEmpty(request.ViTriId))
                {
                    return BadRequest(new { message = "Thiếu thông tin!" });
                }

                var sanPhamViTri = _quanLySevices.GetById<SanPhamViTri>(request.SanPhamDonViId, request.ViTriId);
                
                if (sanPhamViTri == null)
                {
                    return NotFound(new { message = "Không tìm thấy!" });
                }

                var result = _quanLySevices.SoftDelete(sanPhamViTri);
                
                if (result)
                {
                    return Ok(new { message = "Xóa thành công!" });
                }
                
                return BadRequest(new { message = "Không thể xóa!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in XoaSanPhamViTri: {ex.Message}");
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }

        //  cập nhật vị trí
        [HttpPost]
        [Route("/CapNhatViTri")]
        public IActionResult CapNhatViTri([FromBody] CapNhatViTriRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Id))
                {
                    return BadRequest(new { message = "Thiếu ID vị trí!" });
                }

                var viTri = _quanLySevices.GetById<ViTri>(request.Id);
                
                if (viTri == null)
                {
                    return NotFound(new { message = "Không tìm thấy vị trí!" });
                }

                // Kiểm tra mã vị trí mới có trùng không (nếu thay đổi mã)
                if (viTri.MaViTri != request.MaViTri)
                {
                    var existingMaViTri = _quanLySevices.GetList<ViTri>()
                        .FirstOrDefault(vt => vt.MaViTri == request.MaViTri && vt.Id != request.Id);
                    
                    if (existingMaViTri != null)
                    {
                        return BadRequest(new { message = "Mã vị trí đã tồn tại!" });
                    }
                }

                viTri.MaViTri = request.MaViTri;
                viTri.LoaiViTri = request.LoaiViTri;
                viTri.MoTa = request.MoTa;

                var result = _quanLySevices.Update(viTri);
                
                if (result)
                {
                    return Ok(new { message = "Cập nhật thành công!" });
                }
                
                return BadRequest(new { message = "Không thể cập nhật!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CapNhatViTri: {ex.Message}");
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }

        //xóa vị trí
        [HttpPost]
        [Route("/XoaViTri")]
        public IActionResult XoaViTri([FromBody] XoaViTriRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Id))
                {
                    return BadRequest(new { message = "Thiếu ID vị trí!" });
                }

                var viTri = _quanLySevices.GetById<ViTri>(request.Id);
                
                if (viTri == null)
                {
                    return NotFound(new { message = "Không tìm thấy vị trí!" });
                }

                // Kiểm tra xem vị trí có đang được sử dụng không
                var sanPhamViTris = _quanLySevices.GetList<SanPhamViTri>();
                var dangSuDung = sanPhamViTris.Any(sp => sp.ViTriId == request.Id);
                
                if (dangSuDung)
                {
                    return BadRequest(new { message = "Vị trí đang được sử dụng, không thể xóa!" });
                }

                var result = _quanLySevices.SoftDelete(viTri);
                
                if (result)
                {
                    return Ok(new { message = "Xóa thành công!" });
                }
                
                return BadRequest(new { message = "Không thể xóa!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in XoaViTri: {ex.Message}");
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }
    }


    public class CapNhatSPVTRequest
    {
        public string SanPhamDonViId { get; set; }
        public string ViTriIdCu { get; set; }
        public string ViTriIdMoi { get; set; }
        public int SoLuong { get; set; }
    }

    public class XoaSPVTRequest
    {
        public string SanPhamDonViId { get; set; }
        public string ViTriId { get; set; }
    }

    public class CapNhatViTriRequest
    {
        public string Id { get; set; }
        public string MaViTri { get; set; }
        public string LoaiViTri { get; set; }
        public string MoTa { get; set; }
    }

    public class XoaViTriRequest
    {
        public string Id { get; set; }
    }
}

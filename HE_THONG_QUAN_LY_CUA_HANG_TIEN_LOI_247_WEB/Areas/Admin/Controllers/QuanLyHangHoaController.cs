using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [ApiController]
    [Route("API")]
    [Area("Admin")]
    public class QuanLyHangHoaController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyHangHoaController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/QuanLyHangHoa/DanhSachDanhMuc")]
        public IActionResult DanhSachDanhMuc()
        {
            var lstDanhMuc = _quanLySevices.GetList<DanhMuc>();

            ViewData["lstDanhMuc"] = lstDanhMuc;
            return View();
        }
        [Route("/QuanLyHangHoa/DanhSachDonVi")]
        public IActionResult DanhSachDonVi()
        {
            var lstDonViDoLuong = _quanLySevices.GetList<DonViDoLuong>();
            
            ViewData["lstDonViDoLuong"] = lstDonViDoLuong;
            
            return View();
        }
        
        [Route("/QuanLyHangHoa/DanhSachNhanHieu")]
        public IActionResult DanhSachNhanHieu()
        {
            var lstNhanHieu = _quanLySevices.GetList<NhanHieu>();
            
            ViewData["lstNhanHieu"] = lstNhanHieu;
            
            return View();
        }
        
        [Route("/QuanLyHangHoa/DanhSachSanPham")]
        public IActionResult DanhSachSanPham()
        {
            var lstDanhMuc = _quanLySevices.GetList<DanhMuc>();

            ViewData["lstDanhMuc"] = lstDanhMuc;

            var lstSanPham = _quanLySevices.GetList<SanPham>();

            ViewData["lstSanPham"] = lstSanPham;

            var lstNhanHieu = _quanLySevices.GetList<NhanHieu>();

            ViewData["lstNhanHieu"] = lstNhanHieu;

            var lstDonVi = _quanLySevices.GetList<DonViDoLuong>();

            ViewData["lstDonVi"] = lstDonVi;

            return View();
        }
        [Route("/QuanLyHangHoa/LichSuGiaBan")]
        public IActionResult LichSuGiaBan()
        {
            var lstLichSuGiaBan = _quanLySevices.GetList<LichSuGiaBan>();
            
            ViewData["lstLichSuGiaBan"] = lstLichSuGiaBan;
            
            return View();
        }

        [HttpPost("get-next-id-SP")]
        public Task<IActionResult> GetNextIdSP([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLySevices.GenerateNewId<SanPham>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString()))}));
        }

        [HttpPost("get-next-id-NH")]
        public Task<IActionResult> GetNextIdNH([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLySevices.GenerateNewId<NhanHieu>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }

        [HttpPost("add-SP")]
        public async Task<IActionResult> AddSanPham([FromForm] ProductFormData request)         
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }

                var images = request.ImagesUpload; // Các ảnh tải lên
                var productId = request.ProductId; // Mã sản phẩm
                var productName = request.ProductName; // Tên sản phẩm
                var brand = request.Brand; // Nhãn hiệu
                var categories = request.Categories; // Các danh mục
                var unit = request.Unit; // Đơn vị cơ sở
                var conversionFactor = request.ConversionFactor; // Hệ số quy đổi
                var price = request.Price; // Giá bán
                var description = request.Description; // Mô tả sản phẩm
                var status = request.Status; // Trạng thái sản phẩm


                SanPham sanPham = new SanPham
                {
                    Id = productId,
                    Ten = productName,
                    NhanHieuId = brand,
                    MoTa = description,
                };

                if (!_quanLySevices.Add<SanPham>(sanPham))
                {
                    return BadRequest("không thể thêm sản phẩm");
                }

                SanPhamDonVi sanPhamDonVi = new SanPhamDonVi
                {
                    SanPhamId = sanPham.Id,
                    DonViId = unit,
                    Id = _quanLySevices.GenerateNewId<SanPhamDonVi>("SPDV", 8),
                    HeSoQuyDoi = conversionFactor,
                    GiaBan = price,
                    TrangThai = status
                };

                if (!_quanLySevices.Add<SanPhamDonVi>(sanPhamDonVi))
                {
                    return BadRequest("không thể thêm sản phẩm đơn vị");
                }

                List<HinhAnh> hinhAnhs = new List<HinhAnh>();

                foreach (IFormFile image in images)
                {
                    HinhAnh hinhAnh = new HinhAnh
                    {
                        Id = _quanLySevices.GenerateNewId<HinhAnh>("HA", 6),
                        TenAnh = image.FileName,
                        Anh = await _quanLySevices.ConvertImageToByteArray(image)
                    };

                    hinhAnhs.Add(hinhAnh);
                }

                foreach (HinhAnh hinhAnh in hinhAnhs)
                {
                    if (!_quanLySevices.Add<HinhAnh>(hinhAnh))
                    {
                        return BadRequest("không thể thêm ảnh");
                    }
                    AnhSanPhamDonVi anhSanPhamDonVi = new AnhSanPhamDonVi
                    {
                        SanPhamDonViId = sanPhamDonVi.Id,
                        AnhId = hinhAnh.Id,
                    };
                    if (!_quanLySevices.Add<AnhSanPhamDonVi>(anhSanPhamDonVi))
                    {
                        return BadRequest("không thể thêm ảnh sản phẩm đơn vị");
                    }
                }




                return Ok(new { message = "Thêm sản phẩm thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi thêm sản phẩm: {ex.Message}" });
            }
            
        }

        [HttpPost("add-NH")]
        public async Task<IActionResult> AddNhanHieu([FromBody] NhanHieu nhanHieu)
        {
            try
            {
                if (nhanHieu.Id == null || nhanHieu.Ten == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }


                if (_quanLySevices.Add<NhanHieu>(nhanHieu))
                {
                    return Ok(new { message = "Thêm nhãn hiệu thành công!" });
                }

                return BadRequest("thêm nhãn hiệu thất bại");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi thêm nhãn hiệu: {ex.Message}" });
            }

        }

    }


    public class ProductFormData
    {
        // Các ảnh tải lên
        public List<IFormFile> ImagesUpload { get; set; }

        // Các trường thông tin khác
        public string ProductId { get; set; } // Mã sản phẩm
        public string ProductName { get; set; } // Tên sản phẩm
        public string Brand { get; set; } // Nhãn hiệu
        public List<string> Categories { get; set; } // Danh mục (có thể chọn nhiều)
        public string Unit { get; set; } // Đơn vị cơ sở
        public decimal ConversionFactor { get; set; } // Hệ số quy đổi
        public decimal Price { get; set; } // Giá bán
        public string Description { get; set; } // Mô tả sản phẩm
        public string Status { get; set; } // Trạng thái sản phẩm (Còn hàng, Hết hàng, Ngừng kinh doanh)
    }



}

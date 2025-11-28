using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Authorize]
    [ApiController]
    [Route("API")]
    [Area("Admin")]
    public class QuanLyHangHoaController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        private readonly IRealtimeNotifier _notifier;

        public QuanLyHangHoaController(IQuanLyServices quanLySevices, IRealtimeNotifier notifier)
        {
            _quanLySevices = quanLySevices;
            _notifier = notifier;
        }

        //=========================================LoadData=========================================================

        [Authorize(Roles = "ADMIN,NV_BANHANG")]
        [Route("/QuanLyHangHoa/DanhSachDanhMuc")]
        public IActionResult DanhSachDanhMuc()
        {
            var lstDanhMuc = _quanLySevices.GetList<DanhMuc>();

            ViewData["lstDanhMuc"] = lstDanhMuc;
            return View();
        }

        [Authorize(Roles = "ADMIN,NV_BANHANG")]
        [Route("/QuanLyHangHoa/DanhSachDonVi")]
        public IActionResult DanhSachDonVi()
        {
            var lstDonViDoLuong = _quanLySevices.GetList<DonViDoLuong>();

            ViewData["lstDonViDoLuong"] = lstDonViDoLuong;

            return View();
        }

        [Authorize(Roles = "ADMIN,NV_BANHANG")]
        [Route("/QuanLyHangHoa/DanhSachNhanHieu")]
        public IActionResult DanhSachNhanHieu()
        {
            var lstNhanHieu = _quanLySevices.GetList<NhanHieu>();

            ViewData["lstNhanHieu"] = lstNhanHieu;

            return View();
        }

        [Authorize(Roles = "ADMIN,NV_BANHANG")]
        [Route("/QuanLyHangHoa/DanhSachSanPham")]
        public IActionResult DanhSachSanPham()
        {
            var lstSanPham = _quanLySevices.GetList<SanPham>();

            ViewData["lstSanPham"] = lstSanPham;

            var lstDonVi = _quanLySevices.GetList<DonViDoLuong>();

            ViewData["lstDonVi"] = lstDonVi;

            var lstSanPhamDonVi = _quanLySevices.GetList<SanPhamDonVi>();

            ViewData["lstSanPhamDonVi"] = lstSanPhamDonVi;

            return View();
        }

        [Authorize(Roles = "ADMIN,NV_BANHANG")]
        [Route("/QuanLyHangHoa/DanhSachHangHoa")]
        public IActionResult DanhSachHangHoa()
        {
            var lstSanPham = _quanLySevices.GetList<SanPham>();

            ViewData["lstSanPham"] = lstSanPham;

            var lstDanhMuc = _quanLySevices.GetList<DanhMuc>();

            ViewData["lstDanhMuc"] = lstDanhMuc;

            var lstNhanHieu = _quanLySevices.GetList<NhanHieu>();

            ViewData["lstNhanHieu"] = lstNhanHieu;

            return View();
        }

        [Authorize(Roles = "ADMIN,NV_BANHANG")]
        [Route("/QuanLyHangHoa/LichSuGiaBan")]
        public IActionResult LichSuGiaBan()
        {
            var lstLichSuGiaBan = _quanLySevices.GetList<LichSuGiaBan>();

            ViewData["lstLichSuGiaBan"] = lstLichSuGiaBan;

            return View();
        }

        //=========================================GetNextId=======================================================================

        [HttpPost("get-next-id-SP")]
        public Task<IActionResult> GetNextIdSP([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLySevices.GenerateNewId<SanPham>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }

        [HttpPost("get-next-id-NH")]
        public Task<IActionResult> GetNextIdNH([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLySevices.GenerateNewId<NhanHieu>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }

        [HttpPost("get-next-id-DM")]
        public Task<IActionResult> GetNextIdDM([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLySevices.GenerateNewId<DanhMuc>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }

        [HttpPost("get-next-id-DV")]
        public Task<IActionResult> GetNextIdDV([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLySevices.GenerateNewId<DonViDoLuong>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }


        //=========================================AddData=======================================================================


        [HttpPost("add-SP")]
        public async Task<IActionResult> AddSanPham([FromForm] SanPhamDTO request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }


                SanPham sanPham = new SanPham
                {
                    Id = request.Id,
                    Ten = request.Ten,
                    NhanHieuId = request.NhanHieu,
                };

                await _quanLySevices.BeginTransactionAsync();

                _quanLySevices.Add(sanPham);

                foreach (string danhMucId in request.DanhMucs)
                {
                    SanPhamDanhMuc sanPhamDanhMuc = new SanPhamDanhMuc
                    {
                        SanPhamId = request.Id,
                        DanhMucId = danhMucId,
                        Id = _quanLySevices.GenerateNewId<SanPhamDanhMuc>("SPDM", 8)
                    };
                    _quanLySevices.Add(sanPhamDanhMuc);
                }

                if (!await _quanLySevices.CommitAsync("HangHoa"))
                {
                    return BadRequest("Thêm sản phẩm thất bại");
                }

                return Ok(new { message = "Thêm sản phẩm thành công!" });
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi thêm sản phẩm: {ex.Message}" });
            }

        }

        [HttpPost("addSPDV")]
        public async Task<IActionResult> AddSanPhamDonVi([FromForm] SanPhamDonViDTO request)
        {
            try
            {
                if (request == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ!" });

                await _quanLySevices.BeginTransactionAsync();

                // Tạo SanPhamDonVi
                var spdv = new SanPhamDonVi
                {
                    Id = _quanLySevices.GenerateNewId<SanPhamDonVi>("SPDV", 8),
                    SanPhamId = request.SanPhamId,
                    DonViId = request.DonViId,
                    GiaBan = request.GiaBan,
                    HeSoQuyDoi = request.HeSoQuyDoi,
                    TrangThai = request.TrangThai,
                };
                _quanLySevices.Add(spdv);

                if (request.ImagesUpload != null && request.ImagesUpload.Any())
                {
                    foreach (var file in request.ImagesUpload)
                    {
                        var anh = new HinhAnh
                        {
                            Id = _quanLySevices.GenerateNewId<HinhAnh>("ANH", 7),
                            TenAnh = file.FileName,
                            Anh = await _quanLySevices.ConvertImageToByteArray(file)
                        };
                        _quanLySevices.Add<HinhAnh>(anh);

                        var anhSanPhamDonVi = new AnhSanPhamDonVi
                        {
                            SanPhamDonViId = spdv.Id,
                            AnhId = anh.Id
                        };
                        _quanLySevices.Add<AnhSanPhamDonVi>(anhSanPhamDonVi);
                    }
                }

                // ✅ CommitAsync đã tự động Rollback khi thất bại
                if (!await _quanLySevices.CommitAsync("SanPham"))
                {
                    return BadRequest(new { message = "Không thể thêm sản phẩm - đơn vị!" });
                }

                return Ok(new { message = "Thêm sản phẩm thành công!" });
            }
            catch (Exception ex)
            {
                // ✅ Chỉ Rollback khi có exception NGOÀI CommitAsync
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống!",
                    detail = ex.Message
                });
            }
        }



        [HttpPost("add-NH")]
        public async Task<IActionResult> AddNhanHieu([FromBody] NhanHieu nhanHieu)
        {
            try
            {
                if (nhanHieu.Id == null || nhanHieu.Ten == null)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                await _quanLySevices.BeginTransactionAsync();

                _quanLySevices.Add<NhanHieu>(nhanHieu);

                if (await _quanLySevices.CommitAsync("NhanHieu"))
                {
                    return Ok(new { message = "Thêm nhãn hiệu thành công!" });
                }

                return BadRequest(new { message = "Thêm nhãn hiệu thất bại" });
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi thêm nhãn hiệu: {ex.Message}" });
            }

        }


        //[Authorize(Policy = "Create")]
        [HttpPost("add-DM")]
        public async Task<IActionResult> AddDanhMuc([FromBody] DanhMuc danhMuc)
        {
            try
            {
                if (danhMuc.Id == null || danhMuc.Ten == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }

                await _quanLySevices.BeginTransactionAsync();

                _quanLySevices.Add<DanhMuc>(danhMuc);

                if (await _quanLySevices.CommitAsync("DanhMuc"))
                {

                    return Ok(new { message = "Thêm danh mục thành công!" });
                }

                return BadRequest("thêm danh mục thất bại");
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi thêm danh mục: {ex.Message}" });
            }

        }

        [HttpPost("add-DV")]
        public async Task<IActionResult> AddDonVi([FromBody] DonViDoLuong donViDoLuong)
        {
            try
            {
                if (donViDoLuong.Id == null || donViDoLuong.Ten == null || donViDoLuong.KyHieu == null)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                await _quanLySevices.BeginTransactionAsync();

                _quanLySevices.Add<DonViDoLuong>(donViDoLuong);

                if (await _quanLySevices.CommitAsync("DonVi"))
                {
                    return Ok(new { message = "Thêm đơn vị thành công!" });
                }

                return BadRequest(new { message = "Thêm đơn vị thất bại" });
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi thêm đơn vị: {ex.Message}" });
            }
        }

        //=========================================EditData=======================================================================
        //[Authorize(Policy = "Edit")]
        [HttpPost("edit-DM")]
        public async Task<IActionResult> EditDanhMuc([FromBody] DanhMuc danhMuc)
        {
            if (!User.HasClaim("Permission", "SanPham.Create"))
            {
                return Forbid();  // Trả về HTTP 403 Forbidden nếu không có quyền
            }

            try
            {
                if (danhMuc.Id == null || danhMuc.Ten == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }

                await _quanLySevices.BeginTransactionAsync();

                _quanLySevices.Update<DanhMuc>(danhMuc);

                if (await _quanLySevices.CommitAsync("DanhMuc"))
                {

                    return Ok(new { message = "Sửa danh mục thành công!" });
                }

                return BadRequest("Sửa danh mục thất bại");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi sửa danh mục: {ex.Message}" });
            }

        }

        [HttpPost("editSP")]
        public async Task<IActionResult> EditSanPham([FromForm] SanPhamDTO request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }

                await _quanLySevices.BeginTransactionAsync();

                SanPham sanPham = _quanLySevices.GetById<SanPham>(request.Id);

                sanPham.Ten = request.Ten;

                sanPham.NhanHieuId = request.NhanHieu;

                _quanLySevices.Update<SanPham>(sanPham);

                List<string> newDanhMucs = request.DanhMucs;

                List<string> oldDMs = new List<string>();

                foreach (SanPhamDanhMuc sanPhamDanhMuc in sanPham.SanPhamDanhMucs)
                {
                    oldDMs.Add(sanPhamDanhMuc.DanhMucId);
                }

                var toAdd = newDanhMucs.Except(oldDMs);

                foreach (string danhMuc in toAdd)
                {
                    SanPhamDanhMuc sanPhamDanhMuc = new SanPhamDanhMuc
                    {
                        SanPhamId = sanPham.Id,
                        DanhMucId = danhMuc,
                        Id = _quanLySevices.GenerateNewId<SanPhamDanhMuc>("SPDM", 8)
                    };
                    _quanLySevices.Add<SanPhamDanhMuc>(sanPhamDanhMuc);
                }

                var toRemove = oldDMs.Except(newDanhMucs);

                foreach (string danhMucs in toRemove)
                {
                    SanPhamDanhMuc sanPhamDanhMuc = _quanLySevices.GetById<SanPhamDanhMuc>(sanPham.Id, danhMucs);
                    if (sanPhamDanhMuc != null)
                    {
                        _quanLySevices.SoftDelete<SanPhamDanhMuc>(sanPhamDanhMuc);
                    }
                }

                if (!await _quanLySevices.CommitAsync("HangHoa"))
                {
                    return BadRequest("Sửa sản phẩm thất bại");
                }

                return Ok(new { message = "Sửa sản phẩm thành công!" });
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi thêm sửa phẩm: {ex.Message}" });
            }

        }

        [HttpPost("edit-DV")]
        public async Task<IActionResult> EditDonVi([FromBody] DonViDoLuong donViDoLuong)
        {
            try
            {
                if (donViDoLuong == null || string.IsNullOrWhiteSpace(donViDoLuong.Id))
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                if (string.IsNullOrWhiteSpace(donViDoLuong.Ten))
                {
                    return BadRequest(new { message = "Tên đơn vị không được để trống." });
                }

                if (string.IsNullOrWhiteSpace(donViDoLuong.KyHieu))
                {
                    return BadRequest(new { message = "Ký hiệu không được để trống." });
                }

                await _quanLySevices.BeginTransactionAsync();

                _quanLySevices.Update<DonViDoLuong>(donViDoLuong);

                if (!await _quanLySevices.CommitAsync("DonVi"))
                {
                    return BadRequest(new { message = "Không thể cập nhật đơn vị." });
                }

                return Ok(new { message = "Sửa đơn vị thành công!" });
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi sửa đơn vị: {ex.Message}" });
            }
        }

        [HttpPost("edit-NH")]
        public async Task<IActionResult> EditNhanHieu([FromBody] NhanHieu nhanHieu)
        {
            try
            {
                if (nhanHieu == null || string.IsNullOrWhiteSpace(nhanHieu.Id))
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                if (string.IsNullOrWhiteSpace(nhanHieu.Ten))
                {
                    return BadRequest(new { message = "Tên nhãn hiệu không được để trống." });
                }

                await _quanLySevices.BeginTransactionAsync();

                _quanLySevices.Update<NhanHieu>(nhanHieu);

                if (!await _quanLySevices.CommitAsync("NhanHieu"))
                {
                    return BadRequest(new { message = "Không thể cập nhật nhãn hiệu." });
                }

                return Ok(new { message = "Sửa nhãn hiệu thành công!" });
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi sửa nhãn hiệu: {ex.Message}" });
            }
        }

        [HttpPost("editSPDV")]
        public async Task<IActionResult> EditSanPhamDonVi([FromForm] SanPhamDonViDTO request)
        {
            try
            {
                if (request == null)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ!" });

                // Validate
                if (string.IsNullOrWhiteSpace(request.SanPhamId))
                    return BadRequest(new { message = "Vui lòng chọn sản phẩm!" });

                if (string.IsNullOrWhiteSpace(request.DonViId))
                    return BadRequest(new { message = "Vui lòng chọn đơn vị!" });

                if (request.GiaBan <= 0)
                    return BadRequest(new { message = "Giá bán phải lớn hơn 0!" });

                await _quanLySevices.BeginTransactionAsync();

                // Lấy entity hiện tại
                var spdv = _quanLySevices.GetById<SanPhamDonVi>(request.SanPhamId, request.DonViId);
                if (spdv == null)
                {
                    await _quanLySevices.RollbackAsync();
                    return NotFound(new { message = "Không tìm thấy sản phẩm - đơn vị!" });
                }

                // Cập nhật thông tin
                spdv.GiaBan = request.GiaBan;
                spdv.HeSoQuyDoi = request.HeSoQuyDoi;
                spdv.TrangThai = request.TrangThai;

                _quanLySevices.Update(spdv);

                if (request.ImagesUpload != null && request.ImagesUpload.Any())
                {

                    var oldImages = _quanLySevices.GetList<AnhSanPhamDonVi>()
                        .Where(a => a.SanPhamDonViId == spdv.Id)
                        .ToList();

                    foreach (var oldImg in oldImages)
                    {
                        _quanLySevices.HardDelete(oldImg);
                        if (!await _quanLySevices.CommitAsync())
                        {
                            await _quanLySevices.BeginTransactionAsync();
                            _quanLySevices.SoftDelete(oldImg);
                        }
                        else
                        {
                            await _quanLySevices.BeginTransactionAsync();
                        }


                        // Thêm ảnh mới
                       
                    }
                    foreach (var file in request.ImagesUpload)
                    {
                        var anh = new HinhAnh
                        {
                            Id = _quanLySevices.GenerateNewId<HinhAnh>("ANH", 7),
                            TenAnh = file.FileName,
                            Anh = await _quanLySevices.ConvertImageToByteArray(file)
                        };
                        _quanLySevices.Add(anh);

                        var anhSanPhamDonVi = new AnhSanPhamDonVi
                        {
                            SanPhamDonViId = spdv.Id,
                            AnhId = anh.Id
                        };
                        _quanLySevices.Add(anhSanPhamDonVi);
                    }
                }

                if (await _quanLySevices.CommitAsync("SanPham"))
                {
                    return Ok(new { message = "Sửa sản phẩm thành công!" });

                }

                return BadRequest(new { message = "Không thể cập nhật sản phẩm - đơn vị!" });


            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                Console.WriteLine($"Error in EditSanPhamDonVi: {ex.Message}");
                return StatusCode(500, new
                {
                    message = "Lỗi hệ thống!",
                    detail = ex.Message
                });
            }
        }

        //=========================================DeleteData=======================================================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { message = "ID không hợp lệ." });
                }

                await _quanLySevices.BeginTransactionAsync();
                var danhMuc = _quanLySevices.GetById<DanhMuc>(id);
                if (danhMuc == null)
                {
                    await _quanLySevices.RollbackAsync();
                    return NotFound(new { message = "Không tìm thấy danh mục." });
                }

                _quanLySevices.HardDelete(danhMuc);

                if (!await _quanLySevices.CommitAsync("DanhMuc"))
                {
                    await _quanLySevices.BeginTransactionAsync();
                    _quanLySevices.SoftDelete(danhMuc);
                }
                else
                {
                    return Ok(new { message = "Xóa thành công!" });
                }

                if (await _quanLySevices.CommitAsync("DanhMuc"))
                {
                    return Ok(new { message = "Xóa thành công!" });
                }
                else
                {
                    return BadRequest(new { message = "Không thể xóa danh mục. Có thể danh mục đang được sử dụng." });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi xóa danh mục: {ex.Message}" });
            }

        }

        [HttpDelete("deleteSP{id}")]
        public async Task<IActionResult> DeleteSP(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { message = "ID không hợp lệ." });
                }
                await _quanLySevices.BeginTransactionAsync();

                var sanPham = _quanLySevices.GetById<SanPham>(id);
                if (sanPham == null)
                {
                    await _quanLySevices.RollbackAsync();
                    return NotFound(new { message = "Không tìm thấy sản phẩm." });
                }

                _quanLySevices.HardDelete(sanPham);

                if (!await _quanLySevices.CommitAsync("HangHoa"))
                {
                    await _quanLySevices.BeginTransactionAsync();
                    _quanLySevices.SoftDelete(sanPham);
                    if (!await _quanLySevices.CommitAsync("HangHoa"))
                    {
                        return BadRequest(new { message = "Không thể xóa sản phẩm. Có thể sản phẩm đang được sử dụng." });
                    }
                    return Ok(new { message = "Xóa thành công!" });
                }
                else
                {
                    return Ok(new { message = "Xóa thành công!" });
                }

            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi xóa sản phẩm: {ex.Message}" });
            }
        }

        [HttpDelete("deleteSPDV")]
        public async Task<IActionResult> DeleteSanPhamDonVi(string sanPhamId, string donViId)
        {
            try
            {
                if (string.IsNullOrEmpty(sanPhamId) || string.IsNullOrEmpty(donViId))
                {
                    return BadRequest(new { message = "ID không hợp lệ." });
                }

                await _quanLySevices.BeginTransactionAsync();

                var sanPhamDonVi = _quanLySevices.GetById<SanPhamDonVi>(sanPhamId, donViId);
                if (sanPhamDonVi == null)
                {
                    await _quanLySevices.RollbackAsync();
                    return NotFound(new { message = "Không tìm thấy sản phẩm." });
                }

                _quanLySevices.HardDelete(sanPhamDonVi);

                if (!await _quanLySevices.CommitAsync("SanPham"))
                {
                    await _quanLySevices.BeginTransactionAsync();
                    _quanLySevices.SoftDelete(sanPhamDonVi);
                    if (!await _quanLySevices.CommitAsync("SanPham"))
                    {
                        return BadRequest(new { message = "Không thể xóa sản phẩm - đơn vị. Có thể sản phẩm - đơn vị đang được sử dụng." });
                    }
                    else
                    {
                        return Ok(new { message = "Xóa thành công!" });
                    }
                }
                else
                {
                    return Ok(new { message = "Xóa thành công!" });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi xóa sản phẩm - đơn vị: {ex.Message}" });
            }
        }

        [HttpDelete("delete-DV/{id}")]
        public async Task<IActionResult> DeleteDonVi(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { message = "ID không hợp lệ." });
                }

                await _quanLySevices.BeginTransactionAsync();

                var donVi = _quanLySevices.GetById<DonViDoLuong>(id);
                if (donVi == null)
                {
                    await _quanLySevices.RollbackAsync();
                    return NotFound(new { message = "Không tìm thấy đơn vị." });
                }


                _quanLySevices.HardDelete<DonViDoLuong>(donVi);

                // Thử HardDelete trước
                if (!await _quanLySevices.CommitAsync("DonVi"))
                {
                    await _quanLySevices.BeginTransactionAsync();
                    _quanLySevices.SoftDelete<DonViDoLuong>(donVi);
                    if (!await _quanLySevices.CommitAsync("DonVi"))
                    {
                        return BadRequest(new { message = "Không thể xóa đơn vị. Có thể đơn vị đang được sử dụng." });
                    }
                    else
                    {
                        return Ok(new { message = "Xóa thành công!" });
                    }
                }
                // Nếu không được thì SoftDelete
                else
                {
                    return Ok(new { message = "Xóa thành công!" });
                }

            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi xóa đơn vị: {ex.Message}" });
            }
        }

        [HttpDelete("delete-NH/{id}")]
        public async Task<IActionResult> DeleteNhanHieu(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { message = "ID không hợp lệ." });
                }

                await _quanLySevices.BeginTransactionAsync();

                var nhanHieu = _quanLySevices.GetById<NhanHieu>(id);
                if (nhanHieu == null)
                {
                    await _quanLySevices.RollbackAsync();
                    return NotFound(new { message = "Không tìm thấy nhãn hiệu." });
                }
                _quanLySevices.HardDelete<NhanHieu>(nhanHieu);

                // Thử HardDelete trước
                if (!await _quanLySevices.CommitAsync("NhanHieu"))
                {
                    await _quanLySevices.BeginTransactionAsync();
                    _quanLySevices.SoftDelete<NhanHieu>(nhanHieu);
                    if (!await _quanLySevices.CommitAsync("NhanHieu"))
                    {
                        return BadRequest(new { message = "Không thể xóa nhãn hiệu. Có thể nhãn hiệu đang được sử dụng." });
                    }
                    else
                    {
                        return Ok(new { message = "Xóa thành công!" });
                    }
                }
                // Nếu không được thì SoftDelete
                else
                {
                    return Ok(new { message = "Xóa thành công!" });
                }

            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi xóa nhãn hiệu: {ex.Message}" });
            }
        }

        //=========================================GetDataById=======================================================================

        [HttpGet("get-DM-by-id")]
        public async Task<IActionResult> GetDanhMucById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id không hợp lệ");

            var danhMuc = _quanLySevices.GetById<DanhMuc>(id);

            if (danhMuc == null)
                return NotFound("Không tìm thấy danh mục");

            return Ok(new
            {
                id = danhMuc.Id,
                ten = danhMuc.Ten
            });
        }

        [HttpPost("getSanPhamDataById")]
        public async Task<IActionResult> GetSanPhamById([FromBody] string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Id không hợp lệ");

            var sanPham = _quanLySevices.GetById<SanPham>(id);

            if (sanPham == null)
                return NotFound("Không tìm thấy san phẩm");

            return Ok(new
            {
                sanPham.Id,
                sanPham.Ten,
                sanPham.NhanHieuId,
                danhMucs = sanPham.SanPhamDanhMucs.Select(spdm => spdm.DanhMucId).ToArray()
            });
        }

        [HttpPost("getSanPhamDonViDataById")]
        public async Task<IActionResult> GetSanPhamDonViById(string sanPhamId, string donViId)
        {
            if (string.IsNullOrEmpty(sanPhamId) || string.IsNullOrEmpty(donViId))
                return BadRequest("Id không hợp lệ");

            var sanPhamDonVi = _quanLySevices.GetById<SanPhamDonVi>(sanPhamId, donViId);

            if (sanPhamDonVi == null)
                return NotFound("Không tìm thấy san phẩm");

            var newSP = new
            {
                sanPhamDonVi.SanPhamId,
                sanPhamDonVi.DonViId,
                sanPhamDonVi.HeSoQuyDoi,
                sanPhamDonVi.GiaBan,
                sanPhamDonVi.TrangThai,
                anhs = sanPhamDonVi.AnhSanPhamDonVis
        .Select(a => _quanLySevices.ConvertToBase64Image(a.Anh.Anh, a.Anh.TenAnh))
        .ToArray()
            };
            return Ok(newSP);
        }

        //=========================================GetAllData=======================================================================

        [HttpGet("get-all-DM")]
        public async Task<IActionResult> GetAllDanhMuc()
        {
            var lstDanhMuc = _quanLySevices.GetList<DanhMuc>()
                .Where(dm => !dm.IsDelete)
                .Select(dm => new
                {
                    id = dm.Id,
                    ten = dm.Ten,
                    soSanPham = dm.SanPhamDanhMucs.Count
                }).ToList();

            return Ok(lstDanhMuc);
        }


        [HttpGet("get-all-DV")]
        public async Task<IActionResult> GetAllDonVi()
        {
            try
            {
                var lstDonVi = _quanLySevices.GetList<DonViDoLuong>()
                    .Where(dv => !dv.IsDelete)
                    .Select(dv => new
                    {
                        id = dv.Id,
                        ten = dv.Ten,
                        kyHieu = dv.KyHieu
                    })
                    .ToList();

                return Ok(lstDonVi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpGet("get-all-NH")]
        public async Task<IActionResult> GetAllNhanHieu()
        {
            try
            {
                var lstNhanHieu = _quanLySevices.GetList<NhanHieu>()
                    .Where(nh => !nh.IsDelete)
                    .Select(nh => new
                    {
                        id = nh.Id,
                        ten = nh.Ten,
                        soSanPham = nh.SanPhams.Count
                    })
                    .ToList();

                return Ok(lstNhanHieu);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpGet("get-all-SP")]
        public async Task<IActionResult> GetAllSanPham()
        {
            var lstSanPham = _quanLySevices.GetList<SanPham>().Select(sp => new
            {
                sp.Id,
                sp.Ten,
                NhanHieu = sp.NhanHieu.Ten,
                DanhMucs = sp.SanPhamDanhMucs.Select(spdm => spdm.DanhMuc.Ten).ToArray()
            }).ToList();


            return Ok(lstSanPham);
        }

        [HttpGet("getAllSPDV")]
        public async Task<IActionResult> GetAllSanPhamDonVi()
        {
            var lstSanPhamDonVi = _quanLySevices.GetList<SanPhamDonVi>().Select(spdv => new
            {
                spdv.SanPhamId,
                spdv.DonViId,
                spdv.SanPham.Ten,
                danhMucs = spdv.SanPham.SanPhamDanhMucs.Select(spdm => spdm.DanhMuc.Ten).ToArray(),
                nhanHieu = spdv.SanPham.NhanHieu.Ten,
                donVi = spdv.DonVi.Ten,
                spdv.GiaBan,
                spdv.TrangThai
            }).ToList();

            return Ok(lstSanPhamDonVi);
        }
    }
}














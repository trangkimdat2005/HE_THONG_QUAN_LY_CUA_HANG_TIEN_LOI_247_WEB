using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
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
            var lstSanPham = _quanLySevices.GetList<SanPham>();

            ViewData["lstSanPham"] = lstSanPham;

            var lstDonVi = _quanLySevices.GetList<DonViDoLuong>();

            ViewData["lstDonVi"] = lstDonVi;

            var lstSanPhamDonVi = _quanLySevices.GetList<SanPhamDonVi>();

            ViewData["lstSanPhamDonVi"] = lstSanPhamDonVi;

            return View();
        }
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

                if (!_quanLySevices.Add<SanPham>(sanPham))
                {
                    return BadRequest("không thể thêm sản phẩm");
                }

                foreach (string danhMucId in request.DanhMucs)
                {
                    SanPhamDanhMuc sanPhamDanhMuc = new SanPhamDanhMuc
                    {
                        SanPhamId = request.Id,
                        DanhMucId = danhMucId,
                        Id = _quanLySevices.GenerateNewId<SanPhamDanhMuc>("SPDM", 8)
                    };
                    if (!_quanLySevices.Add<SanPhamDanhMuc>(sanPhamDanhMuc))
                    {
                        return BadRequest("không thể thêm sản phẩm danh mục");
                    }
                }

                await _notifier.NotifyReloadAsync("HangHoa");

                return Ok(new { message = "Thêm sản phẩm thành công!" });
            }
            catch (Exception ex)
            {
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

                if (!_quanLySevices.Add(spdv))
                {
                    return BadRequest(new { message = "Không thể thêm sản phẩm - đơn vị!" });
                }

                if (request.ImagesUpload != null && request.ImagesUpload.Any())
                {
                    foreach (var file in request.ImagesUpload)
                    {

                        HinhAnh anh = new HinhAnh
                        {
                            Id = _quanLySevices.GenerateNewId<HinhAnh>("ANH", 7),
                            TenAnh = file.FileName,
                            Anh = await _quanLySevices.ConvertImageToByteArray(file)
                        };
                        if (_quanLySevices.Add<HinhAnh>(anh))
                        {
                            AnhSanPhamDonVi anhSanPhamDonVi = new AnhSanPhamDonVi
                            {
                                SanPhamDonViId = spdv.Id,
                                AnhId = anh.Id
                            };

                            if (!_quanLySevices.Add<AnhSanPhamDonVi>(anhSanPhamDonVi))
                            {
                                return BadRequest(new { message = "không thể thêm ảnh!" });
                            }
                        }
                    }
                }

                

                // Trigger realtime reload
                await _notifier.NotifyReloadAsync("SanPham");

                return Ok(new { message = "Thêm sản phẩm thành công!" });
            }
            catch (Exception ex)
            {
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
                    return BadRequest("Dữ liệu không hợp lệ.");
                }


                if (_quanLySevices.Add<NhanHieu>(nhanHieu))
                {
                    await _notifier.NotifyReloadAsync("NhanHieu");

                    return Ok(new { message = "Thêm nhãn hiệu thành công!" });
                }

                return BadRequest("thêm nhãn hiệu thất bại");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi thêm nhãn hiệu: {ex.Message}" });
            }

        }


        [HttpPost("add-DM")]
        public async Task<IActionResult> AddDanhMuc([FromBody] DanhMuc danhMuc)
        {
            try
            {
                if (danhMuc.Id == null || danhMuc.Ten == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }


                if (_quanLySevices.Add<DanhMuc>(danhMuc))
                {
                    await _notifier.NotifyReloadAsync("DanhMuc");

                    return Ok(new { message = "Thêm danh mục thành công!" });
                }

                return BadRequest("thêm danh mục thất bại");
            }
            catch (Exception ex)
            {
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
                    return BadRequest("Dữ liệu không hợp lệ.");
                }


                if (_quanLySevices.Add<DonViDoLuong>(donViDoLuong))
                {
                    await _notifier.NotifyReloadAsync("DonVi");

                    return Ok(new { message = "Thêm đơn vị thành công!" });
                }

                return BadRequest("thêm đơn vị thất bại");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi thêm đơn vị: {ex.Message}" });
            }

        }

        //=========================================EditData=======================================================================

        [HttpPost("edit-DM")]
        public async Task<IActionResult> EditDanhMuc([FromBody] DanhMuc danhMuc)
        {
            try
            {
                if (danhMuc.Id == null || danhMuc.Ten == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }


                if (_quanLySevices.Update<DanhMuc>(danhMuc))
                {
                    await _notifier.NotifyReloadAsync("DanhMuc");

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



                SanPham sanPham = _quanLySevices.GetById<SanPham>(request.Id);

                sanPham.Ten = request.Ten;
                sanPham.NhanHieuId = request.NhanHieu;
                if (!_quanLySevices.Update<SanPham>(sanPham))
                {
                    return BadRequest("không thể sửa sản phẩm");
                }
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
                    if (!_quanLySevices.Add<SanPhamDanhMuc>(sanPhamDanhMuc))
                    {
                        return BadRequest("không thể thêm sản phẩm danh mục");
                    }
                }

                var toRemove = oldDMs.Except(newDanhMucs);

                foreach (string danhMucs in toRemove)
                {
                    SanPhamDanhMuc sanPhamDanhMuc = _quanLySevices.GetById<SanPhamDanhMuc>(sanPham.Id, danhMucs);
                    if (sanPhamDanhMuc != null)
                    {
                        if (_quanLySevices.HardDelete<SanPhamDanhMuc>(sanPhamDanhMuc))
                        {

                        }
                        else if (!_quanLySevices.SoftDelete<SanPhamDanhMuc>(sanPhamDanhMuc))
                        {
                            return BadRequest("không thể xóa sản phẩm danh mục");
                        }
                    }
                }

                await _notifier.NotifyReloadAsync("HangHoa");

                return Ok(new { message = "Sửa sản phẩm thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi thêm sửa phẩm: {ex.Message}" });
            }

        }

        //=========================================DeleteData=======================================================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var danhMuc = _quanLySevices.GetById<DanhMuc>(id);
            if (danhMuc == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục." });
            }


            if (_quanLySevices.HardDelete<DanhMuc>(danhMuc))
            {

            }
            else if (_quanLySevices.SoftDelete<DanhMuc>(danhMuc))
            {
                //tên NotifyReloadAsync("DanhMuc") = key: 'DanhMuc'(bên js)
                await _notifier.NotifyReloadAsync("DanhMuc");

                return Ok(new { message = "Xóa thành công!" });

            }

            return BadRequest("lỗi khi xoá dữ liệu");

        }

        [HttpDelete("deleteSP{id}")]
        public async Task<IActionResult> DeleteSP(string id)
        {
            var sanPham = _quanLySevices.GetById<SanPham>(id);
            if (sanPham == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm." });
            }

            if (_quanLySevices.HardDelete<SanPham>(sanPham))
            {

            }
            else if (_quanLySevices.SoftDelete<SanPham>(sanPham))
            {
                //tên NotifyReloadAsync("DanhMuc") = key: 'DanhMuc'(bên js)
                await _notifier.NotifyReloadAsync("DanhMuc");

                return Ok(new { message = "Xóa thành công!" });

            }

            return BadRequest("lỗi khi xoá dữ liệu");

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
            var lstDanhMuc = _quanLySevices.GetList<DanhMuc>().Select(dm => new
            {
                id = dm.Id,
                ten = dm.Ten,
                soSanPham = dm.SanPhamDanhMucs.Count
            }).ToList();

            return Ok(lstDanhMuc);
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



using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [ApiController]
    [Route("API")]
    public class GetNextIdController : ControllerBase
    {
        private readonly IQuanLyServices _quanLySevices;

        public GetNextIdController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [HttpPost("get-next-id-SP")]
        public Task<IActionResult> GetNextId([FromBody] GetNextIdSPRequest request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLySevices.GenerateNewId<SanPham>(request.prefix, request.totalLength) }));
        }
    }
    public class GetNextIdSPRequest
    {
        public string prefix { get; set; }
        public int totalLength { get; set; }
    }
}

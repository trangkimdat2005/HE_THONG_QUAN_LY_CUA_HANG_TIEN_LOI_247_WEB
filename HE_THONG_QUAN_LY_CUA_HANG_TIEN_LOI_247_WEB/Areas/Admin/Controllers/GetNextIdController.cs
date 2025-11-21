using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Authorize]
    [ApiController]
    [Route("API")]
    public class GetNextIdController : ControllerBase
    {
        private readonly IQuanLyServices _quanLySevices;

        public GetNextIdController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        
        
    }
    
}

using Application.IService;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
    [EnableCors("Allow")]
    [Route("api/koi")]
    [ApiController]
    [Authorize(Roles = "Staff,Admin,Customer")]
    public class KOIController : BaseController
    {
        private readonly IKoiService _koiService;
        public KOIController(IKoiService koiService)
        {
            _koiService = koiService;
        }       
        [HttpPost]    
        public async Task<IActionResult> CreateProductAsync(cCreateKOIDTO product)
        {
            var result = await _koiService.cCreateKOIAsync(product);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 5,
            [FromQuery] string? search = "", [FromQuery] string? sort = "")
        {
            var result = await _koiService.GetAllKoisAsync(page, pageSize, search, sort);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }
    }
}

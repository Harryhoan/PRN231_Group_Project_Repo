using Application.IService;
using Application.ViewModels.KoiDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
    [EnableCors("Allow")]
    [Route("api/koi")]
    [ApiController]
    //[Authorize(Roles = "Staff,Admin,Customer")]
    public class KOIController : BaseController
    {
        private readonly IKoiService _koiService;
        public KOIController(IKoiService koiService)
        {
            _koiService = koiService;
        }

        /// <summary>
        /// Create Product By Admin
        /// </summary>
        /// <param name="registerObject">The registration details for the new user.</param>
        /// <returns>A response indicating success or failure of the registration.</returns>

        [Authorize(Roles = "Admin")]
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
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredKOIsAsync(dFilterKoiDTO filter)
        {
            var result = await _koiService.dGetFilteredKOIsAsync(filter);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }

    
        [AllowAnonymous]
        [HttpGet("get")]
        public async Task<IActionResult> GetAllKoisAsync(int pageNumber, int pageSize)
        {
            var result = await _koiService.dGetAllKois(pageNumber, pageSize);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> cGetAllProductsAdmin([FromQuery] int page = 1, [FromQuery] int pageSize = 5,
          [FromQuery] string? search = "", [FromQuery] string? sort = "")
        {
            var result = await _koiService.cGetAllKoisAsync(page, pageSize, search, sort);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetKoiById(int id)
        {
            var result = await _koiService.dGetKOIById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}

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
    [Authorize(Roles = "Staff,Admin,Customer")]
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
        //[Authorize(Roles = "Admin")]
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
        /// <summary>
        /// Update Product By Admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> cUpdateProductAsync(int id, cUpdateProductDTO product)
        {
            var result = await _koiService.cUpdateProductAsync(product);
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Get koi by filter 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredKOIsAsync([FromQuery] dFilterKoiDTO filter)
        {
            var result = await _koiService.dGetFilteredKOIsAsync(filter);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }

        /// <summary>
        /// Get koi list 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all koi by admin
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, staff")]
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

        /// <summary>
        /// Get koi by koiId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        [Authorize(Roles = "Admin, staff")]
        [HttpGet("{id}")]
        public async Task<IActionResult> cGetKoiByIdAdminNotImage(int id)
        {
            var result = await _koiService.cGetKoibyAdmin(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Delete product by admin
        /// </summary>    
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> cDeleteProductAsync(int id)
        {
            var result = await _koiService.cDeleteProductAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// get image for product by admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{koiId}/images")]
        public async Task<IActionResult> GetImagesByKoiId(int koiId)
        {
            var images = await _koiService.GetImagesByKoiIdForAdmin(koiId);
            if (images == null || images.Count == 0)
            {
                return NotFound(new { Message = "Không tìm thấy ảnh cho Koi này." });
            }
            return Ok(new { Data = images });
        }
        /// <summary>
        /// count koi for admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("count")]
        public IActionResult CountKois()
        {
            int koiCount = _koiService.CountKois();
            return Ok(new { KoiCount = koiCount });
        }

    }
}

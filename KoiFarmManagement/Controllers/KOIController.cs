using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.KoiDTO;
using Domain.Entities;
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
        [AllowAnonymous]
        //[Authorize(Roles = "Staff,Admin")]
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

        [HttpGet]
        public async Task<IActionResult> GetFilteredKOIsAsync(dFilterKoiDTO filter)
        {
            var result = await _koiService.dGetFilteredKOIsAsync(filter);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllKoisAsync(int pageNumber, int pageSize)
        {
            var result = await _koiService.dGetAllKois(pageNumber, pageSize);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
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

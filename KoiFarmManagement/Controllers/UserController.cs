using Application.IService;
using Application.Services;
using Application.ViewModels.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
    [EnableCors("Allow")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly cIUserService _userService;

        public UserController(cIUserService cIUserService)
        {
            _userService = cIUserService;
        }
        [Authorize(Roles = "Customer")]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userService.GetUserByResponse(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }
            if (!user.Success)
            {
                return BadRequest(user);
            }
            return Ok(user);
        }
        [Authorize(Roles = "Customer")]
        [HttpGet("Address")]
        public async Task<IActionResult> GetAddress()
        {
            var result = await _userService.GetAddressByUser(HttpContext.User);
            if (result == null)
            {
                return Unauthorized();
            }
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "Customer")]
        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDTO profileDTO)
        {
            var result = await _userService.UpdateProfile(HttpContext.User,profileDTO);
            if (result == null)
            {
                return Unauthorized();
            }
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = "Customer")]
        [HttpPut("Address")]
        public async Task<IActionResult> UpdateAddress( [FromBody] AddressDTO addressDTO)
        {
            var result = await _userService.UpdateAddress(HttpContext.User, addressDTO);
            if (result == null)
            {
                return Unauthorized();
            }
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = "Customer")]
        [HttpDelete("Address/{id}")]
        public async Task<IActionResult> DeleteAddress([FromRoute] int id)
        {
            var result = await _userService.DeleteAdress(HttpContext.User,id);
            if (result == null)
            {
                return Unauthorized();
            }
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("Address/")]
        public async Task<IActionResult> AddAddress([FromBody] AddressDTO addressDTO)
        {
            var result = await _userService.CreateAddress(HttpContext.User, addressDTO);
            if (result == null)
            {
                return Unauthorized();
            }
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


    }
}

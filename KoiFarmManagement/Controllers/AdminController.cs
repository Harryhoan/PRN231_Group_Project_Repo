using Application.IService;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
    [EnableCors("Allow")]
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly cIUserService _userService;
        public AdminController(cIUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users by admin
        /// </summary>
        /// <param name="registerObject">The registration details for the new user.</param>
        /// <returns>A response indicating success or failure of the registration.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersAdmin([FromQuery] int page = 1, [FromQuery] int pageSize = 5, [FromQuery] string? search = "", [FromQuery] string? sort = "")
        {
            var result = await _userService.GetAllUsersByAdmin(page, pageSize, search, sort);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// count users for admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("count-user")]
        public IActionResult CountKois()
        {
            int userCount = _userService.GetCount();
            return Ok(new { UserCount = userCount });
        }
    }
}

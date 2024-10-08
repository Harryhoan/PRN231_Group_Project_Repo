using Application.IService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
    [EnableCors("Allow")]
    [Route("api/user")]
    [ApiController]
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
        public async Task<IActionResult> GetAllUsersAdmin([FromQuery] int page = 1, [FromQuery] int pageSize = 5, [FromQuery] string? search = "", [FromQuery] string? sort = "")
        {
            var result = await _userService.GetAllUsersByAdmin(page, pageSize, search, sort);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}

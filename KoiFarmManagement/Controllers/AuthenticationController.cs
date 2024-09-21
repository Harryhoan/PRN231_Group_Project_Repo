using Application.IService;
using Application.ViewModels.UserDTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
    [EnableCors("Allow")]
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenService _authenService;

        public AuthenticationController(IAuthenService authent)
        {
            _authenService = authent;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(cRegisterDTO registerObject)
        {
            var result = await _authenService.cRegisterAsync(registerObject);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(cLoginUserDTO loginObject)
        {
            var result = await _authenService.cLoginAsync(loginObject);

            if (!result.Success)
            {
                return StatusCode(401, result);
            }
            else
            {
                return Ok(
                    new
                    {
                        success = result.Success,
                        message = result.Message,
                        token = result.DataToken,
                        role = result.Role,
                        hint = result.HintId,
                    }
                );
            }
        }
        [HttpPost("resend")]
        public async Task<IActionResult> ReSendConfirm(string sEmail)
        {
            var result = await _authenService.cResendConfirmationTokenAsync(sEmail);

            if (!result.Success)
            {
                return StatusCode(401, result);
            }
            else
            {
                return Ok(result);
            }
        }

    }
}

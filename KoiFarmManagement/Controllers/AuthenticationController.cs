using Application.IService;
using Application.ViewModels.UserDTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
    /// <summary>
    /// Handles authentication-related actions, such as registration, login, and resending confirmation tokens.
    /// </summary>
    [EnableCors("Allow")]
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenService _authenService;

        /// <summary>
        /// Constructor for the AuthenticationController.
        /// </summary>
        /// <param name="authent">The authentication service used to handle authentication requests.</param>
        public AuthenticationController(IAuthenService authent)
        {
            _authenService = authent;
        }

        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="registerObject">The registration details for the new user.</param>
        /// <returns>A response indicating success or failure of the registration.</returns>
        [HttpPost("register")]
        [AllowAnonymous]
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

        /// <summary>
        /// Logs in an existing user with the provided login details.
        /// </summary>
        /// <param name="loginObject">The login details including username and password.</param>
        /// <returns>A response indicating success or failure of the login, along with a token and user role if successful.</returns>
        [HttpPost("login")]
        [AllowAnonymous]

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

        /// <summary>
        /// Resends the confirmation token to the specified email address.
        /// </summary>
        /// <param name="sEmail">The email address to which the confirmation token will be resent.</param>
        /// <returns>A response indicating success or failure of the resend action.</returns>
        [HttpPost("resend")]
        [Authorize(Roles = "Customer")]
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

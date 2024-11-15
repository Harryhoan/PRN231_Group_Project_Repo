using Application.IService;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderDetailService _orderDetailService;
        public PaymentController(IPaymentService paymentService, IOrderDetailService orderService)
        {
            _paymentService = paymentService;
            _orderDetailService = orderService;
        }

        /// <summary>
        /// Create a new payment
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Customer")]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment()
        {
            var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _paymentService.CreatePaymentAsync(user.Id, "http://localhost:3000/payment", "http://localhost:3000/user/cart");

			if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Exercute if the payment is success or not 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="token"></param>
        /// <param name="PayerID"></param>
        /// <returns></returns>
        [HttpGet("execute")]
        [AllowAnonymous]
        public async Task<IActionResult> ExecutePayment([FromQuery] string paymentId, [FromQuery] string token, [FromQuery] string PayerID)
        {
            var result = await _paymentService.ExecutePaymentAsync(paymentId, PayerID);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


    }
}

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
        [Authorize(Roles = "Customer")]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment( string returnUrl, string cancelUrl)
        {
            var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _paymentService.CreatePaymentAsync(user.Id, returnUrl, cancelUrl);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost("execute")]
        public async Task<IActionResult> ExecutePayment(string paymentId, string payerId)
        {
            var result = await _paymentService.ExecutePaymentAsync(paymentId, payerId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }


    }
}

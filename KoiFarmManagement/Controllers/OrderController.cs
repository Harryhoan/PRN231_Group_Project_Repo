using Application.IService;
using Application.Services;
using Application.ViewModels.OrderDetailDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
    [EnableCors("Allow")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
	{
		private readonly IOrderDetailService _orderDetailService;
		private readonly IOrderService _orderService;
		public OrderController(IOrderDetailService orderDetailService, IOrderService orderService)
		{
			_orderDetailService = orderDetailService;
			_orderService = orderService;
		}

		[Authorize(Roles = "Customer")]
		[HttpGet("/cart")]
		public async Task<IActionResult> GetCart()
		{
			var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
			if (user == null)
			{
				return Unauthorized();
			}
			var result = await _orderService.GetCart(user);
			if (!result.Success)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

        [Authorize(Roles = "Customer")]
        [HttpGet("/history")]
        public async Task<IActionResult> GetOrderHistory()
        {
            var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _orderService.GetOrdersByUser(user);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [Authorize(Roles = "Customer")]
		[HttpPost]
		public async Task<IActionResult> AddToCart(aCreateOrderDetailDTO cart)
		{
			var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
			if (user == null)
			{
				return Unauthorized();
			}
			var result = await _orderDetailService.aCreateOrderDetailAsync(cart, user);
			if (!result.Success)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete]
		public async Task<IActionResult> DeleteCart(int id)
		{
			var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
			if (user == null)
			{
				return Unauthorized();
			}
			var result = await _orderDetailService.aDeleteOrderDetailAsync(id, user);
			if (!result.Success)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
	}
}

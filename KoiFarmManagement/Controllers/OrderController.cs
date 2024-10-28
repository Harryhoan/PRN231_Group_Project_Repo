using Application.IService;
using Application.Services;
using Application.ViewModels.OrderDetailDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
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
		[HttpGet]
		public async Task<IActionResult> GetCart(aCreateOrderDetailDTO cart)
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
        /// <summary>
        /// Retrieves all orders for admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> cGetAllOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 5,
            [FromQuery] string search = "", [FromQuery] string status = "", [FromQuery] string sort = "id")
        {
            var result = await _orderService.cGetAllOrder(page, pageSize, search, status, sort);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }


    }
}

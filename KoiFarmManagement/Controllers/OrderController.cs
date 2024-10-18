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
		public OrderController(IOrderDetailService orderDetailService)
		{
			_orderDetailService = orderDetailService;
		}

		[Authorize(Roles = "Admin")]
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
	}
}

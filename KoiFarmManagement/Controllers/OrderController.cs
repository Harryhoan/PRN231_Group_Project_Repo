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

		/// <summary>
		/// List out item in cart 
		/// </summary>
		/// <returns></returns>
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

        /// <summary>
        /// List out order history
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Get all orders for admin
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
		[HttpGet("/admin/all")]
		public async Task<IActionResult> GetOrders()
		{
			var result = await _orderService.GetAllOrders();
			if (!result.Success)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

        /// <summary>
        /// Adding new item to cart
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        [Authorize(Roles = "Customer")]
		[HttpPost("addtocart")]
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
		/// <summary>
		/// Update address in order
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        [Authorize(Roles = "Customer")]
        [HttpPut("Address/{id}")]
        public async Task<IActionResult> UpdateOrderAddress([FromRoute] int id)
        {
            var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _orderService.UpdateAddress(id, user);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Delete cart
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Customer")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCart([FromRoute] int id)
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
            [FromQuery] string? search = "", [FromQuery] string? status = "", [FromQuery] string? sort = "")
        {
            var result = await _orderService.cGetAllOrder(page, pageSize, search, status, sort);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }
        /// <summary>
        /// count orders for admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("count")]
        public IActionResult CountOrders()
        {
            int orderCount = _orderService.CountOrders();
            return Ok(new { OrderCount = orderCount });
        }

    }
}

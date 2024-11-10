using Application.IService;
using Application.ViewModels.ReviewDTO;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
    [EnableCors("Allow")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : BaseController
    {
        private readonly IReviewService _reviewService;
        private readonly IOrderDetailService _orderDetailService;

        public ReviewController(IReviewService reviewService, IOrderDetailService orderDetailService)
        {
            _reviewService = reviewService;
            _orderDetailService = orderDetailService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet] 
        public async Task<IActionResult> GetAllReviewAsync()
        {
            var result = await _reviewService.GetAllReviewAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> AddReviewAsync(int orderId, ReviewRequestDTO reviewRequest)
        {
            var result = await _reviewService.ReviewAsync(orderId, reviewRequest);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpGet("{reviewId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReviewAsync([FromRoute] int reviewId)
        {
            var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _reviewService.GetReviewAsync(reviewId, user);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpGet("/koi/{reviewId}")]
        public async Task<IActionResult> GetListReviewByKoiAsync([FromRoute] int reviewId)
        {
            var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _reviewService.GetListReviewByKoiAsync(reviewId, user);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpGet("/orderdetail/{orderDetailId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReviewByOrderDetailAsync([FromRoute] int orderDetailId)
        {
            var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _reviewService.GetReviewByOrderDetailAsync(orderDetailId, user);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("/user/{userId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetReviewByUserAsync([FromRoute] int userId)
        {
            var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
            if (user == null || (user.Role == "Customer" && user.Id == userId))
            {
                return Unauthorized();
            }
            var result = await _reviewService.GetReviewsByCustomerId(user.Id);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }



        [Authorize(Roles = "Customer")]
        [HttpDelete]
        public async Task<IActionResult> DeleteReviewAsync(int reviewId)
        {
            var result = await _reviewService.DeleteReviewAsync(reviewId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditReview([FromBody] aEditReviewDTO review)
        {
            var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _reviewService.aEditReviewAsync(review, user);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}

using Application.IService;
using Application.ViewModels.ReviewDTO;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
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

        [Authorize]
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

        [Authorize]
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
        public async Task<IActionResult> GetReviewAsync([FromRoute] int reviewId)
        {
            var result = await _reviewService.GetReviewAsync(reviewId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [Authorize]
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

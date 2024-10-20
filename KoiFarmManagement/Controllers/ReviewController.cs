using Application.IService;
using Domain.Entities;
using Domain.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : BaseController
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

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
        [HttpPost]
        public async Task<IActionResult> AddReviewAsync(int orderId, ReviewRequest reviewRequest)
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
    }
}

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

        /// <summary>
        /// Get all reviews by admin
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Adding new review by customer
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="reviewRequest"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get review by reviewId
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [HttpGet("{reviewId}")]
        [Authorize]
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
        /// <summary>
        /// Get review by productId
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [HttpGet("koi/{koiId}")]
        public async Task<IActionResult> GetReviewByKoiAsync([FromRoute] int koiId)
        {
            //var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
            //if (user == null)
            //{
            //    return Unauthorized();
            //}
            var result = await _reviewService.GetReviewByKoiAsync(koiId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Get review by orderDetailId
        /// </summary>
        /// <param name="orderDetailId"></param>
        /// <returns></returns>
        [HttpGet("orderdetail/{orderDetailId}")]
        [Authorize(Roles = "Customer, Admin, Staff")]
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

        /// <summary>
        /// Get review by customerId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetReviewByUserAsync()
        {
            var user = await _orderDetailService.aGetUserByTokenAsync(HttpContext.User);
            if (user == null)
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


        /// <summary>
        /// Customer Delete review 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Customer edit their review 
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
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

using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.ReviewDTO;
using AutoMapper;
using Domain.Entities;
using Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepo _reviewRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepo reviewRepo, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _reviewRepo = reviewRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<ReviewDTO>> DeleteReviewAsync(int reviewId)
        {
            var response = new ServiceResponse<ReviewDTO>();

            try
            {
                var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
                if (review == null)
                {
                    response.Success = false;
                    response.Message = "Review not found";
                    return response;
                }
                await _unitOfWork.ReviewRepository.Remove(review);
                response.Success = true;
                response.Message = "Review deleted successfully.";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to delete review: {ex.Message}";
                return response;
            }
        }

        public async Task<ServiceResponse<List<ReviewDTO>>> GetAllReviewAsync()
        {
            var response = new ServiceResponse<List<ReviewDTO>>();

            try
            {
                var review = await _unitOfWork.ReviewRepository.GetAllAsync();
                if (review == null)
                {
                    response.Success = false;
                    response.Message = "No review exist.";
                    return response;
                }
                response.Data = _mapper.Map<List<ReviewDTO>>(review);
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to get review: {ex.Message}";
                return response;
            }
        }

        public async Task<ServiceResponse<ReviewDTO>> GetReviewAsync(int reviewId)
        {
            var response = new ServiceResponse<ReviewDTO>();

            try
            {
                var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
                if (review == null)
                {
                    response.Success = false;
                    response.Message = "Review not found";
                    return response;
                }
                response.Data = _mapper.Map<ReviewDTO>(review);
                response.Success = true;
                response.Message = "Review found successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to get review: {ex.Message}";
                return response;
            }
        }
        public async Task<ServiceResponse<ReviewDTO>> ReviewAsync(int orderId, ReviewRequest reviewRequest)
        {
            var response = new ServiceResponse<ReviewDTO>();

            try
            {
                var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(orderId);
                if (orderDetail == null)
                {
                    response.Success = false;
                    response.Message = "Order not found";
                    return response;
                }
                var existingReviewing = _mapper.Map<Review>(reviewRequest);
                if (existingReviewing.OrderDetailId != default)
                {
                    response.Success = false;
                    response.Message = "Order has already had a review.";
                    return response;
                }

                existingReviewing.OrderDetailId = orderId;
                existingReviewing.Rating = reviewRequest.Rating;
                existingReviewing.Comment = reviewRequest.Comment;

                await _unitOfWork.ReviewRepository.AddAsync(existingReviewing);
                response.Data = _mapper.Map<ReviewDTO>(existingReviewing);
                response.Success = true;
                response.Message = "Review added successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to add review: {ex.Message}";
            }
            return response;
        }
    }
}

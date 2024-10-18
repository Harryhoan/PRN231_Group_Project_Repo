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

        public async Task<ServiceResponse<ReviewDTO>> ReviewAsync(int orderId, ReviewRequest reviewRequest)
        {
            var response = new ServiceResponse<ReviewDTO>();

            try
            {
                var review = await _unitOfWork.ReviewRepository.checkIdExist(orderId);
                if (review == false)
                {
                    response.Success = false;
                    response.Message = "Order not found";
                    return response;
                }
                var existingReviewing = _mapper.Map<Review>(reviewRequest);
                if (existingReviewing.OrderDetailId == default)
                {
                    response.Success = false;
                    response.Message = "Order has already had a review.";
                    return response;
                }

                var newReview = _mapper.Map<Review>(reviewRequest);
                newReview.OrderDetailId = orderId;
                newReview.Rating = reviewRequest.Rating;
                newReview.Comment = reviewRequest.Comment;

                await _unitOfWork.ReviewRepository.AddAsync(newReview);
                response.Data = _mapper.Map<ReviewDTO>(newReview);
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

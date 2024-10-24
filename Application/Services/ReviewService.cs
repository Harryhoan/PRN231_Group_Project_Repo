﻿using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.ReviewDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IMapper mapper, IUnitOfWork unitOfWork)
        {
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

        public async Task<ServiceResponse<ReviewDTO>> aEditReviewAsync(aEditReviewDTO review, User user)
        {
            var response = new ServiceResponse<ReviewDTO>();

            try
            {
                if (user == null || !(user.Id > 0) || review == null)
                {
                    throw new ArgumentException();
                }
                var existingReview = await _unitOfWork.ReviewRepository.GetByIdAsync(review.Id);
                if (existingReview == null)
                {
                    response.Success = false;
                    response.Message = "Review not found";
                    return response;
                }
                var existingOrderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(existingReview.Id);
                if (existingReview == null)
                {
                    response.Success = false;
                    response.Message = "Order detail not found";
                    return response;
                }
                if (user.Role == "Customer" && !(await _unitOfWork.OrderDetailRepository.CheckOrderDetailBelongingToUser(existingReview.Id, user.Id)))
                {
                    response.Success = false;
                    response.Message = "Unauthorized modification";
                    return response;
                }
                existingReview.Comment = review.Comment;
                existingReview.Rating = review.Rating;
                await _unitOfWork.ReviewRepository.Update(existingReview);
                response.Data = _mapper.Map<ReviewDTO>(existingReview);
                response.Success = true;
                response.Message = "Review found successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to update review: {ex.Message}";
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

        public async Task<ServiceResponse<List<ReviewDTO>>> GetReviewsByCustomerId(int userId)
        {
            var response = new ServiceResponse<List<ReviewDTO>>();

            try
            {
                var reviews = await _unitOfWork.ReviewRepository.GetReviewsByUserIdAsync(userId);
                if (reviews == null)
                {
                    response.Success = false;
                    response.Message = "No review exist.";
                    return response;
                }
                response.Data = _mapper.Map<List<ReviewDTO>>(reviews);
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

        public async Task<ServiceResponse<ReviewDTO>> GetReviewAsync(int reviewId, User user)
        {
            var response = new ServiceResponse<ReviewDTO>();

            try
            {
                if (user == null || !(user.Id > 0) || !(reviewId > 0))
                {
                    throw new ArgumentException();
                }
                var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
                if (review == null)
                {
                    response.Success = false;
                    response.Message = "Review not found";
                    return response;
                }
                if (user.Role == "Customer")
                {
                    var existingOrderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(review.OrderDetailId);
                    if (existingOrderDetail == null || !(await _unitOfWork.OrderDetailRepository.CheckOrderDetailBelongingToUser(existingOrderDetail.Id, user.Id)))
                    {
                        response.Success = false;
                        response.Message = "Unauthorized access";
                        return response;
                    }
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

        public async Task<ServiceResponse<ReviewDTO>> GetReviewByOrderDetailAsync(int orderDetailId, User user)
        {
            var response = new ServiceResponse<ReviewDTO>();

            try
            {
                if (user == null || !(user.Id > 0) || !(orderDetailId > 0))
                {
                    throw new ArgumentException();
                }
                var review = await _unitOfWork.ReviewRepository.GetReviewByOrderDetailAsync(orderDetailId);
                if (review == null)
                {
                    response.Success = false;
                    response.Message = "Review not found";
                    return response;
                }
                if (user.Role == "Customer")
                {
                    var existingOrderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(review.OrderDetailId);
                    if (existingOrderDetail == null || !(await _unitOfWork.OrderDetailRepository.CheckOrderDetailBelongingToUser(existingOrderDetail.Id, user.Id)))
                    {
                        response.Success = false;
                        response.Message = "Unauthorized access";
                        return response;
                    }
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

        public async Task<ServiceResponse<ReviewDTO>> ReviewAsync(int orderId, ReviewRequestDTO reviewRequest)
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
                var existingReview = await _unitOfWork.ReviewRepository.GetReviewByOrderDetailAsync(orderDetail.Id);
                if (existingReview != null)
                {
                    response.Success = false;
                    response.Message = "An order detail can only have one review at a time";
                    return response;
                }
                var review = _mapper.Map<Review>(reviewRequest);
                if (review.OrderDetailId != default)
                {
                    response.Success = false;
                    response.Message = "Order has already had a review.";
                    return response;
                }

                review.OrderDetailId = orderId;
                review.Rating = reviewRequest.Rating;
                review.Comment = reviewRequest.Comment;

                await _unitOfWork.ReviewRepository.AddAsync(review);
                response.Data = _mapper.Map<ReviewDTO>(review);
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

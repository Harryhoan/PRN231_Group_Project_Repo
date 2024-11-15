using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.ImageDTO;
using Application.ViewModels.ReviewDTO;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.ViewEngines;
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
                await _unitOfWork.ReviewRepository.cDeleteTokenAsync(review);
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
                var existingOrderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(existingReview.OrderDetailId);
                if (existingOrderDetail == null)
                {
                    response.Success = false;
                    response.Message = "Order detail not found";
                    return response;
                }
                if (user.Role == "Customer" && !(await _unitOfWork.OrderDetailRepository.CheckOrderDetailBelongingToUser(existingOrderDetail.Id, user.Id)))
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
                response.Message = "Review Updated Successfully";
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
                var reviews = await _unitOfWork.ReviewRepository.GetAllAsync();
                if (reviews == null || reviews.Count == 0)
                {
                    response.Success = false;
                    response.Message = "No reviews exist.";
                    return response;
                }

                var reviewDTOs = new List<ReviewDTO>();
                foreach (var review in reviews)
                {
                    var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(review.OrderDetailId);
                    if (orderDetail != null)
                    {
                        var koi = await _unitOfWork.KoiRepo.GetByIdAsync(orderDetail.KoiId);
                        var koiImages = await _unitOfWork.ImageRepository.aGetImagesByKoiIdAsync(orderDetail.KoiId);

                        var reviewDTO = _mapper.Map<ReviewDTO>(review);
                        reviewDTO.KoiName = koi.Name;
                        reviewDTO.KoiImage = koiImages.FirstOrDefault()?.ImageUrl;

                        reviewDTOs.Add(reviewDTO);
                    }
                }

                response.Data = reviewDTOs;
                response.Success = true;
                response.Message = "Reviews retrieved successfully.";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to get reviews: {ex.Message}";
                return response;
            }
        }


        public async Task<ServiceResponse<List<ReviewDTO>>> GetReviewsByCustomerId(int userId)
        {
            var response = new ServiceResponse<List<ReviewDTO>>();

            try
            {
                var reviews = await _unitOfWork.ReviewRepository.GetReviewsByUserIdAsync(userId);
                if (reviews == null || reviews.Count == 0)
                {
                    response.Success = false;
                    response.Message = "No reviews exist.";
                    return response;
                }

                var reviewDTOs = new List<ReviewDTO>();
                foreach (var review in reviews)
                {
                    var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(review.OrderDetailId);
                    if (orderDetail != null)
                    {
                        var koi = await _unitOfWork.KoiRepo.GetByIdAsync(orderDetail.KoiId);
                        var koiImages = await _unitOfWork.ImageRepository.aGetImagesByKoiIdAsync(orderDetail.KoiId);

                        var reviewDTO = _mapper.Map<ReviewDTO>(review);
                        reviewDTO.KoiName = koi.Name;
                        reviewDTO.KoiImage = koiImages.FirstOrDefault()?.ImageUrl;

                        reviewDTOs.Add(reviewDTO);
                    }
                }

                response.Data = reviewDTOs;
                response.Success = true;
                response.Message = "Reviews retrieved successfully.";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to get reviews: {ex.Message}";
                return response;
            }
        }


        public async Task<ServiceResponse<ReviewDTO>> GetReviewAsync(int reviewId, User user)
        {
            var response = new ServiceResponse<ReviewDTO>();

            try
            {
                if (user == null || user.Id <= 0 || reviewId <= 0)
                {
                    throw new ArgumentException("Invalid user or review ID.");
                }

                var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
                if (review == null)
                {
                    response.Success = false;
                    response.Message = "Review not found";
                    return response;
                }

                var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(review.OrderDetailId);
                if (orderDetail == null)
                {
                    response.Success = false;
                    response.Message = "Order detail not found";
                    return response;
                }

                var koi = await _unitOfWork.KoiRepo.GetByIdAsync(orderDetail.KoiId);
                if (koi == null)
                {
                    response.Success = false;
                    response.Error = "Error: Koi Not Found.";
                    return response;
                }

                var koiImages = await _unitOfWork.ImageRepository.aGetImagesByKoiIdAsync(koi.Id);

                var reviewResponse = _mapper.Map<ReviewDTO>(review);
                response.Data = new ReviewDTO
                {
                    KoiName = koi.Name,
                    KoiImage = koiImages.Count > 0 ? koiImages.First().ImageUrl : "No Image Available",
                    Rating = reviewResponse.Rating,
                    Comment = reviewResponse.Comment,
                    Id = reviewResponse.Id,
                    OrderDetailId = reviewResponse.OrderDetailId
                };
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
        public async Task<ServiceResponse<ReviewDTO>> GetReviewByKoiAsync(int koiId)
        {
            var response = new ServiceResponse<ReviewDTO>();

            try
            {
                var review = await _unitOfWork.ReviewRepository.GetReviewByProductIdAsync(koiId);
                if (review == null)
                {
                    response.Success = false;
                    response.Message = "No reviews found for the specified Koi.";
                    return response;
                }
                var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(review.OrderDetailId);
                if (orderDetail == null)
                {
                    response.Success = false;
                    response.Message = "Order detail not found";
                    return response;
                }
                if (orderDetail.Id == review.Id && orderDetail.IsReviewed == false)
                {
                    orderDetail.IsReviewed = true;
                    await _unitOfWork.OrderDetailRepository.Update(orderDetail);
                }

                var koi = await _unitOfWork.KoiRepo.GetByIdAsync(orderDetail.KoiId);
                if (koi == null)
                {
                    response.Success = false;
                    response.Error = "Error: Koi Not Found.";
                    return response;
                }

                var koiImages = await _unitOfWork.ImageRepository.aGetImagesByKoiIdAsync(koi.Id);

                var reviewResponse = _mapper.Map<ReviewDTO>(review);
                response.Data = new ReviewDTO
                {
                    KoiName = koi.Name,
                    KoiImage = koiImages.Count > 0 ? koiImages.First().ImageUrl : "No Image Available",
                    Rating = reviewResponse.Rating,
                    Comment = reviewResponse.Comment,
                    Id = reviewResponse.Id,
                    OrderDetailId = reviewResponse.OrderDetailId
                };
                response.Success = true;
                response.Message = "Reviews found successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to get reviews: {ex.Message}";
            }
            return response;
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
                var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(review.OrderDetailId);
                if (orderDetail == null)
                {
                    response.Success = false;
                    response.Message = "Order detail not found";
                    return response;
                }

                var koi = await _unitOfWork.KoiRepo.GetByIdAsync(orderDetail.KoiId);
                if (koi == null)
                {
                    response.Success = false;
                    response.Error = "Error: Koi Not Found.";
                    return response;
                }

                var koiImages = await _unitOfWork.ImageRepository.aGetImagesByKoiIdAsync(koi.Id);

                var reviewResponse = _mapper.Map<ReviewDTO>(review);
                response.Data = new ReviewDTO
                {
                    KoiName = koi.Name,
                    KoiImage = koiImages.Count > 0 ? koiImages.First().ImageUrl : "No Image Available",
                    Rating = reviewResponse.Rating,
                    Comment = reviewResponse.Comment,
                    Id = reviewResponse.Id,
                    OrderDetailId = reviewResponse.OrderDetailId
                };
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

                var koi = await _unitOfWork.KoiRepo.GetByIdAsync(orderDetail.KoiId);
                if (koi == null)
                {
                    response.Success = false;
                    response.Error = "Error: Koi Not Found.";
                    return response;
                }

                review.OrderDetailId = orderId;
                review.Rating = reviewRequest.Rating;
                review.Comment = reviewRequest.Comment;

                await _unitOfWork.ReviewRepository.AddAsync(review);

                orderDetail.IsReviewed = true;
                await _unitOfWork.OrderDetailRepository.Update(orderDetail);
                //await _unitOfWork.SaveChangeAsync();

                var koiImages = await _unitOfWork.ImageRepository.aGetImagesByKoiIdAsync(koi.Id);
                var reviewResponse = _mapper.Map<ReviewDTO>(review);
                response.Data = new ReviewDTO
                {
                    KoiName = koi.Name,
                    KoiImage = koiImages.Count > 0 ? koiImages.First().ImageUrl : "No Image Available",
                    Rating = reviewResponse.Rating,
                    Comment = reviewResponse.Comment,
                    Id = reviewResponse.Id,
                    OrderDetailId = reviewResponse.OrderDetailId
                };
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

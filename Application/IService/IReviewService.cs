using Application.ServiceResponse;
using Application.ViewModels.ReviewDTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IReviewService
    {
        Task<ServiceResponse<ReviewDTO>> ReviewAsync(int orderId, ReviewRequestDTO reviewRequest);
        Task<ServiceResponse<ReviewDTO>> GetReviewAsync(int reviewId, User user);
        Task<ServiceResponse<ReviewDTO>> DeleteReviewAsync(int reviewId);
        Task<ServiceResponse<List<ReviewDTO>>> GetAllReviewAsync();
        Task<ServiceResponse<ReviewDTO>> aEditReviewAsync(aEditReviewDTO review, User user);
        Task<ServiceResponse<ReviewDTO>> GetReviewByOrderDetailAsync(int orderDetailId, User user);
        Task<ServiceResponse<List<ReviewDTO>>> GetReviewsByCustomerId(int userId);
    }
}

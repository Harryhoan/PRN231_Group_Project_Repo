using Application.ServiceResponse;
using Application.ViewModels.ReviewDTO;
using Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IReviewService
    {
        Task<ServiceResponse<ReviewDTO>> ReviewAsync(int orderId, ReviewRequest reviewRequest);
        Task<ServiceResponse<ReviewDTO>> GetReviewAsync(int reviewId);
        Task<ServiceResponse<ReviewDTO>> DeleteReviewAsync(int reviewId);
        Task<ServiceResponse<List<ReviewDTO>>> GetAllReviewAsync();
    }
}

﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IReviewRepo : IGenericRepo<Review>
    {
        Task<bool> checkIdExist(int id);
        Task<List<Review>> GetReviewsByUserIdAsync(int userId);
        Task<Review?> GetReviewByOrderDetailAsync(int orderDetailId);
        Task<Review?> GetReviewByProductIdAsync(int productId);
    }
}

using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReviewRepo : GenericRepo<Review>, IReviewRepo
    {
        private readonly ApiContext _dbContext;
        public ReviewRepo(ApiContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> checkIdExist(int id)
        {
            return await _dbContext.Reviews.AnyAsync(x => x.Id == id);
        }

        public async Task<List<Review>> GetReviewsByUserIdAsync(int userId)
        {
            return await _dbContext.Reviews.Where(r => r.OrderDetail.Order.UserId == userId).ToListAsync();
        }
        public async Task<Review?> GetReviewByOrderDetailAsync(int orderDetailId)
        {
            return await _dbContext.Reviews.FirstOrDefaultAsync(r => r.OrderDetailId == orderDetailId);
        }
        public async Task<Review?> GetReviewByProductIdAsync(int productId)
        {
            return await _dbContext.Reviews.FirstOrDefaultAsync(r => r.OrderDetail.KoiId == productId);
        }
    }
}

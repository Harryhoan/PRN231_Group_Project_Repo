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
            return await _dbContext.OrderDetails.AnyAsync(x => x.Id == id);
        }
    }
}

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
	public class OrderRepo : GenericRepo<Order>, IOrderRepo
	{
		private readonly ApiContext _dbContext;
		public OrderRepo(ApiContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Order?> aGetPendingOrderByUserIdAsync(int userId)
		{
			return await _dbContext.Orders.Include(o => o.OrderDetails).SingleOrDefaultAsync(o => o.UserId == userId && o.OrderStatus == false);
		}
	}
}

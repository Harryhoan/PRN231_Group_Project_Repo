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
	public class OrderDetailRepo : GenericRepo<OrderDetail>, IOrderDetailRepo
	{
		private readonly ApiContext _dbContext;
		public OrderDetailRepo(ApiContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<bool> CheckOrderDetailBelongingToUser(int orderDetaildId, int userId)
		{
			return await _dbContext.OrderDetails.AnyAsync(o => o.Id == orderDetaildId && o.Order.UserId == userId);
		}
	}
}

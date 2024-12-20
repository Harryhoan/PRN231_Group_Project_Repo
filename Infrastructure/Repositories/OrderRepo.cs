﻿using Application.IRepositories;
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

        public async Task<List<Order>> aGetOrdersByUser(int userId)
        {
            return await _dbContext.Orders.Include(o => o.OrderDetails).Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Order>> cGetAllOrders()
        {
            var orders = await _dbContext.Orders.Include(o => o.User).Include(o => o.OrderDetails).ToListAsync();
            return orders;
        }

        public int GetCountOrders()
        {
            return _dbContext.Orders.Count();
        }
    }
}

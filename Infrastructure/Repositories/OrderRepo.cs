using Application.IRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
	public class OrderRepo : GenericRepo<Order>, IOrderRepo
	{
		public OrderRepo(ApiContext context) : base(context)
		{
		}
	}
}

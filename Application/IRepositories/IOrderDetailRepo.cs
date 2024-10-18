﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
	public interface IOrderDetailRepo : IGenericRepo<OrderDetail>
	{
		Task<bool> CheckOrderDetailBelongingToUser(int orderDetaildId, int userId);
	}
}
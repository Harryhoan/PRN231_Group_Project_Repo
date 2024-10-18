﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
	public interface IOrderRepo : IGenericRepo<Order> 
	{
		Task<Order?> aGetPendingOrderByUserIdAsync(int userId);
	}
}

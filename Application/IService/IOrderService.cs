﻿using Application.ServiceResponse;
using Application.ViewModels.OrderDTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
	public interface IOrderService
	{
		Task<ServiceResponse<aOrderDTO>> GetCart(User user);
		Task<ServiceResponse<List<aOrderDTO>>> GetOrdersByUser(User user);

    }
}

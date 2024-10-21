using Application.ServiceResponse;
using Application.ViewModels.OrderDetailDTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
	public interface IOrderDetailService
	{
		Task<ServiceResponse<aViewOrderDetailDTO>> aCreateOrderDetailAsync(aCreateOrderDetailDTO cart, User user);
		Task<User> aGetUserByTokenAsync(ClaimsPrincipal claims);
		Task<ServiceResponse<bool>> aDeleteOrderDetailAsync(int id, User user);
	}
}

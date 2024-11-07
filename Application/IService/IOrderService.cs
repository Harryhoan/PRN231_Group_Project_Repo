using Application.ServiceResponse;
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
        Task<ServiceResponse<PaginationModel<cOrderDTO>>> cGetAllOrder(int page, int pageSize, string search, string filter, string sort);
		Task<ServiceResponse<List<aOrderDTO>>> GetAllOrders();
        int CountOrders();
    }
}

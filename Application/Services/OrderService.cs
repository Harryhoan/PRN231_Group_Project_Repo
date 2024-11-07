using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Utils;
using Application.ViewModels;
using Application.ViewModels.CategoryDTO;
using Application.ViewModels.KoiDTO;
using Application.ViewModels.OrderDetailDTO;
using Application.ViewModels.OrderDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IOrderRepo _orderRepo;
		public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IOrderRepo order)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_orderRepo = order;
		}

		public async Task<ServiceResponse<PaginationModel<cOrderDTO>>> cGetAllOrder(int page, int pageSize, string search, string filter, string sort)
		{
			var response = new ServiceResponse<PaginationModel<cOrderDTO>>();

			try
			{
				var orders = await _orderRepo.cGetAllOrders();
				if (!string.IsNullOrEmpty(search))
				{
					orders = orders.Where(o =>
						o.User != null && o.User.FullName.Contains(search, StringComparison.OrdinalIgnoreCase));
				}

				if (bool.TryParse(filter, out bool status))
				{
					orders = orders.Where(c => c.OrderStatus == status).ToList();
				}

				// Lọc đơn hàng dựa trên lựa chọn ComboBox
				orders = sort.ToLower() switch
				{
					"completed" => orders.Where(o => o.OrderStatus).OrderBy(o => o.OrderDate),  // Đơn hàng đã hoàn thành, sắp xếp theo ngày
					"pending" => orders.Where(o => !o.OrderStatus).OrderBy(o => o.OrderDate),   // Đơn hàng đang chờ, sắp xếp theo ngày
					_ => orders.OrderBy(o => o.Id) // Mặc định sắp xếp theo Id nếu không chọn gì
				};
				var orderDtOs = MapToDTO(orders); // Map products to ProductDTO

				// Apply pagination
				var paginationModel =
					await cPagination.GetPaginationIENUM(orderDtOs, page,
						pageSize); // Adjusted pageSize as per original example

				response.Data = paginationModel;
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = $"Failed to retrieve orders: {ex.Message}";
			}

            return response;
        }
        private List<cOrderDTO> MapToDTO(IEnumerable<Order> orders)
        {
            return orders.Select(order => MapToDTO(order)).ToList();
        }
        private cOrderDTO MapToDTO(Order order)
        {
            return new cOrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User.FullName,
                PaymentDate = order.OrderDate,
                Status = order.OrderStatus,
                OrderDetails = order.OrderDetails.Select(detail => new cOrderDetailsResDTO
                {
                    Id = detail.Id,
                    ProductId = detail.KoiId,
                    Price = detail.Price,
                    Quantity = detail.Quantity,
					ImageUrls = detail.Koi?.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>()
				}).ToList()
            };
        }

		public async Task<ServiceResponse<aOrderDTO>> GetCart(User user)
		{
			var response = new ServiceResponse<aOrderDTO>();

			try
			{
				var order = await _unitOfWork.OrderRepository.aGetPendingOrderByUserIdAsync(user.Id);
				if (order == null || order.OrderDetails == null)
				{
					throw new ArgumentNullException(nameof(order));
				}
				foreach (var orderDetail in order.OrderDetails)
				{
					var koi = await _unitOfWork.KoiRepo.dGetKoiWithCategoryAndImages(orderDetail.KoiId);
					if (koi == null || koi.Category == null)
					{
						throw new ArgumentException();
					}
				}
				response.Data = _mapper.Map<aOrderDTO>(order);
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = $"Failed to get cart: {ex.Message}";
			}
			return response;
		}

		public async Task<ServiceResponse<List<aOrderDTO>>> GetAllOrders()
		{
			var response = new ServiceResponse<List<aOrderDTO>>();
			try
			{
				var orders = await _unitOfWork.OrderRepository.cGetAllOrders();
				if (orders == null)
				{
					throw new ArgumentNullException(nameof(orders));
				}
				if (orders.Any() && orders.First().OrderDetails != null)
				{
					foreach (var order in orders)
					{
						foreach (var orderDetail in order.OrderDetails)
						{
							var koi = await _unitOfWork.KoiRepo.dGetKoiWithCategoryAndImages(orderDetail.KoiId);
							if (koi == null || koi.Category == null)
							{
								throw new ArgumentException();
							}
						}
					}
				}
				response.Data = _mapper.Map<List<aOrderDTO>>(orders);
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = $"Failed to get all orders: {ex.Message}";
			}
			return response;
		}

		public async Task<ServiceResponse<List<aOrderDTO>>> GetOrdersByUser(User user)
        {
            var response = new ServiceResponse<List<aOrderDTO>>();
            try
            {
                var orders = await _unitOfWork.OrderRepository.aGetOrdersByUser(user.Id);
                if (orders == null)
                {
                    throw new ArgumentNullException(nameof(orders));
                }
                var viewOrders = _mapper.Map<List<aOrderDTO>>(orders);
                foreach (var order in viewOrders)
                {
                    foreach (var orderDetail in order.OrderDetails)
                    {
                        var koi = await _unitOfWork.KoiRepo.dGetKoiWithCategoryAndImages(orderDetail.KoiId);
                        if (koi == null || koi.Category == null)
                        {
                            throw new ArgumentException();
                        }
                        orderDetail.CategoryName = koi.Category.Name;
                    }
                }
                response.Data = viewOrders;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to get cart: {ex.Message}";
            }
            return response;
        }

        public int CountOrders()
        {
            return _orderRepo.GetCountOrders();
        }
    }
}

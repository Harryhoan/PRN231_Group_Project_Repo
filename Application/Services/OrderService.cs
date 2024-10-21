using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.CategoryDTO;
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
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                var viewOrder = _mapper.Map<aOrderDTO>(order);
                foreach (var orderDetail in viewOrder.OrderDetails)
                {
                    var koi = await _unitOfWork.KoiRepo.dGetKoiWithCategory(orderDetail.KoiId);
                    if (koi == null || koi.Category == null)
                    {
                        throw new ArgumentException();
                    }
                    orderDetail.CategoryName = koi.Category.Name;
                }
                response.Data = viewOrder;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to get cart: {ex.Message}";
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
                        var koi = await _unitOfWork.KoiRepo.dGetKoiWithCategory(orderDetail.KoiId);
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
    }
}

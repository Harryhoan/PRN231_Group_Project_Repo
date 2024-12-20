﻿using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Utils;
using Application.ViewModels;
using Application.ViewModels.CategoryDTO;
using Application.ViewModels.KoiDTO;
using Application.ViewModels.OrderDetailDTO;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.UserDTO;
using AutoMapper;
using Domain.Entities;
using PayPal.Api;
using PayPal.Api.OpenIdConnect;
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
        public int CountOrders()
        {
            return _unitOfWork.OrderRepository.GetCountOrders();
        }
        public async Task<ServiceResponse<PaginationModel<cOrderDTO>>> cGetAllOrder(int page, int pageSize, string search, string filter, string sort)
        {
            var response = new ServiceResponse<PaginationModel<cOrderDTO>>();
            try
            {
                var orders = await _unitOfWork.OrderRepository.cGetAllOrders();
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
        private List<cOrderDTO> MapToDTO(IEnumerable<Domain.Entities.Order> orders)
        {
            return orders.Select(order => MapToDTO(order)).ToList();
        }
        private cOrderDTO MapToDTO(Domain.Entities.Order order)
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
                        throw new ArgumentException(nameof(koi));
                    }
                    orderDetail.Koi = koi;
                }
                if (order.AddressId == null)
                {
                    if (!(order.OrderDetails.Count > 0))
                    {
                        response.Message = "No Product Found";
                    }
                    var orderDto = _mapper.Map<aOrderDTO>(order);
                    response.Data = orderDto;
                    response.Success = true;

                }
                else
                {
                    var address = await _unitOfWork.AddressRepo.GetByIdAsync((int)order.AddressId);
                    order.Address = address;
                    if (!(order.OrderDetails.Count > 0))
                    {
                        response.Message = "No Product Found";
                    }
                    var orderDto = _mapper.Map<aOrderDTO>(order);
                    orderDto.Address = _mapper.Map<AddressDTO>(address);
                    response.Data = orderDto;
                    response.Success = true;
                }
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

                var orderDtos = new List<aOrderDTO>();

                if (orders.Any() && orders.First().OrderDetails != null)
                {
                    foreach (var order in orders)
                    {
                        foreach (var orderDetail in order.OrderDetails)
                        {
                            var koi = await _unitOfWork.KoiRepo.dGetKoiWithCategoryAndImages(orderDetail.KoiId);
                            if (koi == null || koi.Category == null)
                            {
                                throw new ArgumentException(nameof(koi));
                            }
                            orderDetail.Koi = koi;
                        }

                        if (order.AddressId != null)
                        {
                            var address = await _unitOfWork.AddressRepo.GetByIdAsync(order.AddressId.Value);
                            if (address == null)
                            {
                                throw new ArgumentException(nameof(address));
                            }
                            order.Address = address;
                        }

                        var orderDto = _mapper.Map<aOrderDTO>(order);
                        orderDtos.Add(orderDto);
                    }
                }

                response.Data = orderDtos;
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

                var orderDtos = new List<aOrderDTO>();

                if (orders.Count > 0 && orders.First().OrderDetails != null)
                {
                    foreach (var order in orders)
                    {
                        foreach (var orderDetail in order.OrderDetails)
                        {
                            var koi = await _unitOfWork.KoiRepo.dGetKoiWithCategoryAndImages(orderDetail.KoiId);
                            if (koi == null || koi.Category == null)
                            {
                                throw new ArgumentException(nameof(koi));
                            }
                            orderDetail.Koi = koi;
                        }

                        var address = order.AddressId.HasValue
                            ? await _unitOfWork.AddressRepo.GetByIdAsync(order.AddressId.Value)
                            : null;
                        //if (address == null)
                        //{
                        //    throw new ArgumentException(nameof(address));
                        //}

                        var orderDto = _mapper.Map<aOrderDTO>(order);
                        orderDto.Address = _mapper.Map<AddressDTO>(address);
                        orderDtos.Add(orderDto);
                    }
                }
                if (!(orders.Count > 0))
                {
                    response.Message = "No Order Found";
                }

                orderDtos.RemoveAll(x => !x.OrderStatus);
                response.Data = orderDtos;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to get orders by user: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<string>> UpdateAddress(int addressId, User user)
        {
            var response = new ServiceResponse<string>();
            try
            {

                var address = await _unitOfWork.UserRepository.GetAddressById(addressId);
                if (address == null)
                {
                    response.Success = false;
                    response.Message = "Address Id not found";
                    return response;
                }
                if (address.UserId != user.Id)
                {
                    response.Success = false;
                    response.Message = "You dont have permision to use this address";
                    return response;
                }
                // Update the product in the repository
                var order = await _unitOfWork.OrderRepository.aGetPendingOrderByUserIdAsync(user.Id);
                if (order == null)
                {
                    response.Success = false;
                    response.Message = "Order not found";
                    return response;
                }
                order.AddressId = addressId;
                await _unitOfWork.OrderRepository.Update(order);
                response.Message = "Adress Update Successfully";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to update address: {ex.Message}";
            }
            return response;
        }
    }
}
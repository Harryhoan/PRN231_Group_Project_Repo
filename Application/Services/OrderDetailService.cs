﻿using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.KoiDTO;
using Application.ViewModels.OrderDetailDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class OrderDetailService : IOrderDetailService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public OrderDetailService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<ServiceResponse<aViewOrderDetailDTO>> aCreateOrderDetailAsync(aCreateOrderDetailDTO cart, User user)
		{
			var response = new ServiceResponse<aViewOrderDetailDTO>();
			try
			{
				if (user == null || !(user.Id > 0) || cart == null)
				{
					throw new ArgumentNullException();
				}
				var koi = await _unitOfWork.KoiRepo.dGetKoiWithCategoryAndImages(cart.KoiId);
				if (koi == null || koi.Category == null || koi.Images == null)
				{
					throw new ArgumentNullException(nameof(koi));
				}
				if (!(koi.Quantity > 0 && koi.Quantity < 2))
				{
                    response.Success = false;
                    response.Message = "There is no more koi to add to cart";
					return response;
                }
                var order = await _unitOfWork.OrderRepository.aGetPendingOrderByUserIdAsync(user.Id);
				if (order == null || order.OrderDetails == null)
				{
					throw new ArgumentNullException(nameof(order));
				}
				if (order.OrderDetails.Any(o => o.KoiId == koi.Id))
				{
                    response.Success = false;
                    response.Message = "The cart already has this koi";
					return response;
                }
                await _unitOfWork.KoiRepo.Update(koi);
				var orderDetail = _mapper.Map<OrderDetail>(cart);
				orderDetail.Price = koi.Price;
				orderDetail.OrderId = order.Id;
				await _unitOfWork.OrderDetailRepository.AddAsync(orderDetail);
				order.TotalPrice += orderDetail.Price;
				await _unitOfWork.OrderRepository.Update(order);
				var viewCart = _mapper.Map<aViewOrderDetailDTO>(orderDetail);
				viewCart.Koi = _mapper.Map<dViewKoiDTO>(koi);
				response.Data = viewCart;
				response.Success = true;
				response.Message = "Cart added successfully";
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = $"Failed to create a detail of an order: {ex.Message}";
			}
			return response;
		}

		public async Task<bool> aCheckIfTheOrderDetailHasOrderWithUserId(int id, int userId)
		{
			try
			{
				if (!(userId > 0) || !(id > 0))
				{
					return false;
				}
				return await _unitOfWork.OrderDetailRepository.CheckOrderDetailBelongingToUser(id, userId);
			}
			catch
			{
				return false;
			}

		}

		public async Task<ServiceResponse<bool>> aDeleteOrderDetailAsync(int id, User user)
		{
			var response = new ServiceResponse<bool>();
			try
			{
				var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
				if (orderDetail == null)
				{
					throw new ArgumentNullException(nameof(orderDetail));
				}
				var existingOrder = await _unitOfWork.OrderRepository.aGetPendingOrderByUserIdAsync(user.Id);
				if (existingOrder == null || orderDetail.OrderId != existingOrder.Id)
				{
                    response.Success = false;
                    response.Message = "Cannot delete order detail";
					return response;
                }
    //            if (!(await aCheckIfTheOrderDetailHasOrderWithUserId(id, user.Id)))
				//{
				//	throw new UnauthorizedAccessException();
				//}
				await _unitOfWork.OrderDetailRepository.cDeleteTokenAsync(orderDetail);
				existingOrder.TotalPrice -= orderDetail.Price;
				await _unitOfWork.OrderRepository.Update(existingOrder);

				response.Data = true;
				response.Success = true;
				response.Message = "Cart deleted successfully";
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = $"Failed to delete a detail of an order: {ex.Message}";
			}
			return response;
		}


		public async Task<User> aGetUserByTokenAsync(ClaimsPrincipal claims)
		{
			if (claims == null)
			{
				throw new ArgumentNullException("Invalid token");
			}
			var userId = claims.FindFirst("Id")?.Value;
			if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
			{
				throw new ArgumentException("No user can be found");
			}

			var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
			if (user == null)
			{
				throw new NullReferenceException("No user can be found");
			}
			return user;
		}
	}
}

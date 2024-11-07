using Application.IService;
using Application.ServiceResponse;
using Application.Utils;
using Application.ViewModels.OrderDTO;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PayPal;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration _configuration;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public PaymentService(IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration)
		{
			_configuration = configuration;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		public async Task<ServiceResponse<Payment>> CreatePaymentAsync(int userId, string returnUrl, string cancelUrl)
		{
			var response = new ServiceResponse<Payment>();

			try
			{
				// Retrieve the pending order for the user
				//var order = await _unitOfWork.OrderRepository.aGetPendingOrderByUserIdAsync(userId);

				//if (order == null || order.OrderStatus != false)
				//{
				//	response.Success = false;
				//	response.Message = "Order not found.";
				//	return response;
				//}
				//if (order.OrderDetails == null || order.OrderDetails.Count <= 0)
				//{
				//	response.Success = false;
				//	response.Message = "No product found.";
				//	return response;
				//}


				//int i = 0;
				//while (i < order.OrderDetails.Count)
				//{
				//	var koi = await _unitOfWork.KoiRepo.GetByIdAsync(order.OrderDetails.ElementAt(i).KoiId);
				//	if (koi != null && koi.Price > 0 && koi.Quantity > 0)
				//	{
				//		if (!(order.OrderDetails.ElementAt(i).Price > 0) || order.OrderDetails.ElementAt(i).Price < koi.Price)
				//		{
				//			if (order.OrderDetails.ElementAt(i).Quantity < 1)
				//			{
				//				order.OrderDetails.ElementAt(i).Quantity = 1;
				//			}
				//			order.OrderDetails.ElementAt(i).Price = koi.Price * order.OrderDetails.ElementAt(i).Quantity;
				//		}
				//		i++;
				//		continue;
				//	}
				//	var id = order.OrderDetails.ElementAt(i);
				//	order.OrderDetails.Remove(order.OrderDetails.ElementAt(i));
				//	await _unitOfWork.OrderDetailRepository.cDeleteTokenAsync(id);
				//}

				//if (order.OrderDetails == null || order.OrderDetails.Count <= 0)
				//{
				//	response.Success = false;
				//	response.Message = "The cart is invalid.";
				//	return response;
				//}

				//if (!(order.TotalPrice > 0))
				//{
				//	order.TotalPrice = order.OrderDetails.Sum(o => o.Price);
				//	await _unitOfWork.OrderRepository.Update(order);
				//}

				var order = await ValidateOrder(userId);
				if (order == null)
				{
					response.Success = false;
					response.Message = string.IsNullOrEmpty(errorMessage) ? "The order is invalid. Please try again." : errorMessage;
					return response;
				}

				decimal conversionRate = 23000m;
				decimal totalAmountVND = order.TotalPrice;
				decimal totalAmountUSD = totalAmountVND / conversionRate;
				string totalAmountInUSD = totalAmountUSD.ToString("F2");

				var apiContext = new APIContext(new OAuthTokenCredential(
					_configuration["PayPal:ClientId"],
					_configuration["PayPal:ClientSecret"]
				).GetAccessToken());

				var payment = new Payment
				{
					intent = "sale",
					payer = new Payer { payment_method = "paypal" },
					transactions = new List<Transaction>
			{
				new Transaction
				{
					description = $"Order {order.Id} - Payment",
					invoice_number = order.Id.ToString(),
					amount = new Amount
					{
						currency = "USD",
						total = totalAmountInUSD
					},
					custom = order.UserId.ToString()
				}
			},
					redirect_urls = new RedirectUrls
					{
						cancel_url = cancelUrl,
						return_url = returnUrl
					}
				};

				var createdPayment = payment.Create(apiContext);

				// Return the response
				response.Success = true;
				response.Message = "Payment created successfully.";
				response.Data = createdPayment;
			}
			catch (PayPalException payPalEx)
			{
				response.Success = false;
				response.Message = $"PayPal error: {payPalEx.Message}";
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = $"Failed to create payment: {ex.Message}";
			}

			return response;
		}

		public async Task<ServiceResponse<Payment>> ExecutePaymentAsync(string paymentId, string payerId)
		{
			var response = new ServiceResponse<Payment>();
			try
			{
				var apiContext = new APIContext(new OAuthTokenCredential(
					_configuration["PayPal:ClientId"],
					_configuration["PayPal:ClientSecret"]
				).GetAccessToken());

				var payment = Payment.Get(apiContext, paymentId);

				if (payment == null || string.IsNullOrEmpty(payment.state) || !(payment.transactions.Count > 0) || !int.TryParse(payment.transactions.First().custom, out int userId) || !(userId > 0) || !int.TryParse(payment.transactions.First().invoice_number, out int orderId) || !(orderId > 0))
				{
					response.Success = false;
					response.Message = "Payment not found or invalid.";
					return response;
				}

				//if (payment.state != "approved")
				//{
				//	response.Success = false;
				//	response.Message = "Payment is not approved yet.";
				//	return response;
				//}

				var order = await ValidateOrder(userId);

				if (!string.IsNullOrEmpty(errorMessage) || order == null || order.Id != orderId || order.OrderDetails == null || !(order.OrderDetails.Count > 0))
				{
					if (payment.state == "authorized")
					{
						var authorization = new Authorization() { id = payment.id };
						var voidResponse = authorization.Void(apiContext);
					}
					response.Success = false;
					response.Message = string.IsNullOrEmpty(errorMessage) ? "The order is invalid. Please try again." : errorMessage;
					return response;

				}

				order.OrderStatus = true;
				order.OrderDate = DateTime.Now;
				await _unitOfWork.OrderRepository.Update(order);
				foreach (var orderDetail in order.OrderDetails)
				{
					var koi = await _unitOfWork.KoiRepo.GetByIdAsync(orderDetail.KoiId);
					if (koi != null)
					{
						koi.Quantity -= orderDetail.Quantity;
						await _unitOfWork.KoiRepo.Update(koi);
					}
				}

				// Prepare to execute the payment
				var paymentExecution = new PaymentExecution() { payer_id = payerId };
				var executedPayment = payment.Execute(apiContext, paymentExecution);

				if (executedPayment == null || string.IsNullOrEmpty(executedPayment.state) || executedPayment.state == "failed")
				{
					response.Success = false;
					response.Message = "Payment executed unsuccessfully.";
					return response;
				}

				var cart = new Domain.Entities.Order();
				cart.UserId = userId;
				cart.ShippingFee = 0;
				cart.TotalPrice = 0;
				await _unitOfWork.OrderRepository.AddAsync(cart);

				response.Success = true;
				response.Message = "Payment executed successfully.";
				response.Data = executedPayment;
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = $"Failed to execute payment: {ex.Message}";
			}

			return response;
		}

		private string errorMessage = string.Empty;

		public async Task<Domain.Entities.Order?> ValidateOrder(int userId)
		{
			try
			{
				var order = await _unitOfWork.OrderRepository.aGetPendingOrderByUserIdAsync(userId);

				if (order == null || order.OrderStatus != false)
				{
					errorMessage = "The details of the order have been changed and the payment cannot be proceeded with.";
					return null;
				}
				if (order.OrderDetails == null || order.OrderDetails.Count <= 0)
				{
					errorMessage = "No product found.";
					return null;
				}

				int i = 0;
				while (i < order.OrderDetails.Count)
				{
					var koi = await _unitOfWork.KoiRepo.GetByIdAsync(order.OrderDetails.ElementAt(i).KoiId);
					if (koi != null && koi.Price > 0 && koi.Quantity > 0)
					{
						if (!(order.OrderDetails.ElementAt(i).Price > 0) || order.OrderDetails.ElementAt(i).Price < koi.Price)
						{
							if (order.OrderDetails.ElementAt(i).Quantity < 1)
							{
								order.OrderDetails.ElementAt(i).Quantity = 1;
							}
							order.OrderDetails.ElementAt(i).Price = koi.Price * order.OrderDetails.ElementAt(i).Quantity;
							errorMessage = "The details of the order have been changed. Please try again.";
						}
						i++;
						continue;
					}
					errorMessage = "The details of the order have been changed. Please try again.";
					var id = order.OrderDetails.ElementAt(i);
					order.OrderDetails.Remove(order.OrderDetails.ElementAt(i));
					await _unitOfWork.OrderDetailRepository.cDeleteTokenAsync(id);
				}

				if (order.OrderDetails == null || order.OrderDetails.Count <= 0)
				{
					errorMessage = "The cart is invalid.";
					return null;
				}

				if (!(order.TotalPrice > 0))
				{
					order.TotalPrice = order.OrderDetails.Sum(o => o.Price);
					await _unitOfWork.OrderRepository.Update(order);
					errorMessage = "The details of the order have been changed. Please try again.";
				}

				return order;
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return null;
			}
		}
	}

}




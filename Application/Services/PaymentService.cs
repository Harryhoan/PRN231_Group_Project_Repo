using Application.IService;
using Application.ServiceResponse;
using Application.Utils;
using Application.ViewModels.OrderDTO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IMapper mapper, IUnitOfWork unitOfWork,IConfiguration configuration)
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
                var order = await _unitOfWork.OrderRepository.aGetPendingOrderByUserIdAsync(userId);

                if (order == null)
                {
                    response.Success = false;
                    response.Message = "Order not found.";
                    return response;
                }

                // Setup PayPal API context
                var apiContext = new APIContext(new OAuthTokenCredential(
                    _configuration["PayPal:ClientId"],
                    _configuration["PayPal:ClientSecret"]
                ).GetAccessToken());

                // Create the payment object
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
                        currency = "VND",
                        total = order.TotalPrice.ToString("F2") // Format to two decimal places
                    }
                }
            },
                    redirect_urls = new RedirectUrls
                    {
                        cancel_url = cancelUrl,
                        return_url = returnUrl
                    }
                };

                // Create the payment
                var createdPayment = payment.Create(apiContext);

                // Return the response
                response.Success = true;
                response.Message = "Payment created successfully.";
                response.Data = createdPayment; // Return the created payment object
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

                var paymentExecution = new PaymentExecution() { payer_id = payerId };
                var payment = new Payment() { id = paymentId };

                var executedPayment = payment.Execute(apiContext, paymentExecution);

                response.Success = true;
                response.Message = "Payment executed successfully.";
                response.Data = executedPayment; // Return the executed payment object
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to execute payment: {ex.Message}";
            }

            return response;
        }

    }



}




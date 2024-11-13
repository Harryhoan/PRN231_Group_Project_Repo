using Application.ServiceResponse;
using Application.ViewModels.OrderDTO;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
	public interface IPaymentService
	{
		Task<ServiceResponse<string>> CreatePaymentAsync(int userId, string returnUrl, string cancelUrl, int addressId);

        Task<ServiceResponse<Payment>> ExecutePaymentAsync(string paymentId, string payerId);
	}
}

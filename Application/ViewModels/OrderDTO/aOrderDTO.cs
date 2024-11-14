using Application.ViewModels.OrderDetailDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.ViewModels.OrderDTO
{
	public class aOrderDTO
	{
		public int Id { get; set; }
		public DateTime OrderDate { get; set; }
		public bool OrderStatus { get; set; }
		public decimal ShippingFee { get; set; }
		public decimal TotalPrice { get; set; }
		public int UserId { get; set; }

		//[JsonIgnore]
		public UserDTO.AddressDTO? Address { get; set; } = null;
		public List<aViewOrderDetailDTO> OrderDetails { get; set; } = new List<aViewOrderDetailDTO>();

	}
}

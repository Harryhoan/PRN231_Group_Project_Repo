using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.OrderDetailDTO
{
	public class aCreateOrderDetailDTO
	{
		[Required(ErrorMessage = "Quantity is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be a non-negative number")]
		public int Quantity { get; set; }

		[Required(ErrorMessage = "Koi is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Koi's identifier must be a non-negative number")]
		public int KoiId { get; set; }

	}
}

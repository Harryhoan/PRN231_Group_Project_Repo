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
		[Range(1, 1, ErrorMessage = "Quantity must be 1")]
		public int Quantity { get; set; } = 1;

		[Required(ErrorMessage = "Koi is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Koi's identifier must be a positive integer")]
		public int KoiId { get; set; } = 0;
    }
}

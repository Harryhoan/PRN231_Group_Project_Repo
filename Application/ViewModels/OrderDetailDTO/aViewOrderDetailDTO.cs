﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.OrderDetailDTO
{
	public class aViewOrderDetailDTO
	{
		public int Id { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public int KoiId { get; set; }
		public string CategoryName { get; set; } = string.Empty;
	}
}

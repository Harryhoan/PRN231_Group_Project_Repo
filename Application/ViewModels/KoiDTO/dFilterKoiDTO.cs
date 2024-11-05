using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.KoiDTO
{
    public class dFilterKoiDTO
    {

        public int? Id { get; set; } = null;
		[StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
		public string? Name { get; set; } = null;

		[StringLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
        public string? Description { get; set; } = null;

		[Range(0, double.MaxValue, ErrorMessage = "Minimum price must be a non-negative number")]
        public decimal? MinPrice { get; set; } = null;
		[Range(0, double.MaxValue, ErrorMessage = "Maximum price must be a non-negative number")]
        public decimal? MaxPrice { get; set; } = null;
		[Range(0, int.MaxValue, ErrorMessage = "Minimum size must be a non-negative integer")]
        public int? MinSize { get; set; } = null;
		[Range(0, int.MaxValue, ErrorMessage = "Maximum size must be a non-negative integer")]
        public int? MaxSize { get; set; } = null;
		[DataType(DataType.Date, ErrorMessage = "Invalid start date")]
        public DateTime? DobStart { get; set; } = null;
		[DataType(DataType.Date, ErrorMessage = "Invalid end date")]
        public DateTime? DobEnd { get; set; } = null;
		//[Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive integer")]
  //      public int? CategoryId { get; set; } = null;

		[StringLength(100, ErrorMessage = "Category Name can't be longer than 100 characters")]
		public string? CategoryName { get; set; } = null;
		[Range(0, int.MaxValue, ErrorMessage = "Page Number must be a positive integer")]
		public int PageNumber { get; set; } = 1;
		[Range(0, int.MaxValue, ErrorMessage = "Page Size must be a positive integer")]
		public int PageSize { get; set; } = 10;
    }

}

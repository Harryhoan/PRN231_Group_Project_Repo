using Application.ViewModels.ImageDTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.KoiDTO
{
    public class dViewKoiDTO
    {
		[Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer")]
		public int Id { get; set; }
		[Required(ErrorMessage = "Name is required")]
		[StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
		public string Name { get; set; } = string.Empty;

		[StringLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative number")]

        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non - negative integer")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Size is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Size must be a non-negative integer")]
        public int Size { get; set; }
        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date of birth")]
        public DateTime Dob
        { get; set; }
        [Required(ErrorMessage = "Category ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive integer")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        public string CategoryName { get; set; } = string.Empty;

        public List<aImageDTO> Images { get; set; } = new();

    }
}

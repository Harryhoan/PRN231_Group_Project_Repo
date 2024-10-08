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

        [StringLength(100, ErrorMessage="Too long")]
        public string? Query { get; set; }

        [Range(0, double.MinValue, ErrorMessage = "Minimum price must be a non-negative number")]
        public decimal? MinPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Maximum price must be a non-negative number")]
        public decimal? MaxPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Minimum size must be a non-negative integer")]
        public int? MinSize { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Maximum size must be a non-negative integer")]
        public int? MaxSize { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid start date")]
        public DateTime? DobStart { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid end date")]
        public DateTime? DobEnd { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive integer")]
        public int? CategoryId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}

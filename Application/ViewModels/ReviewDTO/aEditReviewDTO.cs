using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ReviewDTO
{
    public class aEditReviewDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Comment is required")]
        [StringLength(100, ErrorMessage = "Comment can't be longer than 100 characters")]
        public string Comment { get; set; } = string.Empty;
        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be an integer between 1 and 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "OrderDetailId is required")]
        public int OrderDetailId { get; set; }

    }
}

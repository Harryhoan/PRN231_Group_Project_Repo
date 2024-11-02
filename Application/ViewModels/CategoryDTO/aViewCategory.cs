using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.CategoryDTO
{
    public class aViewCategory
    {
        [Required(ErrorMessage = "Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters.")]

        public string Name { get; set; } = string.Empty;
    }
}

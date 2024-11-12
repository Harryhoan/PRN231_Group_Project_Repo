using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserDTO
{
    public class ProfileDTO
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name can't be longer than 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email can't be longer than 100 characters.")]
        public string Email { get; set; }
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telephone number must be 10 to 11 digits.")]
        public string? TelephoneNumber { get; set; }
    }

    public class AddressDTO
    {
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Province is required.")]
        [StringLength(100, ErrorMessage = "Province can't be longer than 100 characters.")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        [StringLength(200, ErrorMessage = "Street can't be longer than 200 characters.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Ward is required.")]
        [StringLength(100, ErrorMessage = "Ward can't be longer than 100 characters.")]
        public string Ward { get; set; }
    }
}

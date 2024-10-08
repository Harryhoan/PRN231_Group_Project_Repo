using System.ComponentModel.DataAnnotations;
namespace Application.ViewModels
{
    public class cKOIDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name can't be longer than 100 characters")]
        public string NameProduct { get; set; }

        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
        public string DescriptionProduct { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [ValidateType(typeof(double), ErrorMessage = "Price must be a valid number")]
        [NonNegative(ErrorMessage = "Price must be a non-negative number")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [NonNegative(ErrorMessage = "Quantity must be a non-negative integer")]
        [ValidateType(typeof(int), ErrorMessage = "Quantity must be a valid number")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        [ValidateType(typeof(int), ErrorMessage = "Category ID must be an integer")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Size is required")]
        [ValidateType(typeof(int), ErrorMessage = "Size must be an integer")]
        [NonNegative(ErrorMessage = "Price must be a non-negative number")]
        public int Size { get; set; }
        public List<string?>? ImageUrls { get; set; }
    }
    public class NonNegativeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value is int intValue && intValue < 0)
                {
                    return new ValidationResult(ErrorMessage);
                }
                else if (value is double doubleValue && doubleValue < 0)
                {
                    return new ValidationResult(ErrorMessage);
                }
                else if (value is string stringValue)
                {
                    if (!double.TryParse(stringValue, out double parsedValue) || parsedValue < 0)
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
            }

            return ValidationResult.Success;
        }
    }

    public class ValidateTypeAttribute : ValidationAttribute
    {
        private readonly Type _expectedType;

        public ValidateTypeAttribute(Type expectedType)
        {
            _expectedType = expectedType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value.GetType() != _expectedType)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}

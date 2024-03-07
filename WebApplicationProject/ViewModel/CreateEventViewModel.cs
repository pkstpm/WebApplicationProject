using System.ComponentModel.DataAnnotations;
using WebApplicationProject.Areas.Identity.Data;
using WebApplicationProject.Models;

namespace WebApplicationProject.ViewModel
{
    public class CreateEventViewModel
    {

        [Required(ErrorMessage = "Title is required.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Activity time is required.")]
        [FutureDate(ErrorMessage = "Activity time cannot be in the past.")]
        public DateTime ActivityTime { get; set; }

        [Required(ErrorMessage = "Expire time is required.")]
        [FutureDate(ErrorMessage = "Activity time cannot be in the past.")]
        
        public DateTime ExpireTime { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public required string Location { get; set; }

        [Required(ErrorMessage = "Detail is required.")]
        public required string Detail { get; set; }

        public required string Contact { get; set; }

        public int Capacity { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateValue = (DateTime)value;
            if (dateValue < DateTime.Now)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}

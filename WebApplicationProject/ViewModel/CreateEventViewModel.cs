using System.ComponentModel.DataAnnotations;
using WebApplicationProject.Areas.Identity.Data;
using WebApplicationProject.Models;

namespace WebApplicationProject.ViewModel
{
    public class CreateEventViewModel
    {

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Activity time is required.")]
        [FutureDate(ErrorMessage = "Activity time cannot be in the past.")]
        public DateTime ActivityTime { get; set; }

        [Required(ErrorMessage = "Expire time is required.")]
        [FutureDate(ErrorMessage = "Activity time cannot be in the past.")]
        
        public DateTime ExpireTime { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Detail is required.")]
        public string Detail { get; set; }

        public string Contact { get; set; }

        public int Capacity { get; set; }

        public ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
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

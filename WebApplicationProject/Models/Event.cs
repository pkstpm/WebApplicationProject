using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplicationProject.Areas.Identity.Data;

namespace WebApplicationProject.Models
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public DateTime ActivityTime { get; set; }
        [Required]
        public DateTime ExpireTime { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Detail { get; set; }
        [Required]
        public string Contact { get; set; }

        public bool IsOpen { get; set; }

        public int Capacity { get; set; }
        public int Amount { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<UserEvent>? UserEvents { get; set; }

        public ICollection<Comment>? Comments { get; set; }
    }

    public enum Category
    {
        Aerobic,
        Badminton,
        Basketball,
        Bicycle,
        Football,
        Futsal,
        Skateboard,
        Swimming,
        Volleyball,
        Other
    }
}

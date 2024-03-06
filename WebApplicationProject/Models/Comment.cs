using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplicationProject.Areas.Identity.Data;
using WebApplicationProject.Data;

namespace WebApplicationProject.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }  // Foreign key referencing the user who authored the comment
        public ApplicationUser User { get; set; }

        [ForeignKey("Event")]
        public Guid? EventId { get; set; }
        public Event Event { get; set; }

        [Required]
        public string Detail { get; set; }  // Content of the comment

        public DateTime CreatedAt { get; set; }  // Timestamp of when the comment was created
    }
}

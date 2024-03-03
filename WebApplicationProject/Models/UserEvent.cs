using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplicationProject.Areas.Identity.Data;

namespace WebApplicationProject.Models
{
    public class UserEvent
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("User")]
        [Column(Order = 1)]
        [Required]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        [ForeignKey("Event")]
        [Column(Order = 2)]
        [Required]
        public Guid EventID { get; set; }
        public Event Event { get; set; }

        [Required]
        public bool IsJoin { get; set; }
    }
}

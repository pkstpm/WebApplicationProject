using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplicationProject.Areas.Identity.Data;

namespace WebApplicationProject.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public string Detail { get; set; }
    }
}

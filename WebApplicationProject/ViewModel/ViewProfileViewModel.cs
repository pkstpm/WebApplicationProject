using WebApplicationProject.Areas.Identity.Data;
using WebApplicationProject.Models;

namespace WebApplicationProject.ViewModel
{
    public class ViewProfileViewModel
    {
        public ApplicationUser User { get; set; }
        public List<Event> MyEvents { get; set; }
        public List<Event> JoinedEvents{ get; set; }
    }
}

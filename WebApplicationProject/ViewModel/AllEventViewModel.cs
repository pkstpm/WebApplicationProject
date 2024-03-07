using WebApplicationProject.Models;

namespace WebApplicationProject.ViewModel
{
    public class AllEventViewModel
    {
        public List<Event> MyEvents { get; set; } = [];
        public List<Event> JoinedEvents { get; set; } = [];
        public List<Event> OtherEvents { get; set; } = [];
    }
}

namespace WebApplicationProject.ViewModel
{
    public class EditEventViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Detail { get; set; }

        public string Location { get; set; }

        public string Contact { get; set; }

        public DateTime ActivityTime { get; set; }

        public DateTime ExpireTime {  get; set; }

        public int Capacity { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace WebCalendarAPI.ModelInputs
{
    public class EventsInput
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

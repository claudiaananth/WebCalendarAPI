using System.ComponentModel.DataAnnotations;

namespace WebCalendarAPI.ModelInputs
{
    public class EventsInsertInput : EventsInput
    {
        public List<string> Email { get; set; }
    }
}

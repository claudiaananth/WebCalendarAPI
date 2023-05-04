using System.ComponentModel.DataAnnotations;

namespace WebCalendarAPI.ModelInputs
{
    public class EventsPatchInputs : EventsInput
    {
        [Required]
        public Guid EventId { get; set; }
    }
}

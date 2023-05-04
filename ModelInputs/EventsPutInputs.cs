using System.ComponentModel.DataAnnotations;

namespace WebCalendarAPI.ModelInputs
{
    public class EventsPutInputs : EventsInput
    {
        [Required]
        public Guid EventId { get; set; }

        public string Email { get; set; }
    }
}

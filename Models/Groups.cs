using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebCalendarAPI.Models
{
    public class Groups
    {
        [Key]
        public Guid Id { get; set; }
        
        public virtual List<Events> EventsIds { get; set; }
    }
}

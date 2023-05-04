﻿namespace WebCalendarAPI.Models
{
    public class Events
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

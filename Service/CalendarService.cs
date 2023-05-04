using Microsoft.EntityFrameworkCore;
using WebCalendarAPI.Controllers;
using WebCalendarAPI.Models;

namespace WebCalendarAPI.Service
{
    public class CalendarService
    {
        private readonly ApplicationDbContext _dbContext;
        public CalendarService(ILogger<CalendarService> logger, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckTiming(Guid userId, DateTime StartDate, DateTime EndDate)
        {
            var TimeDate = await _dbContext.Events
                       .Where(Q => Q.UserId == userId)
                       .Select(Q => new
                       {
                           TimeStart = Q.StartDate.TimeOfDay,
                           TimeEnd = Q.EndDate.TimeOfDay,
                           DateStart = Q.StartDate.Date,
                           DateEnd = Q.EndDate.Date,
                       })
                       .ToListAsync();

            // check if the event already found in DB 
            var foundEvents = TimeDate
                             .Where(Q => (StartDate.TimeOfDay >= Q.TimeStart &&
                                         EndDate.TimeOfDay <= Q.TimeEnd) &&
                                         (StartDate.Date >= Q.DateStart))
                             .Any();

            return foundEvents;
        }
    }
}

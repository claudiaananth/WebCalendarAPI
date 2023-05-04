using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebCalendarAPI.ModelInputs;
using WebCalendarAPI.Models;
using WebCalendarAPI.Service;

namespace WebCalendarAPI.Controllers
{
   
        [ApiController]
        [Route("[controller]")]
        public class CalendarController : ControllerBase
        {

            private readonly ILogger<CalendarController> _logger;
            private readonly ApplicationDbContext _dbContext;
            private readonly CalendarService _calendarService;

            public CalendarController(ILogger<CalendarController> logger, ApplicationDbContext dbContext, CalendarService calendarService)
            {
                _logger = logger;
                _dbContext = dbContext;
                _calendarService = calendarService;
            }

            [HttpGet(Name = "GetCalendarEvents")]
            public async Task<List<Events>> Get(Guid userId)
            {

                var events = await _dbContext.Events
                        .Where(Q => Q.UserId == userId)
                        .ToListAsync();

               return events;
            }

            [HttpPost(Name = "PostCalendarEvents")]
            public async Task<List<Events>> Insert(EventsInsertInput input)
            {
               
                var users = await _dbContext.Users.
                                Where(Q => input.Email.Contains(Q.Email)).
                                ToListAsync();

                if (users == null)
                {
                    throw new Exception("user not found");
                }

                var eventList = new List<Events>();
                var GroupingId = Guid.NewGuid();

                foreach(var user in users)
                {
                    var checkExsisting = await _calendarService.CheckTiming(user.Id, input.StartDate, input.EndDate);

                    if (checkExsisting == false)
                    {

                        var events = new Events
                        {
                            StartDate = input.StartDate,
                            EndDate = input.EndDate,
                            Description = input.Description,
                            Id = Guid.NewGuid(),
                            Title = input.Title,
                            UserId = user.Id,
                            GroupId = GroupingId,
                        };

                        eventList.Add(events);

                    }
                    else
                    {
                        continue;
                    }
                }

                if( eventList.Count == 0) 
                {
                    throw new Exception("None Of The Participant Can Join The Event");
                }

                var Grouping = new Groups
                {
                    Id = GroupingId,
                    EventsIds = eventList,
                };

                _dbContext.Groups.Add(Grouping);
                _dbContext.Events.AddRange(eventList);

                await _dbContext.SaveChangesAsync();

                return eventList;

            }

            [HttpPut(Name = "PutCalendarEvents")]
            public async Task<List<Events>> Put(EventsPutInputs input)
            {
                var eventsGroups = await (from e in _dbContext.Events
                                    join g in _dbContext.Groups on e.GroupId equals g.Id
                                    where e.Id == input.EventId
                                    select new
                                    {   
                                        groups = g.EventsIds,
                                    })
                                    .FirstOrDefaultAsync(); 

                if (eventsGroups == null)
                {
                    throw new Exception("Event Not Found");
                }

                var eventList = new List<Events>();

                foreach (var data in eventsGroups.groups)
                {
                    if (data.Id == input.EventId)
                    {
                        var user = await _dbContext.Users
                            .Where(Q => Q.Email == input.Email)
                            .FirstOrDefaultAsync();

                        if (user == null)
                        {
                            throw new Exception("User Not Found");
                        }

                        data.UserId = user.Id;
                    }

                    var checkExisting = await _calendarService.CheckTiming(data.UserId, input.StartDate, input.EndDate);

                    if(checkExisting == false)
                    {
                        data.Title = input.Title;
                        data.Description = input.Description;
                        data.StartDate = input.StartDate;
                        data.EndDate = input.EndDate;

                        eventList.Add(data);
                    }
                    else
                    {
                        continue;
                    }
                }

                _dbContext.Events.UpdateRange(eventList);

                await _dbContext.SaveChangesAsync();

                return eventList;
            }

            [HttpPatch(Name = "PatchCalendarEvents")]
            public async Task<List<Events>> Patch(EventsPatchInputs input)
            {
                var events = await (from e in _dbContext.Events
                                    join g in _dbContext.Groups on e.GroupId equals g.Id
                                    where e.Id == input.EventId
                                    select new
                                    {
                                        groups = g.EventsIds,
                                    })
                                   .FirstOrDefaultAsync();

                if (events == null)
                {
                    throw new Exception("Event Not Found");
                }

                var eventList = new List<Events>();

                foreach (var data in events.groups)
                {

                    var checkExisting = await _calendarService.CheckTiming(data.UserId, input.StartDate, input.EndDate);

                    if (checkExisting == false)
                    {
                        data.Title = input.Title;
                        data.Description = input.Description;
                        data.StartDate = input.StartDate;
                        data.EndDate = input.EndDate;

                        eventList.Add(data);
                    }
                    else
                    {
                        continue;
                    }
                }

                _dbContext.Events.UpdateRange(eventList);

                await _dbContext.SaveChangesAsync();

                return eventList;
            }

            [HttpDelete(Name = "DeleteCalendarEvents")]
            public async Task<string> Delete(Guid EventId)
            {
                var events = await _dbContext.Events.
                                Where(Q => Q.Id == EventId).
                                FirstOrDefaultAsync();

                if(events == null)
                {
                    throw new Exception("event not found");
                }

                var group = await _dbContext.Groups
                            .FirstOrDefaultAsync(Q => events.GroupId == Q.Id);

                if (group == null) {
                    throw new Exception("group not found");
                }

                _dbContext.Groups.Remove(group);
                _dbContext.Events.Remove(events);
                await _dbContext.SaveChangesAsync();

                return "Event with this Guid " + EventId + " has been deleted";
            }

        }
    
}

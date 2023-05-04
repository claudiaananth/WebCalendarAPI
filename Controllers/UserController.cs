using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebCalendarAPI.ModelInputs;
using WebCalendarAPI.Models;

namespace WebCalendarAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<CalendarController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public UserController(ILogger<CalendarController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<List<User>> Get()
        {
            return await _dbContext.Users.ToListAsync();
        }

        [HttpPost(Name = "PostUser")]
        public async Task<User?> Insert(UserInputs input)
        {
            var newUser = await _dbContext.Users.
                            Where(Q => Q.Email == input.Email).
                            FirstOrDefaultAsync();

            if (newUser == null)
            {

                if(input.ConfirmPassword.Equals(input.Password))
                {
                    newUser = new User
                    {
                        Email = input.Email,
                        Id = Guid.NewGuid(),
                        Name = input.Name,
                        Password = input.Password
                    };

                    _dbContext.Users.Add(newUser);

                    await _dbContext.SaveChangesAsync();
                }

            }
            else
            {
                throw new Exception("User already Registered!");
            }

            return newUser;
        }

        [HttpPut(Name = "PutUsers")]
        public async Task<User> Put(UserInputs input)
        {

            if (input.UserId == null)
            {
                throw new Exception("Must Input UserId");
            }

            var user = await _dbContext.Users.
                            Where(Q => Q.Id == input.UserId).
                            FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User Not Found");
            }

            user.Email = input.Email;
            user.Password = input.Password;
            user.Name = input.Name;
            

            _dbContext.Users.Update(user);

            await _dbContext.SaveChangesAsync();

            return user;
        }

        [HttpDelete(Name = "DeleteUsers")]
        public async Task<string> Delete(Guid UserId)
        {
            var user = await _dbContext.Users.
                             Where(Q => Q.Id == UserId).
                             FirstOrDefaultAsync();
            if (user == null)
            {
                throw new Exception("event not found");
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return "User that has this " + UserId +" has been deleted";
        }
    }
}

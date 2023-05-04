using Microsoft.EntityFrameworkCore;

namespace WebCalendarAPI.Models
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Groups> Groups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) 
            => options.UseSqlite(@"Data Source=D:\Code Playground\WebCalendarAPI\Database\Calendar.db");


    }
}

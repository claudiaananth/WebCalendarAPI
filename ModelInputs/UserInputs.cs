using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebCalendarAPI.ModelInputs
{
    public class UserInputs
    {
        public Guid? UserId { get; set; }
        public string Name { get; set;}
        [EmailAddress] 
        public string Email { get; set;}
        public string Password { get; set;}
        public string ConfirmPassword { get; set;}
    }
}

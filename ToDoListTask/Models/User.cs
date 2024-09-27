using Microsoft.AspNetCore.Identity;
namespace ToDoListTask.Models
{
    public class User : IdentityUser<int>
    {
        public Guid UserId { get; set; }
        public string UserName {  get; set; }   
        public string Password { get; set; }
        public string Email { get; set; } 
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}

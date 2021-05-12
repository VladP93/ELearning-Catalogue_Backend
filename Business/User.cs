using Microsoft.AspNetCore.Identity;

namespace Business
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } 
    }
}
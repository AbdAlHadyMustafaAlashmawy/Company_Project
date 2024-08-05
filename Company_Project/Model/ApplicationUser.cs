using Microsoft.AspNetCore.Identity;

namespace Company_Project.Model
{
    public class ApplicationUser:IdentityUser
    {
        public string? Address { get; set; }
    }
}

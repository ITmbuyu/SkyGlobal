using Microsoft.AspNetCore.Identity;

namespace SkyGlobal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePictureUrl { get; set; } = "Unknown";


    }
}

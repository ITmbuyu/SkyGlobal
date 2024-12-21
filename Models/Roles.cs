using Microsoft.AspNetCore.Identity;

namespace SkyGlobal.Models
{
    public class Roles
    {
        public Roles() { }
        public Roles(IdentityRole role)
        {
            string Id = role.Id;
            string Name = role.Name;
        }
        public string RolesId { get; set; }
        public string RoleName { get; set; }
    }
}

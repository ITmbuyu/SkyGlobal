using Microsoft.AspNetCore.Identity;

namespace SkyGlobal.Models
{
    public class Roles
    {
        public Roles() { }

        public Roles(IdentityRole role)
        {
            RolesId = role.Id; // Assign to property instead of local variable
            RoleName = role.Name; // Assign to property instead of local variable
        }

        public string RolesId { get; set; }
        public string RoleName { get; set; }
    }

}

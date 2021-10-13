using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SecretProjectBack.Entity.User
{
    public class AppRole : IdentityRole<long>
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}

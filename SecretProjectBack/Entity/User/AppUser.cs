using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecretProjectBack.Entity.User
{
    public class AppUser : IdentityUser<long>
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        [StringLength(255)]
        public string Image { get; set; }
    }
}
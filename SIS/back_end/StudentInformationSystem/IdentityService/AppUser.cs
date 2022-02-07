using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityService
{
    public sealed class AppUser : IdentityUser
    {
        public AppUser(string email, string username)
        {
            Email = email;
            UserName = username;
            SecurityStamp = Guid.NewGuid().ToString();
        }

        public AppUser(string email)
            : this(email, email) { }
    }
}

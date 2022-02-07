using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService
{
    public class UserRole
    {
        // a class that functions like an enumerator but with string equivalents for its values
        private static readonly HashSet<UserRole> allRoles = new();
        public static IEnumerable<UserRole> AllRoles => allRoles.ToList();

        public static readonly UserRole ADMIN = new("Admin");
        public static readonly UserRole USER = new("User");
        public static readonly UserRole DEVELOPER = new("Developer");
        public static readonly UserRole OTHER = new("Other");

        // instance members
        private readonly string name;

        private UserRole(string name)
        {
            this.name = name;
            if(!allRoles.Add(this)) throw new ArgumentException("A role with that name is already present");
        }

        public override string ToString()
        {
            return name;
        }
    }
}

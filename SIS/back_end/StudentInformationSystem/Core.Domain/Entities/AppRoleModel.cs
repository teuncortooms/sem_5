using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class AppRoleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<AppClaimModel> RoleClaims { get; set; }
    }
}

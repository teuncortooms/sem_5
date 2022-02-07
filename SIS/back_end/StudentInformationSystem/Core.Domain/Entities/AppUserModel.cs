using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class AppUserModel
    {
        public string Id { get; set; }
        public string LoginName { get; set; }
        public string EMail { get; set; }
        public ICollection<AppClaimModel> Claims { get; set; }
        public ICollection<AppRoleModel> Roles { get; set; }
    }
}

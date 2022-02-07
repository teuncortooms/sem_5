using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Queries.ResponseModels
{
    public class ClaimModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
    
    public class UserDetailsModel
    {
        public string Username { get; set; }
        public ICollection<string> Roles { get; set; }
        public ICollection<ClaimModel> Claims { get; set; }
        

        public UserDetailsModel()
        {
            this.Roles = new List<string>();
            this.Claims = new List<ClaimModel>();
        }
    }
}

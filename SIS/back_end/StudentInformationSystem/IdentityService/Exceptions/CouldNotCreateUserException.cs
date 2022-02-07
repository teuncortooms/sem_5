using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Exceptions
{
    public class CouldNotCreateUserException : Exception
    {
        public CouldNotCreateUserException(string message)
            : base(message) { }
    }
}

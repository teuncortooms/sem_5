using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Commands.ResponseModels
{
    public enum RegistrationStatus
    {
        Success,
        Error
    }

    public class RegisterResponse
    {
        public RegistrationStatus StatusCode { get; set; }
        public string Status => StatusCode.ToString();
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public static RegisterResponse From(IdentityResult result, string message = null)
        {
            return result.Succeeded ? Success(message) : Error(result, message);
        }

        public static RegisterResponse Success(string message = "User created successfully!")
        {
            return new RegisterResponse
            {
                StatusCode = RegistrationStatus.Success,
                Message = message
            };
        }
        
        public static RegisterResponse Error(IEnumerable<string> errors = null, string message = "User creation failed! Please check user details and try again.")
        {
            return new RegisterResponse
            {
                StatusCode = RegistrationStatus.Error,
                Message = message,
                Errors = errors
            };
        }

        public static RegisterResponse Error(IdentityResult result, string message = null)
        {
            var errors = result.Errors.Select(e => e.Description);

            return Error(errors, message);
        }

        public static RegisterResponse Error(string message = null)
        {
            return Error(new List<string> { message }, message);
        }
    }
}

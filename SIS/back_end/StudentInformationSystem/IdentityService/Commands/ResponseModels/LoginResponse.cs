using System;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityService.Commands.ResponseModels
{
    public enum LoginStatus
    {
        Success,
        Unauthorized
    }

    public class LoginResponse
    {
        public LoginStatus StatusCode { get; set; }
        public string Status => StatusCode.ToString();
        public string Message;
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public static LoginResponse Unauthorized(string message = null)
        {
            return new LoginResponse
            {
                StatusCode = LoginStatus.Unauthorized,
                Message = message
            };
        }

        public static LoginResponse Success(JwtSecurityToken token)
        {
            return new LoginResponse()
            {
                StatusCode = LoginStatus.Success,
                Message = "Successful login",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }
    }
}

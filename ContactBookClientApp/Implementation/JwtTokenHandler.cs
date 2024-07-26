using ContactBookClientApp.Infrastructure;
using System.IdentityModel.Tokens.Jwt;

namespace ContactBookClientApp.Implementation
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly JwtSecurityTokenHandler _handler;

        public JwtTokenHandler()
        {
            _handler = new JwtSecurityTokenHandler();
        }

        public JwtSecurityToken ReadJwtToken(string token)
        {
            return _handler.ReadJwtToken(token);
        }
    }
}

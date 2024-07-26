using System.IdentityModel.Tokens.Jwt;

namespace ContactBookClientApp.Infrastructure
{
    public interface IJwtTokenHandler
    {
        JwtSecurityToken ReadJwtToken(string token);
    }
}

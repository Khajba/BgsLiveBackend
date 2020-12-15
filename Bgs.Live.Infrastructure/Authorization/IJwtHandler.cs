using System.Collections.Generic;
using System.Security.Claims;

namespace Bgs.Infrastructure.Api.Authorization
{
    public interface IJwtHandler
    {
        JsonWebToken CreateToken(int userId, string role = null, IDictionary<string, string> claims = null);

        JsonWebToken RefreshToken(IEnumerable<Claim> claims);
    }
}
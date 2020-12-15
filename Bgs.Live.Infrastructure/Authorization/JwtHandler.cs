using Bgs.Utility.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Bgs.Infrastructure.Api.Authorization
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtOptions _options;
        private readonly SigningCredentials _signingCredentials;

        public JwtHandler(JwtOptions options)
        {
            _options = options;

            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            _signingCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);
        }

        public JsonWebToken CreateToken(int userId, string role = null, IDictionary<string, string> claims = null)
        {
            if (userId == default)
                throw new ArgumentException("User id claim can not be empty.", nameof(userId));

            var now = DateTime.UtcNow;
            var jwtClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString()),
            };

            if (!string.IsNullOrWhiteSpace(role))
            {
                jwtClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var customClaims = claims?.Select(claim => new Claim(claim.Key, claim.Value)).ToArray()
                               ?? Array.Empty<Claim>();
            jwtClaims.AddRange(customClaims);

            return GetTokenFromClaims(jwtClaims);
        }

        public JsonWebToken RefreshToken(IEnumerable<Claim> claims) =>
            GetTokenFromClaims(claims);

        private JsonWebToken GetTokenFromClaims(IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;

            var expires = now.AddMinutes(_options.ExpiryMinutes);
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: _signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JsonWebToken
            {
                AccessToken = token,
                ExpiresInMinutes = _options.ExpiryMinutes
            };
        }
    }
}
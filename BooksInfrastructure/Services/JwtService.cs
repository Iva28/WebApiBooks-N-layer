using BooksAppCore;
using BooksAppCore.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BooksInfrastructure.Services
{
    public class JwtService : IJwtService
    {
        private AuthOptions authOptions;

        public JwtService(IOptions<AuthOptions> options)
        {
            this.authOptions = options.Value;
        }

        public string GetJwt(ClaimsIdentity identity)
        {
            var now = DateTime.Now;
            JwtSecurityToken token = new JwtSecurityToken(
                    issuer: authOptions.Issuer,
                    audience: authOptions.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.AddMinutes(authOptions.AccessLifetime),
                    signingCredentials: new SigningCredentials(authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedJwt;
        }
    }
}

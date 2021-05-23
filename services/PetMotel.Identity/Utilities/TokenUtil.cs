using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetMotel.Common;

namespace PetMotel.Identity
{
    public static class TokenUtil
    {
        //https://dejanstojanovic.net/aspnet/2018/june/token-based-authentication-in-aspnet-core-part-2/
        public static String GetToken(IdentityUser user, IEnumerable<string> roles, IConfiguration configuration)
        {
            var utcNow = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, utcNow.ToString()),
            };
            foreach(var role in roles) 
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            
            byte[] key = Convert.FromBase64String(configuration.GetValue<String>(Constants.Config.TokenSigningKey));
            var signingKey = new SymmetricSecurityKey(key);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(configuration.GetValue<int>(Constants.Config.TokenLifetime)),
                audience: configuration.GetValue<String>(Constants.Config.TokenAudience),
                issuer: configuration.GetValue<String>(Constants.Config.TokenIssuer)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
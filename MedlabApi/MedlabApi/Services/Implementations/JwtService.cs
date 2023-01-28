using Medlab.Core.Entities;
using MedlabApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedlabApi.Services.Implementations
{
    public class JwtService : IJwtService
    {
        public string GenerateJwtToken(AppUser user, IList<string> roles, IConfiguration config)
        {
            List<Claim> Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
            Claims.AddRange(roleClaims);


            var symmetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config.GetSection("JWT:secret").Value));
            var creds = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
            claims: Claims,
            signingCredentials: creds,
                expires: DateTime.UtcNow.AddHours(8),
                issuer: config.GetSection("JWT:issuer").Value,
                audience: config.GetSection("JWT:audience").Value
                );
            string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenStr;



        }
    }
}

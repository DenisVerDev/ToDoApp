using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using ToDoAPI.Data.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using Task = System.Threading.Tasks.Task;

namespace ToDoAPI.Services.Authentication
{
    public class JwtAuthentication (IConfiguration _conf, UserManager<User> _um) : IAuthentication
    {
        public async Task<ServiceResult<string>> CreateValidation(string email)
        {
            var user = await _um.FindByEmailAsync(email);

            if (user is null)
                return new ServiceResult<string>(null, ServiceResultStatus.AbsentUser);
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.EmailVerified, user.EmailConfirmed.ToString())
            };

            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["JwtBearer:SecurityKey"]!));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_conf["JwtBearer:ExpirationTimeMinutes"])),
                SigningCredentials = signingCredentials,
                Issuer = _conf["JwtBearer:Issuer"],
                Audience = _conf["JwtBearer:Audience"]
            };

            var tokenHandler = new JsonWebTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new ServiceResult<string>(token, ServiceResultStatus.Success);
        }
    }
}

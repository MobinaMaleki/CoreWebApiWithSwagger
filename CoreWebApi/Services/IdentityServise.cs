using CoreWebApi.Domain;
using CoreWebApi.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CoreWebApi.Services
{
    public class IdentityServise : IIdentityServices
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public IdentityServise(UserManager<IdentityUser> userManager,JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }
        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existinguser = await _userManager.FindByEmailAsync(email);

            if (existinguser!=null)
            {
                return new AuthenticationResult() { Errors = new[] { "this user is exist" } };               
            }
            var newuser = new IdentityUser
            {
                Email = email,
                UserName=email
            };

            var createduser = await _userManager.CreateAsync(newuser, password);

            if (!createduser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createduser.Errors.Select(x => x.Description)
                };
              
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(type: JwtRegisteredClaimNames.Sub, value: newuser.Email),
                    new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                    new Claim(type: JwtRegisteredClaimNames.Email, value: newuser.Email),
                    new Claim(type: "id", value: newuser.Id),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}

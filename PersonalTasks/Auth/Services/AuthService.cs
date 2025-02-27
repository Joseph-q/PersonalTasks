using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PersonalTasks.Models;
using PointOfSale.Shared.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalTasks.Auth.Services
{
    public class AuthService(ContextDb context, IOptions<JwtSettings> jwtSeetings) : IAuthService
    {
        private readonly ContextDb _context = context;
        private readonly JwtSettings _jwtSettings = jwtSeetings.Value;
        public string GenerateJWT(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public Task<User?> GetUserWithPasswordByUsername(string username)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public Task RegisterUser(User user)
        {
            _context.Add(user);
            return _context.SaveChangesAsync();

        }
    }
}

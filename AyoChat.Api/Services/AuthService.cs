using AyoChat.Api.Data;
using AyoChat.Api.Entities;
using AyoChat.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AyoChat.Api.Services
{
    public class AuthService(ChatDbContext context,IConfiguration configuration) : IAuthService
    {
        public async Task<LoginResponseDto?> LoginAsync(UserLoginRequestDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (user is null)
            {
                return null;
            }

            if(new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            LoginResponseDto response = await CreateLoginResponse(user);
            return response;
        }

        private async Task<LoginResponseDto> CreateLoginResponse(User user)
        {
            return new LoginResponseDto
            {
                AccessToken = CreateJWTToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
            
        }

        public async Task<LoginResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null)
            {
                return null;
            }

            return await CreateLoginResponse(user);
        }

        public async Task<User?> RegisterAsync(UserDto request)
        {
            if(await context.Users.AnyAsync(u=>u.PhoneNumber == request.PhoneNumber))
            {
                return null;
            }

            var user = new User();

            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.Username = request.Username;
            user.PhoneNumber = request.PhoneNumber;
            user.PasswordHash = hashedPassword;
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        private async Task<User?> ValidateRefreshTokenAsync(Guid uid,string refreshToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == uid);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();
            return refreshToken;
        }

        private string CreateJWTToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken(
                    issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                    audience: configuration.GetValue<string>("AppSettings:Audience"),
                    claims: claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<User?> GetLoggedUser(Guid userId)
        {
            return await context.Users.FindAsync(userId);
        }
    }
}

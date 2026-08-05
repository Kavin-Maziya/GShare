using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GearShare.Api.Data;
using GearShare.Api.DTOs;
using GearShare.Api.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace GearShare.Api.Services;

public class AuthService(IConfiguration configuration) : IAuthService
{
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        await Task.CompletedTask;

        // Look up user from in-memory store
        var user = InMemoryStore.Users.FirstOrDefault(u =>
            u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase));

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedActionException("Invalid email or password.");

        // These claims identify the caller and support later ownership/role checks.
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(configuration["Jwt:ExpiryMinutes"]!)),
            signingCredentials: new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256));

        return new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Email = user.Email,
            Name = user.Name,
            Role = user.Role.ToString()
        };
    }
}

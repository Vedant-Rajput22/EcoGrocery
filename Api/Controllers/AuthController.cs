using Application.Features.Auth.Dtos;
using Domain.Entities;
using Domain.Enums;                            
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _users;
    private readonly RoleManager<IdentityRole<Guid>> _roles;   
    private readonly IConfiguration _cfg;

    public AuthController(UserManager<AppUser> users,
                          RoleManager<IdentityRole<Guid>> roles, 
                          IConfiguration cfg)
    {
        _users = users;
        _roles = roles;       
        _cfg = cfg;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        
        await EnsureRoleExistsAsync(BuiltInRoles.Customer);

        var user = new AppUser(dto.Email, dto.FirstName, dto.LastName);
        var result = await _users.CreateAsync(user, dto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        await _users.AddToRoleAsync(user, BuiltInRoles.Customer);

        var token = await GenerateJwtAsync(user);
        return Ok(new { userId = user.Id, token });
    }

   
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _users.FindByEmailAsync(dto.Email);
        if (user is null || !await _users.CheckPasswordAsync(user, dto.Password))
            return Unauthorized();

        var token = await GenerateJwtAsync(user);
        return Ok(new { userId = user.Id, token });
    }

   
    private async Task EnsureRoleExistsAsync(string role)
    {
        if (!await _roles.RoleExistsAsync(role))
            await _roles.CreateAsync(new IdentityRole<Guid>(role));
    }

    private async Task<string> GenerateJwtAsync(AppUser user)
    {
        var jwt = _cfg.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email,          user.Email ?? "")
        };

        var roles = await _users.GetRolesAsync(user);
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(jwt["Issuer"], jwt["Audience"],
                                         claims,
                                         expires: DateTime.UtcNow.AddHours(3),
                                         signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

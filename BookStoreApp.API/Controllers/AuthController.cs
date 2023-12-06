using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using BookStoreApp.API.@static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;

namespace BookStoreApp.API.Controllers;

[ApiController, Route("api/[controller]"), AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthController(ILogger<AuthController> logger,
        IConfiguration configuration,
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _configuration = configuration;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto dto)
    {
        if (!TryValidateModel(dto))
        {
            _logger.LogWarning("USer provided bad account creation data {dto}", dto);
            return BadRequest("Invalid data provided");
        }
        //var user = new ApplicationUser { FirstName = dto.FirstName, LastName = dto.LastName, UserName = dto.Email, Email = dto.Email};
        var user = _mapper.Map<ApplicationUser>(dto);
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (result.Succeeded)
        {
            // Code for successful registration

        }

        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            _logger.LogError("Failed to add user for dto {dto}", dto);
            // Code for failed registration
            return BadRequest(ModelState);
        }
        if (string.Equals(dto.Role, "user", StringComparison.CurrentCultureIgnoreCase))
        {
            var roleExists = await _roleManager.RoleExistsAsync("User");
            if (!roleExists)
            {
                _logger.LogError("Failed to find the user role to add the user to");

            }
            var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!addToRoleResult.Succeeded)
            {
                _logger.LogError("Failed to add user ot the User role for dto {dto}", dto);
                // Code for failed role assignment
                return BadRequest(addToRoleResult.Errors);
            }
        }
        else
        {
            var roleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                _logger.LogError("Failed to find the Admin role to add the user to");

            }
            var addToRoleResult = await _userManager.AddToRoleAsync(user, "Admin");
            if (!addToRoleResult.Succeeded)
            {
                _logger.LogError("Failed to add user to the Admin role for dto {dto}", dto);
                // Code for failed role assignment
                return BadRequest(addToRoleResult.Errors);
            }
        }

        return Accepted();
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(UserLoginDto dto)
    {
        _logger.LogInformation($"Login attempt for {dto.UserName}");
        try
        {
            var user = await _userManager.FindByNameAsync(dto.UserName!);
            var passwordIsValid = user != null && await _userManager.CheckPasswordAsync(user, dto.Password);
            if (user == null || !passwordIsValid)
            {
                return Unauthorized(dto);
            }

            var tokenString = await GenerateWebToken(user);

            var response = new AuthResponse()
            {
                Email = user.Email!,
                Token = tokenString,
                UserId = user.Id
            };

            return Accepted(response);

        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Something went wrong trying to Log in.");
            return Problem("Something went wrong trying to Log in.");
        }
    }

    private async Task<string> GenerateWebToken(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserName!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(CustomClaimTypes.Uid, user.Id)
            }.Union(roles.Select(role => new Claim(ClaimTypes.Role, role)))
            .Union(await _userManager.GetClaimsAsync(user));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: _configuration["JwtSettings:Issuer"], audience: _configuration["JwtSettings:Audience"],
            claims: claims, expires: DateTime.Now.AddDays(7), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
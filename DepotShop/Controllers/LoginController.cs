using DepotShopDataAccess.Repository;
using DepotShopModels.DTOs;
using DepotShopModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace DepotShop.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IUserRepository userRepository;
    public IConfiguration Configuration { get; }
    public LoginController(IUserRepository userRepository, IConfiguration configuration)
    {
        this.userRepository = userRepository;
        Configuration = configuration;
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Index(LoginDTO loginDetails)
    {
        //check Admin
        var AdminEmail = Configuration.GetValue<string>("Admin:Email");
        var AdminPassword = Configuration.GetValue<string>("Admin:Password");
        if (AdminEmail == loginDetails.Email && AdminPassword == loginDetails.Password)
        {
            return Ok(GetToken(null));
        }
        //check User
        var user = await userRepository.GetOne(user => user.Email == loginDetails.Email);
        if (user == null) return Challenge();
        var HashingResult = BC.Verify(loginDetails.Password, user.Password);
        if (!HashingResult) return Challenge();
        var token = GetToken(user);
        return Ok(token);
    }
    private string GetToken(UserModel? user)
    {
        var SecretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
            Configuration.GetValue<string>("Authentication:SecretKey")!
            ));
        var SigningCredenials = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);
        var Claims = new List<Claim>();
        if (user != null)
        {
            Claims.Add(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user._id));
            Claims.Add(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName, user.Email));
            Claims.Add(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user._id));
        }
        else
        {
            Claims.Add(new Claim("Role", "Admin"));
        }
        var Token = new JwtSecurityToken(
            Configuration.GetValue<string>("Authentication:Issuer"),
            Configuration.GetValue<string>("Authentication:Audiance"),
            Claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            SigningCredenials
            );
        return new JwtSecurityTokenHandler().WriteToken(Token);
    }
}

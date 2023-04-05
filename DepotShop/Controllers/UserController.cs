using AutoMapper;
using DepotShopDataAccess.Repository;
using DepotShopModels.DTOs.User;
using DepotShopModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DepotShop.Controllers;
using BC = BCrypt.Net.BCrypt;



[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    public IUserRepository UsersCollection { get; }
    public IMapper Mapper { get; }
    public UserController(IUserRepository userRepository, IMapper mapper)
    {
        UsersCollection = userRepository;
        Mapper = mapper;
    }

    [HttpGet]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Get()
    {
        return Ok(await UsersCollection.GetAll());
    }

    [HttpGet("{id:length(24)}", Name = "GetUser")]
    public async Task<IActionResult> Get(string id)
    {
        var user = await UsersCollection.GetOne(user => user._id == id);
        return (user == null) ? NotFound() : Ok(user);
    }

    [HttpPost("SignUp", Name = "CreateUser")]
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromBody] createUserDTO user)
    {
        if (ModelState.IsValid)
        {
            var newUser = Mapper.Map<UserModel>(user);
            newUser.Password = BC.HashPassword(newUser.Password, 10);
            var addedUser = await UsersCollection.Add(newUser);
            return CreatedAtRoute("CreateUser", addedUser);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPut("{id:length(24)}")]
    [Authorize(Policy = "HasID")]
    public async Task<IActionResult> Put([FromRoute] string id, [FromBody] EditUserDTO user)
    {
        var AuthUserId = User.FindFirst(JwtRegisteredClaimNames.Sub);
        if (AuthUserId == null || AuthUserId.Value != id) return Challenge();
        var EditedUser = Mapper.Map<UserModel>(user);
        EditedUser._id = id;
        if (EditedUser.Password != null)
            EditedUser.Password = BC.HashPassword(EditedUser.Password, 12);
        var result = await UsersCollection.Update(EditedUser);
        return (result.ModifiedCount > 0) ? Ok() : NotFound();
    }

    [HttpDelete("{id:length(24)}")]
    [Authorize(Policy = "HasID")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await UsersCollection.Delete(x => x._id == id);
        return (result.DeletedCount > 0) ? Ok() : NotFound();
    }
}

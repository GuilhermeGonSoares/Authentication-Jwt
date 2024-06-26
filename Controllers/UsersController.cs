using Authentication_jwt.Entities;
using Authentication_jwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_jwt.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController(UserService userService) : ControllerBase
{
    private readonly UserService _userService = userService;

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _userService.GetUsers());
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _userService.Login(request.Username, request.Password);
        if (string.IsNullOrEmpty(response))
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        await _userService.CreateUser(request.Username, request.Password, Array.Empty<Roles>());
        return Ok();
    }
}

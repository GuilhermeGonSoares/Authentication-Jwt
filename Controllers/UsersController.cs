using Authentication_jwt.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication_jwt.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {   
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }
}

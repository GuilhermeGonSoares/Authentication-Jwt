using Authentication_jwt.Database;
using Authentication_jwt.Entities;
using Authentication_jwt.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Authentication_jwt.Services;

public class UserService(
    ApplicationDbContext context,
    IJwtTokenGenerator jwtService,
    IEncryptService encryptService
)
{
    private readonly ApplicationDbContext _context = context;
    private readonly IJwtTokenGenerator _jwtService = jwtService;
    private readonly IEncryptService _encryptService = encryptService;

    public async Task CreateUser(string username, string password, ICollection<Roles>? roles)
    {
        var user = new User(username, _encryptService.HashPassword(password), roles);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context
            .Users.Select(x => new User(x.Username, string.Empty, x.Roles))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<string> Login(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        if (user == null || !_encryptService.VerifyPassword(password, user.Password))
        {
            return string.Empty;
        }

        return _jwtService.GenerateToken(user);
    }
}

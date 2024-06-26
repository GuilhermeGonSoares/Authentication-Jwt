using Authentication_jwt.Entities;

namespace Authentication_jwt.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}

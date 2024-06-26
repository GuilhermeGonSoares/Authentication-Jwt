using Authentication_jwt.Interfaces;

namespace Authentication_jwt.Services;

public class EncryptService : IEncryptService
{
    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassword(string password, string passwordHash) =>
        BCrypt.Net.BCrypt.Verify(password, passwordHash);
}

namespace Authentication_jwt.Interfaces;

public interface IEncryptService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}

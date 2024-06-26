namespace Authentication_jwt.Entities;

public sealed class User(string username, string password, ICollection<Roles>? roles)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;
    public ICollection<Roles> Roles { get; set; } = roles ?? [];
}

using Isopoh.Cryptography.Argon2;

namespace IeltsPlatform.Infrastructure.Services;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string providedPassword);
}

public class Argon2PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return Argon2.Hash(password);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        return Argon2.Verify(hashedPassword, providedPassword);
    }
}

namespace MyReptileFamilyLibrary.Generators;

public static class TokenGenerator
{
    public static string GenerateHashedToken()
    {
        return BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString());
    }
}
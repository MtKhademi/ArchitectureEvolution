namespace Step02.Common;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        return password;
    }

    public static bool Verify(string password, string hashPassword)
    {
        return password == hashPassword;
    }
}

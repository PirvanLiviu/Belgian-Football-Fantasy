namespace Api.Helpers;

public static class AuthValidator
{
    public static (bool IsValid, string? Error) Validate(string password, string formation)
    {
        if (password.Length < 6)
            return (false, "Password must be at least 6 characters");

        if (!password.Any(char.IsUpper))
            return (false, "Password must contain at least one capital letter");

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            return (false, "Password must contain at least one symbol");

        if (formation.Length != 5)
            return (false, "Invalid formation format");
        if (formation.Split("-").Length != 3)
            return (false, "Invalid formation format");

        return (true, null);
    }
}

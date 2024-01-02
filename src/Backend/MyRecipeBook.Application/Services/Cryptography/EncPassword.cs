using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Application.Services.Cryptography;

public class EncPassword
{
    private readonly string _passAdditionalKey;

    public EncPassword(string passAdditionalKey)
    {
        _passAdditionalKey = passAdditionalKey;
    }

    public string Encrypt(string password)
    {
        var passwordWithAddtionalKey = $"{password}{_passAdditionalKey}";

        var bytes = Encoding.UTF8.GetBytes(passwordWithAddtionalKey);
        byte[] hasBytes = SHA512.HashData(bytes);
        return StringBytes(hasBytes);
    }

    private static string StringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }

        return sb.ToString();
    }
}

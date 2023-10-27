using System.Security.Cryptography;
using System.Text;

namespace Sarf.Extensions;

public static class StringExtensions
{
    public static string GetSha512(this string str)
    {
        using var sha = SHA512.Create();
        return Encoding.UTF8.GetString(sha.ComputeHash(Encoding.UTF8.GetBytes(str)));
    }
}
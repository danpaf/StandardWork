using System.Net.Mail;

namespace Sarf.Utils;

public static class Utils
{
    public static MailAddress? ParseEmail(string emailAddress)
    {
        try
        { 
            return new MailAddress(emailAddress);
        }
        catch (FormatException)
        {
            return null;
        }
    }
}
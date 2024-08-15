using System.Text;

namespace NetForemost.SharedKernel.Helpers;

public static class Base64Helper
{
    /// <summary>
    ///     Encoding string to base64.
    /// </summary>
    /// <param name="plainText">Text to encoding.</param>
    /// <returns>String in base64 text</returns>
    public static string Encode(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    /// <summary>
    ///     Decoding base64 to string
    /// </summary>
    /// <param name="base64Text">Base64 text to decoding.</param>
    /// <returns>String in plain text</returns>
    public static string Decode(string base64Text)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64Text);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

    /// <summary>
    ///     Check if it is a string in base64.
    /// </summary>
    /// <param name="base64Text">Base64 text to verify.</param>
    /// <returns>Returns if true or false</returns>
    public static bool IsBase64String(string base64Text)
    {
        try
        {
            Convert.FromBase64String(base64Text);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
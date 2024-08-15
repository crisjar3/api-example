using Ardalis.Result;
using System.Text.Json;

namespace NetForemost.SharedKernel.Helpers;

public static class StringHelper
{
    private const string _numbers = "0123456789";
    private const string _specialCharacters = "!@#$%&*+-><,.;][{}";
    private const string _uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string _lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    ///     Generate a random password with numbers, letters and special characters.
    /// </summary>
    /// <returns>Returns a random password.</returns>
    public static string GenerateRandomStringPassword()
    {
        Random random = new();
        var generatePassword = "";

        var generatePasswordArray = (GenerateRandomString(2, _numbers) + GenerateRandomString(2, _specialCharacters) +
                                     GenerateRandomString(2, _uppercaseLetters) +
                                     GenerateRandomString(2, _lowercaseLetters))
            .ToCharArray().OrderBy(x => random.Next()).ToArray();

        foreach (var c in generatePasswordArray)
            generatePassword += c;

        return generatePassword;
    }

    /// <summary>
    ///     Generate a random string with a given size.
    /// </summary>
    /// <param name="length">Random string size.</param>
    /// <param name="characters">Available characters.</param>
    /// <returns></returns>
    public static string GenerateRandomString(int length, string characters)
    {
        Random random = new();
        return new string(Enumerable.Repeat(characters, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    /// <summary>
    ///     Convert an object to a json string
    /// </summary>
    /// <param name="objectToConvert">The object to convert</param>
    /// <returns></returns>
    public static string ObjectToString(object objectToConvert)
    {
        return JsonSerializer.Serialize(objectToConvert).Replace("{", "[").Replace("}", "]");
    }

    /// <summary>
    /// Convert string to int array
    /// </summary>
    /// <param name="value">The string value with numbers separate by ","</param>
    /// <returns>Int array</returns>
    public static Result<List<int>> ConvertStringToIntList(string value)
    {
        try
        {
            if (!string.IsNullOrEmpty(value))
            {
                return Result<List<int>>.Success(value.Split(",").Select(int.Parse).ToArray().ToList());
            }
            else
            {
                return Result<List<int>>.Success(new List<int>());
            }
        }
        catch (Exception)
        {
            return Result<List<int>>.Success(new List<int>());
        }
    }

    /// <summary>
    /// Convert string to string array
    /// </summary>
    /// <param name="value">The string value with values separate by ","</param>
    /// <returns>Int array</returns>
    public static Result<List<string>> ConvertStringToStringList(string value)
    {
        try
        {
            if (!string.IsNullOrEmpty(value))
            {
                return Result<List<string>>.Success(value.Split(",").ToList());
            }
            else
            {
                return Result<List<string>>.Success(new List<string>());
            }
        }
        catch (Exception)
        {
            return Result<List<string>>.Success(new List<string>());
        }
    }
}
using System.Text;

namespace NetForemost.Infrastructure.Extensions;

public static class RepositoryExtension
{
    public static string AddUndercoresToSentence(this string text, bool preserveAcronyms = false)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;
        var newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (var i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]))
                if (text[i - 1] != '_' && !char.IsUpper(text[i - 1]) ||
                    preserveAcronyms && char.IsUpper(text[i - 1]) &&
                     i < text.Length - 1 && !char.IsUpper(text[i + 1]))
                    newText.Append('_');
            newText.Append(text[i]);
        }

        return newText.ToString().ToLower();
    }
}


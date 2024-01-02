using System.Globalization;
using System.Text;

namespace MyRecipeBook.Domain.Extension;

public static class StringExtension
{
    public static bool CompareWithoutAccentsAndUpperCase(this string origin, string searchBy)
    {
        var index = CultureInfo.CurrentCulture.CompareInfo.IndexOf(origin, searchBy, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);

        return index >= 0;
    }

    public static string RemoveAccents(this string text)
    {
        return new string(text.Normalize(NormalizationForm.FormD).Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark).ToArray()) ;
    }
}

using System.Globalization;

namespace ErpEssentials.SharedKernel.Extensions;

public static class StringExtensions
{
    public static string ToTitleCaseStandard(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var trimmed = input.Trim().ToLower();
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(trimmed);
    }
}
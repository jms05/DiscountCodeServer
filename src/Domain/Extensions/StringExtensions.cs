using System.Text.RegularExpressions;

namespace System;
public static class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        return Regex.Match(input, "^_+")?.ToString() + Regex.Replace(input, "([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}
namespace Gnome.Platforms;

internal static class StringExtensions
{
    public static string AsString(this ReadOnlySpan<char> span)
    {
#if NETLEGACY
        return new string(span.ToArray());
#else
        return span.ToString();
#endif
    }

    public static string AsString(this Span<char> span)
    {
#if NETLEGACY
        return new string(span.ToArray());
#else
        return span.ToString();
#endif
    }

#if NETLEGACY
    /// <summary>
    /// Splits a <see cref="string"/> into substrings using the separator.
    /// </summary>
    /// <param name="source">The string instance to split.</param>
    /// <param name="separator">The separator that is used to split the string.</param>
    /// <returns>The <see cref="T:string[]"/>.</returns>
    public static string[] Split(this string source, string separator)
    {
        return source.Split(separator.ToCharArray());
    }

    /// <summary>
    /// Splits a <see cref="string"/> into substrings using the separator.
    /// </summary>
    /// <param name="source">The string instance to split.</param>
    /// <param name="separator">The separator that is used to split the string.</param>
    /// <param name="options">The string split options.</param>
    /// <returns>The <see cref="T:string[]"/>.</returns>
    public static string[] Split(this string source, char separator, StringSplitOptions options)
    {
        return source.Split(new[] { separator }, options);
    }

    /// <summary>
    /// Splits a <see cref="string"/> into substrings using the separator.
    /// </summary>
    /// <param name="source">The string instance to split.</param>
    /// <param name="separator">The separator that is used to split the string.</param>
    /// <param name="options">The string split options.</param>
    /// <returns>The <see cref="T:string[]"/>.</returns>
    public static string[] Split(this string source, string separator, StringSplitOptions options)
    {
        return source.Split(separator.ToCharArray(), options);
    }
#endif
}
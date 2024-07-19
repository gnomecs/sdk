using System.Runtime.CompilerServices;

namespace Gnome.Sys.Memory;

#if GNOME_EXPOSE
public
#else
internal
#endif
static class SpanExtensions
{
    /// <summary>
    /// Converts the span of characters to a <see cref="string"/> for
    /// all targeted .net frameworks.
    /// </summary>
    /// <param name="source">The source span.</param>
    /// <returns>A new string from the span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string AsString(this ReadOnlySpan<char> source)
    {
#if NETLEGACY
        return new string(source.ToArray());
#else
        return source.ToString();
#endif
    }
}
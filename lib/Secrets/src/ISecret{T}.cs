#if !NETLEGACY
using System.Buffers;
#endif

namespace Gnome.Secrets;

public interface ISecret<T> : IDisposable
    where T : unmanaged
{
    int Length { get; }

    T[] Reveal();

    int RevealInto(Span<T> destination);

#if !NETLEGACY
    void Inspect<TState>(TState state, ReadOnlySpanAction<T, TState> action);
#endif
}
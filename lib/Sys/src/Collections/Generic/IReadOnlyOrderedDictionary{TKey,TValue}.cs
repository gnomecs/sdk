#if GNOME_EXPOSE
namespace Gnome.Collections.Generic;
#else
namespace Gnome.Collections.Generic.Internal;
#endif

#if GNOME_EXPOSE
public
#else
internal
#endif
interface IReadOnlyOrderedDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
{
}
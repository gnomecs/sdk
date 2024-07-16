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
interface IOrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    void Insert(int index, TKey key, TValue value);
}
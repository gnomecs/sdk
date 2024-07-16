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
class StringList : List<string>
{
    public StringList()
    {
    }

    public StringList(IEnumerable<string> collection)
        : base(collection)
    {
    }

    public StringList(int capacity)
        : base(capacity)
    {
    }

    public static implicit operator StringList(string value)
    {
        return [value];
    }

    public static implicit operator string[](StringList value)
    {
        return value.ToArray();
    }

    public static implicit operator StringList(string[] value)
    {
        return new StringList(value);
    }

    public bool Contains(string value, StringComparison comparisonType)
    {
        foreach (var n in this)
        {
            if (n.Equals(value, comparisonType))
                return true;
        }

        return false;
    }

    public int IndexOf(string value, StringComparison comparisonType)
    {
        for (var i = 0; i < this.Count; i++)
        {
            if (this[i].Equals(value, comparisonType))
                return i;
        }

        return -1;
    }
}
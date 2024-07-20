using Gnome.Collections.Generic;

using static Xunit.Assert;

namespace Tests;

public static class OrderedDictionary_Tests
{
    [UnitTest]
    public static void Ctor()
    {
        var dict = new OrderedDictionary<string, object?>();
        Empty(dict);
    }

    [UnitTest]
    public static void CtorWithComparer()
    {
        var dict = new OrderedDictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        Empty(dict);
    }

    [UnitTest]
    public static void CtorWithValues()
    {
        var dict = new OrderedDictionary<string, object>(new[] { new KeyValuePair<string, object>("a", 1), new KeyValuePair<string, object>("b", 2) });
        Equal(2, dict.Count);
        Equal(1, dict["a"]);
        Equal(2, dict["b"]);

        var keys = dict.Keys.ToList();
        Equal(2, keys.Count);
        Equal("a", keys[0]);
        Equal("b", keys[1]);
    }

    [UnitTest]
    public static void CtorWithValuesAndComparer()
    {
        var dict = new OrderedDictionary<string, object>(
            new[]
            {
                new KeyValuePair<string, object>("a", 1),
                new KeyValuePair<string, object>("b", 2),
            },
            StringComparer.OrdinalIgnoreCase);
        Equal(2, dict.Count);
        Equal(1, dict["A"]);
        Equal(2, dict["B"]);

        var keys = dict.Keys.ToList();
        Equal(2, keys.Count);
        Equal("a", keys[0]);
        Equal("b", keys[1]);
    }

    [UnitTest]
    public static void Indexers()
    {
        var dict = new OrderedDictionary<string, int>();
        dict.Add("Test", 0);
        dict.Add("Bob", 1);
        dict.Add("Alice", 2);

        Equal(0, dict["Test"]);
        Equal(0, dict[0]);
        Equal(1, dict["Bob"]);
        Equal(1, dict[1]);
        Equal(2, dict["Alice"]);
        Equal(2, dict[2]);

        dict["Test"] = 3;
        Equal(3, dict["Test"]);
        Equal(3, dict[0]);

        dict[0] = 4;
        Equal(4, dict["Test"]);
        Equal(4, dict[0]);
    }

    [UnitTest]
    public static void ContainsKey()
    {
        var dict = new OrderedDictionary<string, int>
        {
            { "Test", 0 },
            { "Bob", 1 },
            { "Alice", 2 },
        };

        True(dict.ContainsKey("Test"));
        True(dict.ContainsKey("Bob"));
        True(dict.ContainsKey("Alice"));
        False(dict.ContainsKey("NotHere"));
        False(dict.ContainsKey("NotThere"));
        False(dict.ContainsKey("Alic"));
    }

    [UnitTest]
    public static void ContainsValue()
    {
        var dict = new OrderedDictionary<string, int>();
        dict.Add("Test", 0);
        dict.Add("Bob", 1);
        dict.Add("Alice", 2);

        True(dict.ContainsValue(0));
        True(dict.ContainsValue(1));
        True(dict.ContainsValue(2));
        False(dict.ContainsValue(3));
        False(dict.ContainsValue(4));
    }

    [UnitTest]
    public static void Keys()
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        var keys = dict.Keys.ToArray();
        Assert.Collection(
            keys,
            o => Equal("Bob", o),
            o => Equal("Alice", o),
            o => Equal("Test", o));
    }

    [UnitTest]
    public static void Values()
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        var keys = dict.Values.ToArray();
        Collection(
            keys,
            o => Equal(1, o),
            o => Equal(2, o),
            o => Equal(0, o));
    }

    [UnitTest]
    public static void Add()
    {
        var dict = new OrderedDictionary<string, int>();
        dict.Add("Test", 0);
        dict.Add("Bob", 1);
        dict.Add("Alice", 2);

        Equal(3, dict.Count);
        Equal(0, dict["Test"]);
        Equal(1, dict["Bob"]);
        Equal(2, dict["Alice"]);
    }

    [UnitTest]
    public static void AddDuplicateKey()
    {
        var dict = new OrderedDictionary<string, int>();
        dict.Add("Test", 0);
        Throws<ArgumentException>(() => dict.Add("Test", 1));
    }

    [UnitTest]
    public static void Remove()
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        True(dict.Remove("Test"));
        False(dict.Remove("Test"));
        Equal(2, dict.Count);

        True(dict.Remove("Bob"));
        False(dict.Remove("Bob"));
        True(dict.Count == 1);
    }

    [UnitTest]
    public static void Clear()
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        dict.Clear();
        Empty(dict);

        dict.Add("Test7", 0);
        dict.Add("4", 1);

        Equal(2, dict.Count);
        Equal(0, dict["Test7"]);
        Equal(1, dict["4"]);
    }

    [UnitTest]
    public static void IndexOf()
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        Equal(0, dict.IndexOf("Bob"));
        Equal(1, dict.IndexOf("Alice"));
        Equal(2, dict.IndexOf("Test"));
    }

    [UnitTest]
    public static void RemoveAt()
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        dict.RemoveAt(1);
        Equal(2, dict.Count);

        Throws<ArgumentOutOfRangeException>(() => dict.RemoveAt(10));
    }

    [UnitTest]
    public static void Insert()
    {
        var dict = new OrderedDictionary<string, int>()
        {
            ["Bob"] = 1,
            ["Alice"] = 2,
            ["Test"] = 0,
        };

        dict.Insert(3, "Test2", 3);

        Equal(4, dict.Count);
        Equal(3, dict["Test2"]);
        Equal(3, dict.IndexOf("Test2"));

        dict.Insert(0, "Test3", 4);
        Equal(5, dict.Count);
        Equal(4, dict["Test3"]);
        Equal(0, dict.IndexOf("Test3"));
        Equal(1, dict.IndexOf("Bob"));
    }
}
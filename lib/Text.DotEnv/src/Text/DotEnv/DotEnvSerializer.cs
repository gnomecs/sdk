using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Text;

using Gnome.Sys;
using Gnome.Text.DotEnv.Document;
using Gnome.Text.DotEnv.Serialization;
using Gnome.Text.DotEnv.Tokens;

namespace Gnome.Text.DotEnv;

public static class DotEnvSerializer
{
    public static string SerializeDocument(EnvDocument document, DotEnvSerializerOptions? options = null)
    {
        var sb = new StringBuilder();
        using var sw = new StringWriter(sb);
        Serializer.SerializeDocument(document, sw, options);
        var envDocument = sb.ToString();
        sb.Clear();
        return envDocument;
    }

    public static void SerializeDocument(EnvDocument document, Stream stream, DotEnvSerializerOptions? options = null)
    {
        using var sw = new StreamWriter(stream, Encoding.UTF8, -1, true);
        Serializer.SerializeDocument(document, sw, options);
    }

    public static void SerializeDocument(EnvDocument document, TextWriter writer, DotEnvSerializerOptions? options = null)
        => Serializer.SerializeDocument(document, writer, options);

    public static string SerializeDictionary(IDictionary<string, string?> dictionary, DotEnvSerializerOptions? options = null)
    {
        var sb = new StringBuilder();
        using var sw = new StringWriter(sb);
        Serializer.SerializeDictionary(dictionary, sw, options);
        var envDocument = sb.ToString();
        sb.Clear();
        return envDocument;
    }

    public static string SerializeDictionary(IReadOnlyDictionary<string, string?> dictionary, DotEnvSerializerOptions? options = null)
    {
        var sb = new StringBuilder();
        using var sw = new StringWriter(sb);
        Serializer.SerializeDictionary(dictionary, sw, options);
        var envDocument = sb.ToString();
        sb.Clear();
        return envDocument;
    }

    public static void SerializeDictionary(IDictionary<string, string?> dictionary, Stream stream, DotEnvSerializerOptions? options = null)
    {
        using var sw = new StreamWriter(stream, Encoding.UTF8, -1, true);
        Serializer.SerializeDictionary(dictionary, sw, options);
    }

    public static void SerializeDictionary(IReadOnlyDictionary<string, string?> dictionary, Stream stream, DotEnvSerializerOptions? options = null)
    {
        using var sw = new StreamWriter(stream, Encoding.UTF8, -1, true);
        Serializer.SerializeDictionary(dictionary, sw, options);
    }

    public static void SerializeDictionary(IDictionary<string, string?> dictionary, TextWriter writer, DotEnvSerializerOptions? options = null)
        => Serializer.SerializeDictionary(dictionary, writer, options);

    public static void SerializeDictionary(IReadOnlyDictionary<string, string?> dictionary, TextWriter writer, DotEnvSerializerOptions? options = null)
        => Serializer.SerializeDictionary(dictionary, writer, options);

    public static T DeserializeDictionary<[Dam(Dat.PublicParameterlessConstructor)] T>(string value, DotEnvSerializerOptions? options = null)
        where T : IDictionary<string, string?>
    {
        var dictionary = Activator.CreateInstance<T>();
        return DeserializeDictionary(dictionary, value, options);
    }

    public static T DeserializeDictionary<[Dam(Dat.PublicParameterlessConstructor)] T>(TextReader reader, DotEnvSerializerOptions? options = null)
        where T : IDictionary<string, string?>
    {
        var dictionary = Activator.CreateInstance<T>();
        return DeserializeDictionary(dictionary, reader, options);
    }

    public static T DeserializeDictionary<[Dam(Dat.PublicParameterlessConstructor)] T>(Stream stream, DotEnvSerializerOptions? options = null)
        where T : IDictionary<string, string?>
    {
        var dictionary = Activator.CreateInstance<T>();
        return DeserializeDictionary(dictionary, stream, options);
    }

    public static T DeserializeDictionary<T>(T dictionary, TextReader reader, DotEnvSerializerOptions? options = null)
        where T : IDictionary<string, string?>
    {
        var doc = Serializer.DeserializeDocument(reader, options);
        foreach (var kvp in doc.AsNameValuePairEnumerator())
            dictionary[kvp.Name] = kvp.Value;

        return dictionary;
    }

    public static T DeserializeDictionary<T>(T dictionary, Stream stream, DotEnvSerializerOptions? options = null)
        where T : IDictionary<string, string?>
    {
        var doc = Serializer.DeserializeDocument(stream, options);
        foreach (var kvp in doc.AsNameValuePairEnumerator())
            dictionary[kvp.Name] = kvp.Value;

        return dictionary;
    }

    public static T DeserializeDictionary<T>(T dictionary, string value, DotEnvSerializerOptions? options = null)
        where T : IDictionary<string, string?>
    {
        var doc = Serializer.DeserializeDocument(value, options);
        foreach (var kvp in doc.AsNameValuePairEnumerator())
            dictionary[kvp.Name] = kvp.Value;

        return dictionary;
    }

    public static EnvDocument DeserializeDocument(string value, DotEnvSerializerOptions? options = null)
    {
        using var sr = new StringReader(value);
        return DeserializeDocument(sr, options);
    }

    public static EnvDocument DeserializeDocument(Stream stream, DotEnvSerializerOptions? options = null)
    {
        using var sr = new StreamReader(stream, Encoding.UTF8);
        return DeserializeDocument(sr, options);
    }

    public static EnvDocument DeserializeDocument(TextReader reader, DotEnvSerializerOptions? options = null)
    {
        options ??= new DotEnvSerializerOptions();
        var r = new DotEnvReader(reader, options);
        var doc = new EnvDocument();
        string? key = null;

        while (r.Read())
        {
            switch (r.Current)
            {
                case EnvCommentToken commentToken:
                    doc.Add(new EnvComment(commentToken.RawValue));
                    continue;

                case EnvNameToken nameToken:
                    key = nameToken.Value;
                    continue;

                case EnvScalarToken scalarToken:
                    if (key is not null && key.Length > 0)
                    {
                        if (doc.TryGetNameValuePair(key, out var entry) && entry is not null)
                        {
                            entry.RawValue = scalarToken.RawValue;
                            key = null;
                            continue;
                        }

                        doc.Add(key, scalarToken.RawValue);
                        key = null;
                        continue;
                    }

                    throw new InvalidOperationException("Scalar token found without a name token before it.");
            }
        }
#pragma warning disable S2589
        bool expand = options?.Expand == true;

        if (expand)
        {
            Func<string, string?> getVariable = Environment.GetEnvironmentVariable;
            if (options?.ExpandVariables is not null)
            {
                var ev = options.ExpandVariables;
                getVariable = (name) =>
                {
                    if (doc.TryGetValue(name, out var value))
                        return value;

                    if (ev.TryGetValue(name, out value))
                        return value;

                    value = Environment.GetEnvironmentVariable(name);

                    return value;
                };
            }

            var eso = new EnvExpandOptions()
            {
                UnixAssignment = false,
                UnixCustomErrorMessage = false,
                GetVariable = getVariable,
                SetVariable = Environment.SetEnvironmentVariable,
            };
            foreach (var entry in doc)
            {
                if (entry is EnvNameValuePair pair)
                {
                    var v = EnvVariablesExpander.Expand(pair.RawValue, eso);

                    // Only set the value if it has changed.
                    if (v.Length != pair.RawValue.Length || !v.SequenceEqual(pair.RawValue))
                        pair.SetRawValue(v);
                }
            }
        }

        return doc;
    }
}
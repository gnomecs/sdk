using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Gnome.Sys;

public class DynamicErrorJsonConverter : JsonConverter<DynamicError>
{
    public override DynamicError Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var properties = new Dictionary<string, object?>();
        string? code = null, message = null, target = null;
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.Comment)
                continue;

            if (reader.TokenType != JsonTokenType.PropertyName)
                break;

            var propertyName = reader.GetString();
            if (propertyName is null)
                break;

            switch (propertyName)
            {
                case "code":
                case "Code":
                    reader.Read();
                    code = reader.GetString();
                    continue;

                case "target":
                case "Target":
                    reader.Read();
                    target = reader.GetString();
                    continue;

                case "message":
                case "Message":
                    reader.Read();
                    message = reader.GetString();
                    continue;
                case "innerError":
                case "InnerError":
                    reader.Read();

                    // properties.Add(propertyName, JsonSerializer.Deserialize<IInnerError>(ref reader, options));
                    continue;

                default:
                    reader.Read();
                    var current = properties;
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.StartArray:
                            var data = new JsonEnumerbleConverter().Read(
                                ref reader,
                                typeof(IEnumerable<object?>),
                                options);
                            if (data is null)
                                continue;

                            properties.Add(propertyName, data);
                            break;

                        case JsonTokenType.StartObject:
                            var obj = new JsonDictionaryConverter().Read(
                                ref reader,
                                typeof(Dictionary<string, object?>),
                                options);
                            if (obj is null)
                                continue;
                            properties.Add(propertyName, obj);
                            break;

                        case JsonTokenType.PropertyName:
#pragma warning disable S1854 // Unused assignments should be removed
                            propertyName = reader.GetString();
#pragma warning restore S1854 // Unused assignments should be removed
                            break;

                        case JsonTokenType.False:
                        case JsonTokenType.True:
                            current.Add(propertyName, reader.GetBoolean());
                            break;

                        case JsonTokenType.Number:
                            current.Add(propertyName, reader.GetInt32());
                            break;

                        case JsonTokenType.String:
                            current.Add(propertyName, reader.GetString());
                            break;

                        case JsonTokenType.Null:
                            reader.Read();
                            continue;

                        default:
                            break;
                    }

                    break;
            }
        }

        return new DynamicError(message, target) { Properties = properties, Code = code };
    }

    public override void Write(
        Utf8JsonWriter writer,
        DynamicError error,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        if (error.Message != null)
        {
            writer.WritePropertyName("message");
            writer.WriteStringValue(error.Message);
        }

        if (error.Code != null)
        {
            writer.WritePropertyName("code");
            writer.WriteStringValue(error.Code);
        }

        if (error.Target != null)
        {
            writer.WritePropertyName("target");
            writer.WriteStringValue(error.Target);
        }

        if (error.InnerError != null)
        {
            writer.WriteNull("innerError");
        }

        foreach (var kvp in error.Properties)
        {
            writer.WritePropertyName(kvp.Key);
            switch (kvp.Value)
            {
                case null:
                    writer.WriteNullValue();
                    break;

                case string s:
                    writer.WriteStringValue(s);
                    break;

                case bool b:
                    writer.WriteBooleanValue(b);
                    break;

                case short s:
                    writer.WriteNumberValue(s);
                    break;

                case int i:
                    writer.WriteNumberValue(i);
                    break;

                case long l:
                    writer.WriteNumberValue(l);
                    break;

                case double d:
                    writer.WriteNumberValue(d);
                    break;

                case decimal m:
                    writer.WriteNumberValue(m);
                    break;

                case float f:
                    writer.WriteNumberValue(f);
                    break;

                case DateTime dt:
                    writer.WriteStringValue(dt);
                    break;

                case IEnumerable<object?> enumerable:
                    new JsonEnumerbleConverter().Write(writer, enumerable, options);
                    break;

                case IDictionary<string, object?> dictionary:
                    new JsonDictionaryConverter().Write(writer, dictionary, options);
                    break;

                default:
                    writer.WriteStringValue(kvp.Value.ToString());
                    break;
            }
        }

        writer.WriteEndObject();
    }

    internal class JsonDictionaryConverter : JsonConverter<Dictionary<string, object?>>
    {
        public override Dictionary<string, object?>? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            var properties = new Dictionary<string, object?>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType == JsonTokenType.Comment)
                    continue;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    break;

                var propertyName = reader.GetString();
                if (propertyName is null)
                    break;

                reader.Read();
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartArray:
                        var data = new JsonEnumerbleConverter().Read(ref reader, typeof(IEnumerable<object?>), options);
                        if (data is null)
                            continue;

                        properties.Add(propertyName, data.ToList());

                        break;

                    case JsonTokenType.StartObject:
                        properties.Add(propertyName, this.Read(ref reader, typeToConvert, options));
                        break;

                    case JsonTokenType.False:
                    case JsonTokenType.True:
                        properties.Add(propertyName, reader.GetBoolean());
                        break;

                    case JsonTokenType.Number:
                        properties.Add(propertyName, reader.GetInt32());
                        break;

                    case JsonTokenType.String:
                        properties.Add(propertyName, reader.GetString());
                        break;

                    case JsonTokenType.Null:
                        properties.Add(propertyName, null);
                        break;

                    default:
                        throw new JsonException();
                }
            }

            return properties;
        }

#pragma warning disable S4019
        public void Write(
            Utf8JsonWriter writer,
            IDictionary<string, object?> value,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var kvp in value)
            {
                var next = kvp.Value;
                if (next is null)
                {
                    writer.WriteNull(kvp.Key);
                    continue;
                }

                if (next is IDictionary<string, object?> dictionary)
                {
                    writer.WritePropertyName(kvp.Key);
                    this.Write(writer, dictionary, options);
                    continue;
                }

                if (next is IDictionary map)
                {
                    var lookup = new Dictionary<string, object?>();
                    foreach (var key in map.Keys)
                    {
                        if (key is null)
                            continue;

                        var v2 = map[key];
                        if (v2 is null)
                            continue;

                        if (key is string k)
                        {
                            lookup.Add(k, v2);
                        }
                    }

                    this.Write(writer, value, options);
                }

                if (next is IEnumerable<object?> enumerable)
                {
                    writer.WritePropertyName(kvp.Key);
                    new JsonEnumerbleConverter().Write(writer, enumerable, options);
                    continue;
                }

                var t = next;
                switch (t)
                {
                    case bool b:
                        writer.WriteBoolean(kvp.Key, b);
                        break;

                    case short s:
                        writer.WriteNumber(kvp.Key, s);
                        break;

                    case int i:
                        writer.WriteNumber(kvp.Key, i);
                        break;

                    case long l:
                        writer.WriteNumber(kvp.Key, l);
                        break;

                    case string s:
                        writer.WriteString(kvp.Key, s);
                        break;

                    case double d:
                        writer.WriteNumber(kvp.Key, d);
                        break;

                    case decimal m:
                        writer.WriteNumber(kvp.Key, m);
                        break;

                    case float f:
                        writer.WriteNumber(kvp.Key, f);
                        break;

                    case DateTime dt:
                        writer.WriteString(kvp.Key, dt);
                        break;

                    default:
                        writer.WriteString(kvp.Key, t.ToString());
                        break;
                }
            }

            writer.WriteEndObject();
        }

        public override void Write(
            Utf8JsonWriter writer,
            Dictionary<string, object?> value,
            JsonSerializerOptions options)
        {
            this.Write(writer, (IDictionary<string, object?>)value, options);
        }
    }

    internal class JsonEnumerbleConverter : JsonConverter<IEnumerable<object?>>
    {
        public override IEnumerable<object?>? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            var list = new List<object?>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;

                if (reader.TokenType == JsonTokenType.Comment)
                    continue;

                switch (reader.TokenType)
                {
                    case JsonTokenType.StartArray:
                        list.Add(this.Read(ref reader, typeToConvert, options));
                        break;

                    case JsonTokenType.StartObject:
                        list.Add(new JsonDictionaryConverter().Read(
                            ref reader,
                            typeof(Dictionary<string, object?>),
                            options));
                        break;
                    case JsonTokenType.False:
                    case JsonTokenType.True:
                        list.Add(reader.GetBoolean());
                        break;

                    case JsonTokenType.Number:
                        list.Add(reader.GetInt32());
                        break;

                    case JsonTokenType.String:
                        list.Add(reader.GetString());
                        break;

                    case JsonTokenType.Null:
                        list.Add(null);
                        break;

                    default:
                        throw new JsonException();
                }
            }

            return list;
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<object?> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var next in value)
            {
                switch (next)
                {
                    case null:
                        writer.WriteNullValue();
                        continue;

                    case string s:
                        writer.WriteStringValue(s);
                        continue;

                    case bool b:
                        writer.WriteBooleanValue(b);
                        continue;

                    case short s:
                        writer.WriteNumberValue(s);
                        continue;

                    case int i:
                        writer.WriteNumberValue(i);
                        continue;

                    case long l:
                        writer.WriteNumberValue(l);
                        continue;

                    case double d:
                        writer.WriteNumberValue(d);
                        continue;

                    case decimal m:
                        writer.WriteNumberValue(m);
                        continue;

                    case float f:
                        writer.WriteNumberValue(f);
                        continue;

                    case DateTime dt:
                        writer.WriteStringValue(dt);
                        continue;

                    case IEnumerable<object?> enumerable:
                        this.Write(writer, enumerable, options);
                        continue;

                    case IDictionary<string, object?> dictionary:
                        new JsonDictionaryConverter().Write(writer, dictionary, options);
                        continue;

                    default:
                        writer.WriteStringValue(next.ToString());
                        break;
                }
            }

            writer.WriteEndArray();
        }
    }
}
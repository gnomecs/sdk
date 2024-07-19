using Gnome.Text.DotEnv;
using Gnome.Text.DotEnv.Document;

namespace Gnome.Sys;

public static class DotEnv
{
    public static EnvDocument Parse(string text, DotEnvSerializerOptions? options = null)
        => DotEnvSerializer.DeserializeDocument(text, options);

    public static EnvDocument Parse(Stream stream)
        => DotEnvSerializer.DeserializeDocument(stream);

    public static EnvDocument Parse(TextReader reader)
        => DotEnvSerializer.DeserializeDocument(reader);

    public static void Load(DotEnvLoadOptions options)
        => DotEnvLoader.Load(options);

    public static string Stringify(EnvDocument document, DotEnvSerializerOptions? options = null)
        => DotEnvSerializer.SerializeDocument(document);
}
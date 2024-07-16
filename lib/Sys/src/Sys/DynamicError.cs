using System.Text.Json.Serialization;

namespace Gnome.Sys;

[JsonConverter(typeof(DynamicErrorJsonConverter))]
public class DynamicError : Error
{
    public DynamicError(string? message = null, string? target = null, IInnerError? inner = null)
        : base(message, inner)
    {
        this.Target = target;
    }

    public Dictionary<string, object?> Properties { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}
using System.Text.Json.Serialization;

namespace Gnome.Sys;

public class ArgumentOutOfRangeError : ArgumentError
{
    public ArgumentOutOfRangeError(string paramName)
        : base($"Argument {paramName} is out of range.")
    {
        this.ParamName = paramName;
    }

    public ArgumentOutOfRangeError(string paramName, object? value)
        : base(paramName)
    {
        this.ActualValue = value;
    }

    public ArgumentOutOfRangeError(string paramName, string message)
        : base(paramName, message)
    {
    }

    public ArgumentOutOfRangeError(string paramName, object? value, string message)
        : base(paramName, message)
    {
        this.ActualValue = value;
    }

    public ArgumentOutOfRangeError(string paramName, string message, IInnerError inner)
        : base(paramName, message, inner)
    {
    }

    public ArgumentOutOfRangeError(ArgumentOutOfRangeException ex)
        : base(ex)
    {
        this.ActualValue = ex.ActualValue;
    }

    [JsonPropertyName("value")]
    public object? ActualValue { get; set; }

    public static implicit operator ArgumentOutOfRangeError(ArgumentOutOfRangeException ex)
        => new(ex);

    public override Exception ToException()
    {
        return this.Exception ?? new ArgumentOutOfRangeException(this.ParamName, this.ActualValue, this.Message);
    }
}
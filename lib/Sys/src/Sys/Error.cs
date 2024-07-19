using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Gnome.Sys;

public class Error : IError
{
    public Error(string? message = null, IInnerError? innerError = null, [CallerMemberName] string target = "")
    {
        this.Message = message ?? "Unknown error";
        this.InnerError = innerError;
        this.Target = target;
    }

    public static Func<Exception, Error> Convert { get; set; } = (ex) =>
    {
        if (ex is AggregateException aggEx)
            return new AggregateError(aggEx);

        if (ex is ArgumentNullException argNullEx)
            return new ArgumentNullError(argNullEx);

        if (ex is ArgumentOutOfRangeException argOutOfRangeEx)
            return new ArgumentOutOfRangeError(argOutOfRangeEx);

        if (ex is ArgumentException argEx)
            return new ArgumentError(argEx);

        return new ExceptionError(ex);
    };

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("target")]
    public virtual string? Target
    {
        get;
        protected set;
    }

    [JsonIgnore]
    public virtual string? StackTrace { get; protected set; }

    [JsonPropertyName("innerError")]
    public IInnerError? InnerError { get; set; }

    public static implicit operator Error(Exception ex)
        => Convert(ex);

    public virtual Exception ToException()
    {
        return new InvalidOperationException(this.Message);
    }

    public override string ToString()
    {
        return this.Message;
    }
}